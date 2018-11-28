using UnityEngine;
using System.Collections;

public class EyeFocusMover : MonoBehaviour {
    Vector3 pos;
    public float e = 1;
	// Use this for initialization
	void Start () {
        pos = transform.position;
        getRandomPos();
	}

    public void getRandomPos()
    {
        float a = Random.Range(-e, e);
        float b = Random.Range(-e/2, e/2);
        transform.position = pos + new Vector3(a, b, 0);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
