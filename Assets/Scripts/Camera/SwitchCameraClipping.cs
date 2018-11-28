using UnityEngine;
using System.Collections;

public class SwitchCameraClipping : MonoBehaviour {
    Camera c;
    int step = 0;
    public float farValue = 0.1f;
    public float nearValue = 0.01f;
	// Use this for initialization
	void Start () {
        c = GetComponent<Camera>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {

        string s = "Far";
        if (step % 2 == 0)
        {
            s = "Near";
        }
        if (GUI.Button(new Rect(150, 0, 200, 35), "CameraClipping:" + s))
        {

            step++;

            if (step % 2 == 1)
            {
                
                c.nearClipPlane = farValue;
            }
            else
            {

                c.nearClipPlane  = nearValue;
            }
        }


    }
}
