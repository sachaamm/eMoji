using UnityEngine;
using System.Collections;

public class RaytracingDebug : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() {
        // create a ray going into the scene from the screen location the user clicked at
        var up = transform.TransformDirection(Vector3.up);




        RaycastHit hit;
        Debug.DrawRay(transform.position, -up * 2, Color.green);

        if (Physics.Raycast(transform.position, -up, out hit, 2))
        {

            Debug.Log("HIT");

            if (hit.collider.gameObject.name == "floor")
            {
                Destroy(GetComponent("Rigidbody"));
            }
        }

    }
}
