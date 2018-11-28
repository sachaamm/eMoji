using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveFaceFile : MonoBehaviour {
    public GameObject fbxGuy;
    List<int> indexLinks;
    // Use this for initialization
    void Start () {

        indexLinks = new List<int>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void saveFaceFile(string fileName)
    {
        print(" save Face File ");

        List<string> lines = new List<string>();

        SkinnedFaceTracing faceTracing = GetComponent<SkinnedFaceTracing>();
        indexLinks = faceTracing.returnIndexLinks();
        SkinnedMeshRenderer smr = fbxGuy.GetComponent<SkinnedMeshRenderer>();
        Vector3[] vertices = smr.sharedMesh.vertices;

        for(int i = 0; i < vertices.Length; i++)
        {
            if (indexIsInFace(i))
            {
                Vector3 v = vertices[i];
                string line = i + "_" + v.x + "_" + v.y + "_" + v.z;
                lines.Add(line);
            }

        }

        System.IO.File.WriteAllLines("Assets/FaceFiles/" + fileName + ".txt", lines.ToArray());

    }

    bool indexIsInFace(int index)
    {
        for(int i = 0; i < indexLinks.Count; i++)
        {
            if (indexLinks[i] == index) return true;

        }

        return false;
    }
}
