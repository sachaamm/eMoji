using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FbxToMeshCollider : MonoBehaviour {
    public GameObject fbxGuy;
    MeshCollider mc;
    MeshFilter mf;
	// Use this for initialization
	void Start () {
        mc = GetComponent<MeshCollider>();
        mf = GetComponent<MeshFilter>();
        Mesh m = mf.mesh;
        SkinnedMeshRenderer smr = fbxGuy.GetComponent<SkinnedMeshRenderer>();

        Mesh sm = smr.sharedMesh;
        int[] triangles = sm.triangles;
        Vector3[] vertices = sm.vertices;
        List<Vector3> mvert = new List<Vector3>();

        //print("mesh vertices "+vertices.Length);


        for(int i = 0; i < vertices.Length; i++)
        {
            mvert.Add(vertices[i] * fbxGuy.transform.localScale.x / transform.localScale.x);

        }

        m.SetVertices(mvert);
        m.RecalculateBounds();
        m.RecalculateNormals();

        Mesh nm = new Mesh();
        nm.vertices = mvert.ToArray();
        nm.triangles = triangles;

        mc.sharedMesh = nm;



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
