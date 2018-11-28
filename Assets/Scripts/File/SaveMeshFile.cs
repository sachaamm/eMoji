using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveMeshFile : MonoBehaviour {

    public GameObject inputField;
    bool fieldActive = false;
    public GameObject meshObject;
    public GameObject fbxGuy;
    SkinnedGUIOptions skinnedGUI;

	// Use this for initialization
	void Start () {
        //saveActive
        inputField.SetActive(false);
        skinnedGUI = GetComponent<SkinnedGUIOptions>();

	}
	
	// Update is called once per frame
	void Update () {

        
        if (fieldActive)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Mesh m = meshObject.GetComponent<MeshFilter>().mesh;
                SkinnedMeshRenderer smr = fbxGuy.GetComponent<SkinnedMeshRenderer>();
                m = smr.sharedMesh;

                InputField field = inputField.GetComponent<InputField>();
                startSaveMesh(m, field.text);
                inputField.SetActive(false);
                fieldActive = false;

            }

        }
        
        
	}

    void OnGUI()
    {
        float sw = 120;
        float sh = 50;


        if(skinnedGUI.returnModeValue() == 0)
        {

            if (GUI.Button(new Rect(Screen.width / 2 - sw / 2, Screen.height - sh, sw, sh), "SaveMeshState"))
            {
                fieldActive = !fieldActive;
                inputField.SetActive(fieldActive);
            }


        }

    }


    public void startSaveMesh(Mesh m, string fileName)
    {

        int nbLines = m.vertexCount + m.triangles.Length/3;
        string[] lines = new string[nbLines];

        int vertexIndex = 0;

        while(vertexIndex < m.vertexCount)
        {
            lines[vertexIndex] = "v_" + m.vertices[vertexIndex].x + "_" + m.vertices[vertexIndex].y + "_" + m.vertices[vertexIndex].z;
            vertexIndex++;
        }

        int faceIndex = 0;

        while (faceIndex < m.triangles.Length / 3) { 

        lines[faceIndex + m.vertexCount] = "f_" + m.triangles[faceIndex*3] + "_" + m.triangles[faceIndex*3 + 1] + "_" + m.triangles[faceIndex*3 + 2];
        faceIndex++;

        }

        saveMeshFile(lines, fileName);

    }

    public void saveMeshFile(string[] lines, string name)
    {
        print("saveFile");
        System.IO.File.WriteAllLines("Assets/MeshFiles/" + name + ".txt", lines);

    }
}
