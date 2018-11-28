using UnityEngine;
using System.Collections;

public class MoveEyeGoal : MonoBehaviour {

    // MOVING VAR
    private Transform myTransform;
    public Transform target;
    public float moveSpeed = 3.0f;
    public float rotationSpeed = 3.0f;
    public float range = 10.0f;
    public float range2 = 10.0f;
    public float stop = 0.0f;
    //FT_Action action;

    EyeFocusMover moveFocus;
    //bool pause = false;

    // Use this for initialization
    void Start()
    {
        myTransform = transform;
        moveFocus = target.GetComponent<EyeFocusMover>();
        
    }

    public void setPause(bool b)
    {

        //pause = b;
    }

    // Update is called once per frame
    void Update()
    {

    //    if (pause) return;


        //rotate to look at the player
        float distance = Vector3.Distance(myTransform.position, target.position);
        if (distance <= range2 && distance >= range)
        {
//
            //if (!pause)
          //  {
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                                           Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
           // }
        }
        else if (distance <= range && distance > stop)
        {

            //move towards the player // aller a l'objectif
           // if (!pause)
          //  {
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                                           Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
           // }
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        }
        else if (distance <= stop)
        {

            moveFocus.getRandomPos();






        }




    }
}
