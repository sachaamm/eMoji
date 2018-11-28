using UnityEngine;
using System.Collections;

public class HumanGrid : MonoBehaviour {

    public GameObject guy;
    public GameObject pointCube;
    public GameObject parent;


    Vector3[] vertices;
    Mesh m;


    public float startY = 2;

    // Use this for initialization
    void Start () {

        MeshFilter mf = guy.transform.GetComponent<MeshFilter>();
        m = mf.mesh;
        vertices = m.vertices;

        DecorateMesh();

    }


    void DecorateMesh()
    {

        for (int i = 0; i < vertices.Length; i++)
        {
            if(vertices[i].y > startY)
            {
                GameObject newCube = GameObject.Instantiate(pointCube);
                pointCube.transform.position = vertices[i] + guy.transform.position; 
                newCube.transform.parent = parent.transform;

            }
        }


    }


    // Update is called once per frame
    void Update () {
	
	}


    public void SwitchVisibleStatus()
    {
        int nbChild = parent.transform.childCount;

        for(int i = 0; i < nbChild; i++)
        {
            Transform point = parent.transform.GetChild(i);
            MeshRenderer mr = point.GetComponent<MeshRenderer>();
            bool status = mr.enabled;
            mr.enabled = !status;
        }

    }

    public bool returnVisibleStatus()
    {
        Transform point = parent.transform.GetChild(0);
        MeshRenderer mr = point.GetComponent<MeshRenderer>();

        return mr.enabled;       

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
