using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateSelf : MonoBehaviour
{
    public float RoundsPerSecond;

    [SerializeField]
    public Text speedValueText;

    private void FixedUpdate()
    {
        float rotationSpeed = this.RoundsPerSecond * 360f;

        //Usually this will be the y axis, but we rotate n * 360 degrees around the up axis
        this.transform.Rotate(this.transform.up, rotationSpeed * Time.fixedDeltaTime);
    }

    private void Update() 
    {
        if(this.speedValueText)
        {
            this.speedValueText.text = $"RPS:{this.RoundsPerSecond}";    
        }
    }
}
