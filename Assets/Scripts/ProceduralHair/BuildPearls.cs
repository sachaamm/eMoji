using UnityEngine;
using System.Collections;

public class BuildPearls : MonoBehaviour {
    public GameObject point;
    int nbPoints = 12;
    int nbLines = 5;
    // Use this for initialization
    void Start()
    {

        float angle = Mathf.PI;
        //float x = 0;
        //float y = 0;
        float rY = 1;
        float rZ = 2;

        float[] ratios = new float[nbLines];
        float addRatio = 0.2f;
        float minRatio = 0.2f;
        float currentRatio = minRatio;

        int middle = nbLines - 1 / 2;

        for(int j = 0; j < nbLines; j++)
        {
            if(j <= middle)
            {
                currentRatio += addRatio;
            }
            else
            {
                currentRatio -= addRatio;
            }

            ratios[j] = currentRatio;
            

        }


        float interval = 0.8f ;

        for(int k = 0; k < nbLines; k++)
        {

            for (int i = 0; i < nbPoints; i++)
            {
                GameObject nPoint = GameObject.Instantiate(point);
                nPoint.transform.position = new Vector3(0, 0, 0) + new Vector3( k * interval , rY * Mathf.Sin(angle) * ratios[k], rZ * Mathf.Cos(angle) * ratios[k]);

                angle -= Mathf.PI / (nbPoints - 1);
            }



        }


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
