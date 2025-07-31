using System.Collections.Generic;
using UnityEngine;

public class InputButton
{
    private string buttonName;
    private bool valueLastFrame = false;
    private bool valueThisFrame = false;
    private float bufferTime = 0.0f;

    private float timeSinceLastPress = 9999.0f;

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

    public bool WasReleased() => valueLastFrame == true && valueThisFrame == false;
}