using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HumanGrid2 : MonoBehaviour {

    public GameObject guy, fbxGuy;
    public GameObject pointCube;
    public GameObject childGroup;

    Vector3[] vertices;
    Mesh m;

    public float startY = 2;
    public float startZ = 0;
    SkinnedMeshRenderer smr;
    SkinnedFaceTracing sft;

    float loading = 0;
    // Use this for initialization
    void Start () {

       smr = fbxGuy.transform.GetComponent<SkinnedMeshRenderer>();
       sft = GetComponent<SkinnedFaceTracing>();

        m = smr.sharedMesh;
        
     
        vertices = m.vertices;


        DecorateMesh();
        //StartCoroutine(DecorateMesh());
    }

    void OnGUI()
    {


        if(GUI.Button(new Rect(100, 300, 120, 50), "Load Grid:" + loading))
        {
            vertices = m.vertices;

            //StartCoroutine(DecorateMesh());
            destroyGridCubes();
            DecorateMesh();
        }
        
    }

    IEnumerator buildingGridCoroutine()
    {
        loading = 0;
      
      
       
        yield return loading;

       // DecorateMesh();

    }

    void DecorateMesh()
    {
        Transform root = guy.transform.parent;
       //print(" lossyScale is" + parent.transform.lossyScale);

        Vector3 initScale = pointCube.transform.localScale;
        Vector3 lossyScale = childGroup.transform.lossyScale;

        childGroup.transform.position = guy.transform.position;

        Vector3 normalizeScale = new Vector3(initScale.x / lossyScale.x, initScale.y / lossyScale.y, initScale.z / lossyScale.z);

        sft.resetIndexLinks();//reset index links
  

        for (int i = 0; i < vertices.Length; i++)
        {
            // WE HAD TO TRACKING
            if(vertices[i].y > startY && vertices[i].z > startZ)
            {
                GameObject newCube = GameObject.Instantiate(pointCube);                
                newCube.transform.parent = childGroup.transform;
                newCube.transform.localScale = normalizeScale;
                // 100 car on est a 0.01
                newCube.transform.position = vertices[i] * guy.transform.localScale.x * 100 * root.transform.localScale.x + guy.transform.position;

                newCube.transform.name = "newCube" + i;
                sft.addIndexLink(i);
       
            }

            //loading++;    

        }

        childGroup.transform.rotation = new Quaternion(0, 0, 0, 0);
        sft.setVertices();
       // sft.linkToTrackingPosition();

        // WHEN INDEX LINKING IS DONE , WE CAN INIT THE MORPHING
        FaceMorpher morpher = GetComponent<FaceMorpher>();
        morpher.initMorphing();


    }


    // Update is called once per frame
    void Update () {
	
	}

    public void destroyGridCubes()
    {


        int nbChild = childGroup.transform.childCount;

        for (int i = 0; i < nbChild; i++)
        {
            Destroy(childGroup.transform.GetChild(i).gameObject);

        }

    }



    public void SwitchVisibleStatus()
    {
        int nbChild = childGroup.transform.childCount;

        for(int i = 0; i < nbChild; i++)
        {
            Transform point = childGroup.transform.GetChild(i);
            MeshRenderer mr = point.GetComponent<MeshRenderer>();
            bool status = mr.enabled;
            mr.enabled = !status;
        }

    }

    public bool returnVisibleStatus()
    {

        //int nbChild = parent.transform.childCount;
        // Transform point = parent.transform.GetChild(0);

        // MeshRenderer mr = point.GetComponent<MeshRenderer>();

        //return mr.enabled;       
        return true;
    }


    public void SwitchVisibleTracking()
    {
        
        Transform cleaner = GameObject.Find("Cleaner").transform;

        for(int i = 0; i < cleaner.childCount; i++)
        {

            Transform trackingPoint = cleaner.GetChild(i);
            MeshRenderer mr = trackingPoint.GetComponent<MeshRenderer>();
            bool status = mr.enabled;
            mr.enabled = !status;
        }
        

    }
}
