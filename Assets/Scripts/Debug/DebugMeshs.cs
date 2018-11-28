using UnityEngine;
using System.Collections;
//CHECK IF MESHS ARE SAME ORDERED 
public class DebugMeshs : MonoBehaviour {

    public GameObject guy, fbxGuy;
    Mesh a;
    Mesh b;
    public GameObject vertice;
	// Use this for initialization
	void Start () {

        MeshFilter mf = guy.GetComponent<MeshFilter>();
        a = mf.mesh;
        SkinnedMeshRenderer smr = fbxGuy.GetComponent<SkinnedMeshRenderer>();
        b = smr.sharedMesh;

        // check();

    }




    void check()
    {


        print(a.vertexCount);
        print(b.vertexCount);

        int ar = (int)Random.Range(0, a.vertexCount);
        GameObject newV = GameObject.Instantiate(vertice);
        newV.transform.position = a.vertices[ar] * guy.transform.localScale.x;
        newV.name = "DAE";
        GameObject newVV = GameObject.Instantiate(vertice);
        newVV.transform.position = b.vertices[ar] * fbxGuy.transform.localScale.x;
        newVV.name = "FBX";


    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) check();
	}
}
