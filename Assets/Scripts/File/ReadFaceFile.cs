using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class ReadFaceFile : MonoBehaviour {

    List<string> filesNames;
    List<Vector3> vertices;

    SkinnedMeshRenderer smr;
    public GameObject fbxGuy;
    // Use this for initialization
    void Start() {

        smr = fbxGuy.GetComponent<SkinnedMeshRenderer>();
        Vector3[] v = smr.sharedMesh.vertices;
        vertices = new List<Vector3>(v);

        filesNames = new List<string>();
        listFiles();

    }

    void listFiles()
    {

        // Process the list of files found in the directory.
        string[] filePaths = Directory.GetFiles(@"Assets\FaceFiles\");

        foreach (string fileName in filePaths)
        {
            string[] spl = fileName.Split('.');

            if (spl.Length == 2)
            {
                string cleanName = cleanInput(fileName, 7);
                filesNames.Add(cleanName);
            }

        }

    }


    string cleanInput(string input, int nbCh)
    {
        char[] ch = new char[input.Length - nbCh];

        for (int i = 0; i < input.Length - nbCh; i++)
        {
            ch[i] = input[i + nbCh];
        }

        return new string(ch);
    }

    // Update is called once per frame
    void Update() {

    }

    void readFile(string fileName)
    {
        print(" <<< Load Face File >>> ");
        //List config files
        string[] lines = System.IO.File.ReadAllLines(@"Assets\" + fileName);// dirty because Assets is inside Assets....<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        
        // Display the file contents by using a foreach loop.
        foreach (string line in lines)
        {
            string[] values = line.Split('_');
 
            int index = int.Parse(values[0]);
            float x = float.Parse(values[1]);
            float y = float.Parse(values[2]);
            float z = float.Parse(values[3]);

            vertices[index] = new Vector3(x, y, z);

        }

        smr.sharedMesh.SetVertices(vertices);
        smr.sharedMesh.RecalculateBounds();
        smr.sharedMesh.RecalculateNormals();

        // WHEN WE READ A FACE FILE , WE SYNC THE MESH VERTICES TO THE FACE MODELLER

        SkinnedFaceModeller faceModeller = GetComponent<SkinnedFaceModeller>();
        faceModeller.syncVertices(vertices);

    }


    void OnGUI()
    {


        int ww = 200;
        int sx = Screen.width - ww;
        int sy = 60;
        int sw = ww;
        int sh = 30;

        int i = 0;

        SkinnedGUIOptions skinnedGUI = GetComponent<SkinnedGUIOptions>();

        //WE LOAD MESH ONLY IN OBJECT MODE
        if (skinnedGUI.returnModeValue() == 0)
        {

            foreach (string fileName in filesNames)
            {

                if (GUI.Button(new Rect(sx, sy + i * sh, sw, sh), fileName))
                {
                    readFile(fileName);
                    SaveAnimFile saveAnim = GetComponent<SaveAnimFile>();
                    saveAnim.SetFaceName("Assets\\"+fileName);

                }
                
                i++;

            }

            if (GUI.Button(new Rect(sx, 0, 150, 60), "Load Face"))
            {

            }



        }



    }

    public void loadFace(string fileName)
    {

        readFile(fileName);
        SaveAnimFile saveAnim = GetComponent<SaveAnimFile>();
        saveAnim.SetFaceName("Assets\\" + fileName);


    }




}
