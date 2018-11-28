using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

    public GameObject guy;

    float prevX = 0;
    public float dragSpeed = 0.5f;

    float prevY = 0;

    public GameObject focus;

    SkinnedGUIOptions skinnedGui;


    // Use this for initialization
    void Start () {
        prevX = Input.mousePosition.x;
        prevY = Input.mousePosition.y;
        skinnedGui = GetComponent<SkinnedGUIOptions>();


    }

    // Update is called once per frame
    void Update () {

        if (!skinnedGui.isOverRenderCam()) return;


        if (Input.GetMouseButton(2))
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                transform.RotateAround(guy.transform.position, new Vector3(0, 1, 0), -(prevX - Input.mousePosition.x) * dragSpeed);

            }
            else
            {
                   transform.RotateAround(focus.transform.position, new Vector3(1, 0, 0), -(prevY - Input.mousePosition.y) * dragSpeed);
                //   transform.RotateAround(guy.transform.position, new Vector3(1, 1, 0), -(prevY - Input.mousePosition.y) * dragSpeed);
                //transform.RotateAround(guy.transform.position, new Vector3(1, 1, 0), -(prevY - Input.mousePosition.y) * dragSpeed);


            }

        }

        prevX = Input.mousePosition.x;
        prevY = Input.mousePosition.y;
    }
}
