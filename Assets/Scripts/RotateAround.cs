using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateAround : MonoBehaviour
{
    public bool Clockwise { get; set; }

    public Quadrant Quadrant => this.quadrant;

    private Quadrant quadrant;

    private const float GRAVITATIONAL_FACTOR = 9.81f;

    [SerializeField]
    public Attractor Attractor;

    [SerializeField]
    private Text speedValueText;

    private float currentXSpeed;
    private float currentZSpeed;

    private float currentTimeInMilliseconds = 0;

    private void FixedUpdate()
    {   
        float distance = Vector3.Distance(this.transform.position, this.Attractor.transform.position);

        //Prevent Time from overflowing
        this.currentTimeInMilliseconds += Time.fixedDeltaTime;

        //Velocity calculated according to kepplers motion law -> the higher the mass of the planet the higher the velocity will be.
        this.currentXSpeed = Mathf.Sqrt((RotateAround.GRAVITATIONAL_FACTOR * Attractor.Mass) / this.Attractor.transform.localScale.x);
        this.currentZSpeed = Mathf.Sqrt((RotateAround.GRAVITATIONAL_FACTOR * Attractor.Mass) / this.Attractor.transform.localScale.z);
        
        if(this.Clockwise)
        {
            this.currentXSpeed = -this.currentXSpeed;
            this.currentZSpeed = -this.currentZSpeed;    
        }
        
        float x = Mathf.Cos(currentTimeInMilliseconds * this.currentXSpeed * Time.fixedDeltaTime) * distance;
        float z = Mathf.Sin(currentTimeInMilliseconds * this.currentZSpeed * Time.fixedDeltaTime) * distance;

        this.transform.position = new Vector3(x, 0, z);
    }

    private void Update()
    {
        //Can be null
        if(this.speedValueText != null)
        {
            this.speedValueText.text = $"X: {this.currentXSpeed} Y: 0 Z: {this.currentZSpeed}";
        }
    }
}
