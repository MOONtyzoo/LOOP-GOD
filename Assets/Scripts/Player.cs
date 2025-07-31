using UnityEngine;

public class Player : MonoBehaviour
{
    private int trackNum = 2;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("MoveUp"))
        {
            Debug.Log("Move up!");
        }

        if (Input.GetButtonDown("MoveDown"))
        {
            Debug.Log("Move down!");
        }

        if (Input.GetButtonDown("Sword"))
        {
            Debug.Log("Sword!");
        }

        if (Input.GetButtonDown("Gun"))
        {
            Debug.Log("Gun!");
        }

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump!");
        }
    }
}
