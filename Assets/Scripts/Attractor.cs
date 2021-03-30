using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Objects that have a gravitational pull are Attractors, Attractor can "gravitate" RotateAround objects into orbital motion
public class Attractor : MonoBehaviour
{
    public float Mass 
    {
        get
        {
            return this.mass;
        }
        set
        {
            this.mass = value;
        }
    }

    [SerializeField]
    private float mass;

    [SerializeField]
    private Text massValueText;

    [SerializeField]
    private Text massInput;

    private void Update() 
    {
        if(this.massValueText != null)
        {
            this.massValueText.text = $"{this.mass}";
        }    
    }
}