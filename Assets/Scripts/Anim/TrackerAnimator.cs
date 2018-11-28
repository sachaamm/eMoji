using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class TrackerAnimator : MonoBehaviour {
    public GameObject trackerCenter;
    bool isAnimating;
    Mesh m;
    FaceTracing faceTracing;
    GUIOptions guiOptions;



    List<int> vertexIndexes;
    Vector3[] distances;
    int[] trackingValues;


    public GameObject guy;
    List<Vector3> meshVertices;

    public GameObject yMouthSlider, zMouthSlider;

    //Vector3 trackingCenter;


	// Use this for initialization
	void Start () {

        vertexIndexes = new List<int>();
        faceTracing = GetComponent<FaceTracing>();

        // StartAnimation();
        MeshFilter mr = guy.GetComponent<MeshFilter>();
        m = mr.mesh;
  
        //vertexIndexes = faceTracing.returnVertexIndexes();
        meshVertices = new List<Vector3>();

        for (int i = 0; i < m.vertices.Length; i++)
        {
            meshVertices.Add(m.vertices[i]);
        }


        guiOptions = GetComponent<GUIOptions>();
    }

    public void StartAnimation()
    {
       // if (vertexIndexes.Count > 10) faceTracing.clearTracking();
        print("startAnimation");
        print("Vertex Index Count : " + vertexIndexes.Count);
        
        resetDistances();
        vertexIndexes = new List<int>();
       // 
        vertexIndexes = faceTracing.returnVertexIndexes();
        trackingValues = faceTracing.returnTrackingValues();
        distances = new Vector3[vertexIndexes.Count];
        print("Vertex Index Count : " + vertexIndexes.Count);
        trackerCenter.transform.position = getTrackingCenter();
        calcDistances();
        


    }

    void calcDistances()
    {
       for(int i = 0; i < vertexIndexes.Count; i++)
        {
            distances[i] = trackerCenter.transform.position - m.vertices[vertexIndexes[i]];
        }
    }

    void resetDistances()
    {
        for (int i = 0; i < vertexIndexes.Count; i++)
        {
            distances[i] = new Vector3(0,0,0);
        }

    }


    public void clearTracker()
    {

        vertexIndexes = new List<int>();
        trackingValues = new int[0];

    }
    // Update is called once per frame
    void Update () {


        // meshVertices = m.vertices;
        float yMouth = yMouthSlider.GetComponent<Slider>().value;
        float zMouth = zMouthSlider.GetComponent<Slider>().value;



       // print(yMouth);

        // SI ON EST PAS EN EDITABLE , CAD AJOUT DE POINT DE TRACKING
        if (!guiOptions.returnEditable())
        {

            int modificatorMode = guiOptions.returnModificatorModeValue();

            switch (modificatorMode)
            {

                case 0:
                    // y AND Z TRANSLATION MODE
                    for (int i = 0; i < vertexIndexes.Count; i++)
                    {
                        // distances[i] = trackerCenter.transform.position - m.vertices[vertexIndexes[i]];
                        float mapStrength = trackingValues[i] / (float)100;
                        Vector3 pos = trackerCenter.transform.position - distances[i] + new Vector3(0, yMouth * mapStrength, zMouth * mapStrength);
                        meshVertices[vertexIndexes[i]] = pos;
                    }


                    break;


                case 1:
                    // CURVE Y
                    for (int i = 0; i < vertexIndexes.Count; i++)
                    {
                        float curveStrength = 5;
                        // distances[i] = trackerCenter.transform.position - m.vertices[vertexIndexes[i]];
                        // float mapStrength = trackingValues[i] / (float)100;

                        Vector3 pos = trackerCenter.transform.position - distances[i] + new Vector3(0, yMouth * Mathf.Abs(distances[i].x) * curveStrength, 0);
                        meshVertices[vertexIndexes[i]] = pos;
                    }



                    break;




            }
            /*
        
            */
            // 



      

            m.SetVertices(meshVertices);
            m.RecalculateBounds();
            m.RecalculateNormals();


        }
  


    }

    Vector3 getTrackingCenter()
    {
        Vector3 v = new Vector3(0, 0, 0);


        for(int i = 0; i < vertexIndexes.Count; i++)
        {
            v += m.vertices[vertexIndexes[i]];
        }

        return guy.transform.position + new Vector3(v.x/vertexIndexes.Count, v.y/vertexIndexes.Count, v.z/vertexIndexes.Count);
        //return new Vector3(0, 0, 0);
    }
}
