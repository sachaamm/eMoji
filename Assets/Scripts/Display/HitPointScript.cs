using UnityEngine;
using System.Collections;

public class HitPointScript : MonoBehaviour {

    public float duration = 80;
    float d;

    float sc;
	// Use this for initialization
	void Start () {

       
        d = duration;
       // c = m.color;
        sc = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
        duration--;
        if (duration < 0) Destroy(this.gameObject);
        float o = duration / (float)d;
        float oo = o * sc;

        transform.localScale = new Vector3(oo, oo, oo);

	}
}
