using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMoon : MonoBehaviour
{
    [SerializeField]
    private GameObject moon;

    [SerializeField]
    public Text roundsPerSecondInputText;

    private void Awake() 
    {
        this.GetComponent<Button>().onClick.AddListener(this.SetEarthValues); 
    }

    private void SetEarthValues()
    {
        if(this.roundsPerSecondInputText.text != string.Empty)
        {
            moon.GetComponent<RotateSelf>().RoundsPerSecond =  float.Parse(this.roundsPerSecondInputText.text);
        }
    }
}
