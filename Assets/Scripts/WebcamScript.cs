using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WebcamScript : MonoBehaviour
{
    public static WebCamTexture webcamTexture;
    // Start is called before the first frame update

    void Start()
    {
        webcamTexture = new WebCamTexture();
        // webcamTexture.deviceName = devices[0].name;
        this.GetComponent<MeshRenderer>().material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
