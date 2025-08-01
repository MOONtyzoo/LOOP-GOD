using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnHit;

    private TrackBody trackBody;
    private Hitbox hitbox;

    [Header("Jump")]
    [SerializeField] private float jumpHoldTime;
    [SerializeField] private float weakGravity;
    [SerializeField] private float strongGravity;
    [SerializeField] private float jumpPushdownForce;
    [SerializeField] private float initialJumpVal;
    [SerializeField] private float initialVelocityY;


    [Header("Jump Visual")]
    [SerializeField] private GameObject playerVisualBody;
    [SerializeField] private GameObject playerVisualShadow;
    [SerializeField] private float jumpDefaultOffsetY;
    [SerializeField] private float jumpPeakOffsetY;
    [SerializeField] private float jumpDefaultScale;
    [SerializeField] private float jumpPeakScale;
    [SerializeField] private float jumpDefaultShadowScale;
    [SerializeField] private float jumpPeakShadowScale;
    private float jumpVal = 0.0f;
    private bool isJumping = false;

    [Header("Sword")]
    [SerializeField] private Hurtbox swordHurtbox;
    [SerializeField] private float swordDuration;
    private bool isUsingSword = false;

    private float horizontalInput;
    private InputButton MoveUpButton;
    private InputButton MoveDownButton;
    private InputButton JumpButton;
    private InputButton SwordButton;
    private InputButton GunButton;

    private void Awake()
    {
        trackBody = GetComponentInChildren<TrackBody>();
        hitbox = GetComponentInChildren<Hitbox>();
        swordHurtbox.Disable();

        trackBody.SetTrack(2);

        MoveUpButton = new InputButton("MoveUp", 0.5f * trackBody.GetTrackSwitchDuration());
        MoveDownButton = new InputButton("MoveDown", 0.5f * trackBody.GetTrackSwitchDuration());
        SwordButton = new InputButton("Sword", 0.0f);
        GunButton = new InputButton("Gun", 0.0f);
        JumpButton = new InputButton("Jump", 0.0f);

        hitbox.OnHit += Hitbox_OnHit;
    }

    private void Update()
    {
        HandleInput();
        AnimateJump();
    }

    private void FixedUpdate()
    {
        trackBody.MoveHorizontal(horizontalInput);
    }

    private void Hitbox_OnHit()
    {
        OnHit?.Invoke();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        UpdateInputButtons();

        if (MoveUpButton.WasPressed() && trackBody.CanMoveUp())
        {
            trackBody.MoveUp();
        }

        if (MoveDownButton.WasPressed() && trackBody.CanMoveDown())
        {
            trackBody.MoveDown();
        }

        if (JumpButton.WasPressed() && CanJump())
        {
            JumpButton.ClearBuffer();
            Jump();
        }

        if (SwordButton.WasPressed() && CanUseSword())
        {
            UseSword();
        }

        if (GunButton.WasPressed())
        {
            Debug.Log("Gun!");
        }
    }

    private void UpdateInputButtons()
    {
        MoveUpButton.Update();
        MoveDownButton.Update();
        SwordButton.Update();
        GunButton.Update();
        JumpButton.Update();
    }

    private bool CanJump() => isJumping == false;
    private void Jump() => StartCoroutine(JumpCoroutine());
    private IEnumerator JumpCoroutine()
    {
        isJumping = true;
        hitbox.SetImmunityLevel(ImmunityLevels.Air);

        float jumpTimer = 0.0f;
        bool jumpCanceled = false;
        float currentGravity = weakGravity;
        float velocityY = initialVelocityY;
        jumpVal = initialJumpVal;

        while (jumpVal > 0)
        {
            velocityY += currentGravity * Time.deltaTime;
            jumpVal += velocityY * Time.deltaTime;
            jumpTimer += Time.deltaTime;

            bool jumpEndedEarly = JumpButton.WasReleased();
            bool jumpTimeout = jumpTimer > jumpHoldTime;
            if (!jumpCanceled && (jumpEndedEarly || jumpTimeout))
            {
                jumpCanceled = true;
                velocityY -= jumpPushdownForce;
                currentGravity = strongGravity;
            }

            yield return new WaitForEndOfFrame();
        }

        jumpVal = 0.0f;
        isJumping = false;
        hitbox.SetImmunityLevel(ImmunityLevels.Ground);
    }

    private bool CanUseSword() => !isUsingSword;
    private void UseSword() { StartCoroutine(SwordCoroutine()); }
    private IEnumerator SwordCoroutine()
    {
        isUsingSword = true;
        swordHurtbox.Enable();

        float timer = 0.0f;
        while (timer < swordDuration)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isUsingSword = false;
        swordHurtbox.Disable();
    }

    private void AnimateJump()
    {
        float jumpOffsetY = Mathf.Lerp(jumpDefaultOffsetY, jumpPeakOffsetY, jumpVal);
        float jumpScale = Mathf.Lerp(jumpDefaultScale, jumpPeakScale, jumpVal);
        float jumpShadowScale = Mathf.Lerp(jumpDefaultShadowScale, jumpPeakShadowScale, jumpVal);

        playerVisualBody.transform.localPosition = new Vector2(playerVisualBody.transform.localPosition.x, jumpOffsetY);
        playerVisualBody.transform.localScale = new Vector3(jumpScale, jumpScale, 1.0f);
        playerVisualShadow.transform.localScale = new Vector3(jumpShadowScale, jumpShadowScale, 1.0f);
    }

    public float GetJumpVal() => jumpVal;
}
