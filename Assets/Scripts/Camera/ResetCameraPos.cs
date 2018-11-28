using UnityEngine;
using System.Collections;

public class ResetCameraPos : MonoBehaviour {
    Camera c;
    Vector3 initP;
    Quaternion initR;
	// Use this for initialization
	void Start () {
        c = GetComponent<Camera>();
        initP = c.transform.position;
        initR = c.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {

        if(GUI.Button(new Rect(120, 50, 200, 35), "ResetCamera"))
        {
            c.transform.position = initP;
            c.transform.rotation = initR;
        }

    }
}
