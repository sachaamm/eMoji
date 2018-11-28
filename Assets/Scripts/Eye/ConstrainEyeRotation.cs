using UnityEngine;
using System.Collections;

public class ConstrainEyeRotation : MonoBehaviour {
    public GameObject eye,eyeB;
    public GameObject eyeGoal,eyeGoalB;

    public float yConsMin = 0;
    public float yConsMax = 356;

   MoveEye moveEye,moveEyeB;
	// Use this for initialization
	void Start () {
        moveEye = eye.GetComponent<MoveEye>();
        moveEyeB = eyeB.GetComponent<MoveEye>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        Vector3 e = eye.transform.rotation.eulerAngles;
        int yv = (int)e.y;

        Vector3 eB = eyeB.transform.rotation.eulerAngles;
        int yvB = (int)eB.y;

       // GUI.Button(new Rect(400, 50, 300, 50), "Eye Rotation: Y " + yv);




        if(yv < yConsMax && eye.transform.position.y > eyeGoal.transform.position.y )
        {
            moveEye.setConstrain(true);
            moveEyeB.setConstrain(true);
        }
        else
        {
            moveEye.setConstrain(false);
            moveEyeB.setConstrain(false);
        }




        if (yvB < yConsMax && eyeB.transform.position.y > eyeGoalB.transform.position.y)
        {
          //  moveEyeB.setConstrain(true);
        }
        else
        {
           // moveEyeB.setConstrain(false);
        }


    }
}
