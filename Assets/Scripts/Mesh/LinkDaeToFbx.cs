using UnityEngine;
using System.Collections;

public class LinkDaeToFbx : MonoBehaviour {

    public GameObject vertice, verticeB;
    public GameObject dae, fbx;
    //Mesh a, b;
	// Use this for initialization
	void Start () {

        // FIRST , WE COPY DAE MODEL MESH TO THE FBX MESH MODEL TO GET SAME VERTICES INDEXATION , ETC

        //a = dae.GetComponent<MeshFilter>().mesh;
        //b = fbx.GetComponent<SkinnedMeshRenderer>().sharedMesh;

        //MeshFilter mf = dae.GetComponent<MeshFilter>();
        //mf.mesh = b;

        //copyMesh(ref a,b);
        //dae.transform.localScale = fbx.transform.localScale;


        //MeshCollider mc = dae.GetComponent<MeshCollider>();
        //mc.sharedMesh = a;

       
        //float v = dae.transform.localScale.x;
        //float v2 = fbx.transform.localScale.x;
        //addGrid(a, v, 10, vertice);
        //addGrid(b, v2,10, verticeB);


    }

    void copyMesh(ref Mesh copy, Mesh model)
    {
        copy = model;
    }



    void addGrid(Mesh m , float v, int moduloNb,GameObject _v)
    {

        for(int i = 0; i < m.vertexCount; i+=moduloNb)
        {

            GameObject newV = GameObject.Instantiate(_v);
            newV.transform.position = m.vertices[i] * v;
        }


    }
	// Update is called once per frame
	void Update () {
	


	}
}
