using UnityEngine;
using System.Collections;

public class NavigateCamera : MonoBehaviour {
    public GameObject mainCam;
    public GameObject guy;
    public float zoomSpeed = 0.001f;
    SkinnedGUIOptions skinnedGui;

    // Use this for initialization
    void Start () {
        skinnedGui = GetComponent<SkinnedGUIOptions>();

	}

    // Update is called once per frame
    void Update() {

        if (!skinnedGui.isOverRenderCam()) return;

       
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
          {
            Vector3 diff = guy.transform.position - mainCam.transform.position;
          
            mainCam.transform.Translate(diff * zoomSpeed);
        //    mainCam.transform.LookAt(guy.transform.position);

        }


        if (Input.GetAxis("Mouse ScrollWheel") < 0) // forward
        {
            Vector3 diff = guy.transform.position - mainCam.transform.position;
     
            mainCam.transform.Translate(-diff * zoomSpeed);
         //   mainCam.transform.LookAt(guy.transform.position);


        }

    }




}
