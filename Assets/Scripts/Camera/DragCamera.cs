using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour {

    public float dragSpeed = 0.5f;
    Vector2 prev;
    SkinnedGUIOptions skinnedGui;

    //float prevX, prevY;
    // Use this for initialization
    void Start () {
        skinnedGui = GetComponent<SkinnedGUIOptions>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!skinnedGui.isOverRenderCam()) return;


        if (Input.GetMouseButton(1))
        {
           // transform.RotateAround(guy.transform.position, new Vector3(0, 1, 0), -(prevX - Input.mousePosition.x) * dragSpeed);

            float xDrag = -(prev.x - Input.mousePosition.x) * dragSpeed;
            float yDrag = -(prev.y - Input.mousePosition.y) * dragSpeed;

            Vector3 right = new Vector3(1, 0, 0);
            Vector3 up = new Vector3(0, 1, 0);

            transform.Translate(right * xDrag);
            transform.Translate(up * yDrag);
        }

       prev = Input.mousePosition;
       
    }
}
