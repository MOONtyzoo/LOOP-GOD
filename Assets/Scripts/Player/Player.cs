using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnHit;

    private TrackBody trackBody;
    private HeightVisual heightVisual;
    private Hitbox hitbox;
    private PlayerGun gun;

    [Header("Jump")]
    [SerializeField] private float jumpHoldTime;
    [SerializeField] private float weakGravity;
    [SerializeField] private float strongGravity;
    [SerializeField] private float jumpPushdownForce;
    [SerializeField] private float initialJumpVal;
    [SerializeField] private float initialVelocityY;
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
        heightVisual = GetComponentInChildren<HeightVisual>();
        hitbox = GetComponentInChildren<Hitbox>();
        gun = GetComponentInChildren<PlayerGun>();
        swordHurtbox.Disable();

        trackBody.SetTrack(2);
        gun.SetAmmo(3);

        MoveUpButton = new InputButton("MoveUp", 0.5f * trackBody.GetTrackSwitchDuration());
        MoveDownButton = new InputButton("MoveDown", 0.5f * trackBody.GetTrackSwitchDuration());
        SwordButton = new InputButton("Sword", 0.0f);
        GunButton = new InputButton("Gun", 0.0f);
        JumpButton = new InputButton("Jump", 0.0f);

        swordHurtbox.OnHit += SwordHurtbox_OnHit;
        hitbox.OnHit += Hitbox_OnHit;
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        trackBody.MoveHorizontal(horizontalInput);
    }

    private void Hitbox_OnHit()
    {
        OnHit?.Invoke();
    }

    private void SwordHurtbox_OnHit()
    {
        gun.Reload();
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

        if (GunButton.WasPressed() && gun.HasAmmo())
        {
            gun.Shoot();
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
        float jumpVal = initialJumpVal;

        while (jumpVal > 0)
        {
            velocityY += currentGravity * Time.deltaTime;
            jumpVal += velocityY * Time.deltaTime;
            heightVisual.SetHeight(jumpVal);
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

        heightVisual.SetHeight(0.0f);
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

    public int GetTrack() => trackBody.GetTrack();
}
