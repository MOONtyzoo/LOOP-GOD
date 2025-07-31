using System.Collections.Generic;
using UnityEngine;

public class InputButton
{
    private string buttonName;
    private bool valueLastFrame = false;
    private bool valueThisFrame = false;
    private float bufferTime = 0.0f;

    private float timeSinceLastPress = Mathf.Infinity;

    public InputButton(string buttonName, float bufferTime = 0.0f)
    {
        this.buttonName = buttonName;
        this.bufferTime = bufferTime;
    }

    public void Update()
    {
        valueLastFrame = valueThisFrame;
        valueThisFrame = Input.GetButton(buttonName);

        timeSinceLastPress += Time.deltaTime;
        if (valueLastFrame == false && valueThisFrame == true) timeSinceLastPress = 0.0f;
    }

    public bool WasPressed() => timeSinceLastPress <= bufferTime;
    public bool IsPressed() => Input.GetButton(buttonName);
    public bool WasReleased() => valueLastFrame == true && valueThisFrame == false;

    public void ClearBuffer() => timeSinceLastPress = Mathf.Infinity;
}