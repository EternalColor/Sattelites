using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetEarth : MonoBehaviour
{
    [SerializeField]
    private GameObject earth;

    [SerializeField]
    public Text massInputText;

    [SerializeField]
    public Text roundsPerSecondInputText;

    private void Awake() 
    {
        this.GetComponent<Button>().onClick.AddListener(this.SetEarthValues); 
    }

    private void SetEarthValues()
    {
        if(this.massInputText.text != string.Empty)
        {
            earth.GetComponent<Attractor>().Mass = float.Parse(this.massInputText.text);
        }

        if(this.roundsPerSecondInputText.text != string.Empty)
        {
            earth.GetComponent<RotateSelf>().RoundsPerSecond =  float.Parse(this.roundsPerSecondInputText.text);
        }
    }
}
