using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private float distance = 0.0f;
    private float speed = 4f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Error attempting to instantiate singleton \"GameManager\", There is already one in the scene!");
        }
    }

    private void Update()
    {
        distance += speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            speed += 1f;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            speed -= 1f;
        }
    }

    public float GetDistance() => distance;
    public float GetSpeed() => speed;

    public void IncreaseSpeed(float amount)
    {
        SetSpeed(speed + amount);
    }

    public void DecreaseSpeed(float amount)
    {
        SetSpeed(speed - amount);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = Mathf.Clamp(newSpeed, 0.0f, Mathf.Infinity);
    }
}
