using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class SkinnedFaceModeller : MonoBehaviour {

    public GameObject yMouthSlider, zMouthSlider;
    public GameObject sawStrengthSlider;
    public GameObject trackerCenter;
    public GameObject fbxGuy;

    SkinnedMeshRenderer smr;
    SkinnedFaceTracing faceTracing;
    SkinnedGUIOptions guiOptions;
    BakedAnimationController bakedAnimationController;



    bool isAnimating;

    Mesh m;
 
    List<int> vertexIndexes;
    List<Vector3> meshVertices;
    Vector3[] distances;
    int[] trackingValues;


    List<Vector3[]> allDistances;



    public GameObject debug;
    // Use this for initialization
    void Start () {

        vertexIndexes = new List<int>();
        meshVertices = new List<Vector3>();
        faceTracing = GetComponent<SkinnedFaceTracing>();
        smr = fbxGuy.GetComponent<SkinnedMeshRenderer>();
        m = smr.sharedMesh;
    
        for (int i = 0; i < m.vertices.Length; i++)
        {
            meshVertices.Add(m.vertices[i]);
        }
 
        guiOptions = GetComponent<SkinnedGUIOptions>();
        bakedAnimationController = GetComponent<BakedAnimationController>();

    }

    
    public void StartAnimation()
    {
        resetDistances();
        vertexIndexes = new List<int>();
        vertexIndexes = faceTracing.returnVertexIndexes();
        trackingValues = faceTracing.returnTrackingValues();
        distances = new Vector3[vertexIndexes.Count];
        //Vector3 tpos = getTrackingCenter();
        //print("Tracking center "+tpos);
        trackerCenter.transform.position = getTrackingCenter();
        calcDistances();
           
    }

    void calcDistances()
    {
        print("calc Distances ");
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

        float yMouth = yMouthSlider.GetComponent<Slider>().value;
        float zMouth = zMouthSlider.GetComponent<Slider>().value;
        float sawStrength = sawStrengthSlider.GetComponent<Slider>().value;
        // SI ON EST EN EDIT MODE OU EN BAKED ANIM , ON MODIFIE LE MESH
        if (guiOptions.returnModeValue() == 1) calculModeling(yMouth, zMouth,sawStrength);

        if (guiOptions.returnModeValue() == 3  && !bakedAnimationController.isPlaying) calculModeling(yMouth, zMouth, sawStrength);


    }

    // CALCULATE ALL DISTANCES FOR BAKED ANIM
    public void calculateAllDistances(List<int[]> indexes,List<Vector3> centers)
    {

        allDistances = new List<Vector3[]>();

        for(int j = 0; j < indexes.Count; j++)
        {
            Vector3[] newDistances = new Vector3[indexes[j].Length];
            int[] vindexes = indexes[j];

            for (int i = 0; i < indexes[j].Length; i++)
            {
                newDistances[i] = centers[j] - m.vertices[vindexes[i]]*fbxGuy.transform.localScale.x;
              
                allDistances.Add(newDistances);
            }

        }
   

    }

    public void modelateVerticeList(List<int[]> indexes,float[] values,List<int[]> strengths,List<Vector3> centers)
    {

        float scaling = fbxGuy.transform.localScale.x;
        //float yMouth = yMouthSlider.GetComponent<Slider>().value;
        //float zMouth = zMouthSlider.GetComponent<Slider>().value;
        //float sawStrength = sawStrengthSlider.GetComponent<Slider>().value;


        //int length = indexes.Count;
        
        for(int i = 0; i < indexes.Count; i++)
        {

            int[] vertexIndexes = indexes[i];
            int[] strengthsValues = strengths[i];
            Vector3[] currentDistances = allDistances[i];
            //print("strength length " + strengthsValues.Length);

            for (int j = 0; j < indexes[i].Length; j++)
            {          
                float mapStrength = strengthsValues[j] / (float)100;
                Vector3 pos = centers[i] - currentDistances[j];
                 meshVertices[vertexIndexes[j]] = (pos / scaling) + new Vector3(0, values[i] * mapStrength, 0) / scaling;

            }


            }


        m.SetVertices(meshVertices);
        m.RecalculateBounds();
        m.RecalculateNormals();

    }

    void calculModeling(float yMouth,float zMouth,float sawStrength)
    {


        if (Time.frameCount % 1000 == 0)
        {


            print("<<<< CalculModeling >>>>");


        }



        int modificatorMode = guiOptions.returnModificatorModeValue();
        float scaling = fbxGuy.transform.localScale.x;

        switch (modificatorMode)
        {

            case 0:
                // y AND Z TRANSLATION MODE
                for (int i = 0; i < vertexIndexes.Count; i++)
                {
                    float mapStrength = trackingValues[i] / (float)100;
                    Vector3 pos = trackerCenter.transform.position - distances[i] + new Vector3(0, yMouth * mapStrength / scaling, zMouth * mapStrength / scaling);
                    meshVertices[vertexIndexes[i]] = pos;
                }

                break;


            case 1:
                // Y SAW MODE
                //print("saw mode");
                for (int i = 0; i < vertexIndexes.Count; i++)
                {
                    Vector3 pos = trackerCenter.transform.position - distances[i] + new Vector3(0, 100 * yMouth * Mathf.Abs(distances[i].x) * sawStrength / scaling, 0);
                    meshVertices[vertexIndexes[i]] = pos;
                }

                break;

            case 2:

                break;


            case 3:
                // Y SAW MODE
                //print("expand mode");
                for (int i = 0; i < vertexIndexes.Count; i++)
                {
                    Vector3 pos = trackerCenter.transform.position - distances[i] * yMouth * 66;
                    meshVertices[vertexIndexes[i]] = pos;
                }

                break;

        }

        m.SetVertices(meshVertices);
        m.RecalculateBounds();
        m.RecalculateNormals();


    }

    public void syncVertices(List<Vector3> _vertices)
    {
        meshVertices = _vertices;
    }

    Vector3 getTrackingCenter()
    {
        Vector3 v = new Vector3(0, 0, 0);

        for(int i = 0; i < vertexIndexes.Count; i++)
        {
            v += m.vertices[vertexIndexes[i]];
        }

        return fbxGuy.transform.position + new Vector3(v.x/vertexIndexes.Count, v.y/vertexIndexes.Count, v.z/vertexIndexes.Count);
    }


    public Vector3 getRealTrackingCenter(List<int> vertexIndex)
    {
        Vector3 v = new Vector3(0, 0, 0);


        for (int i = 0; i < vertexIndex.Count; i++)
        {
            v += m.vertices[vertexIndex[i]]*fbxGuy.transform.localScale.x;

          // v += m.vertices[vertexIndex[i]] ;
        }

        return fbxGuy.transform.position + new Vector3(v.x / vertexIndex.Count, v.y / vertexIndex.Count, v.z / vertexIndex.Count);
    }
}
