using UnityEngine;
using System.Collections;

public class MoveEye : MonoBehaviour {

	// MOVING VAR
	private Transform myTransform;
	public Transform target;
	public float moveSpeed = 3.0f;
	public float rotationSpeed = 3.0f; 
	public float range = 10.0f;
	public float range2 = 10.0f;
	public float stop = 0.0f;
    //FT_Action action;
    bool constrain = false;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		//action  = (FT_Action)GetComponent(typeof(FT_Action)); 
	}
	
    public void setConstrain(bool b)
    {
        constrain = b;
    }
	// Update is called once per frame
	void FixedUpdate () {
	

 if(constrain)return;

		//rotate to look at the player
		float distance = Vector3.Distance(myTransform.position, target.position);
		if (distance <= range2 && distance >= range) {
			
			myTransform.rotation = Quaternion.Slerp (myTransform.rotation,
			                                         Quaternion.LookRotation (target.position - myTransform.position), rotationSpeed * Time.deltaTime);
		} else if (distance <= range && distance > stop) {
			
			//move towards the player // aller a l'objectif
			myTransform.rotation = Quaternion.Slerp (myTransform.rotation,
			                                         Quaternion.LookRotation (target.position - myTransform.position), rotationSpeed * Time.deltaTime);
			myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
		} else if (distance <= stop) {


       
			




		}




	}

	public void setTarget(Transform t){

		target = t;

	}

	public Transform returnTarget(){

		return target;
	}
}
