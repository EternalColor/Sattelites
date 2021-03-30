using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatteliteCreator : MonoBehaviour
{
    [SerializeField]
    private Attractor referenceAttractor;

    [SerializeField]
    private GameObject sattelitePrefab;

    [SerializeField]
    private Text distanceText;

    [SerializeField]
    private Text scaleText;

    [SerializeField]
    private Text roundsPerSecondText;

    [SerializeField]
    private Toggle orbitClockwise;

    [SerializeField]
    private SphereCollisionManager sphereCollisionManager;

    [SerializeField]
    public Text speedValueRotateSelfTextForSattelites;
    
    private void Awake() 
    {
        this.GetComponent<Button>().onClick.AddListener(this.Click);
    }

    private void Click()
    {
        GameObject sattelite = Object.Instantiate(this.sattelitePrefab, this.referenceAttractor.transform.position - new Vector3(float.Parse(this.distanceText.text), 0, float.Parse(this.distanceText.text)), Quaternion.identity);

        //Uniform scale must not be changed to non-uniform -> will crash the physics calculations since we assume its a sphere not a ellipsis
        float scale = float.Parse(scaleText.text);
        sattelite.transform.localScale = new Vector3(scale, scale, scale);

        sattelite.GetComponent<RotateAround>().Attractor = this.referenceAttractor;
        sattelite.GetComponent<RotateAround>().Clockwise = this.orbitClockwise.isOn;

        if(this.roundsPerSecondText.text != string.Empty)
        {
            sattelite.GetComponent<RotateSelf>().RoundsPerSecond = float.Parse(this.roundsPerSecondText.text);
        }

        sattelite.GetComponent<RotateSelf>().speedValueText = this.speedValueRotateSelfTextForSattelites;

        //Register the new satelite for collision
        sphereCollisionManager.RegisterRotateAround(sattelite.GetComponent<RotateAround>());
    }
}
