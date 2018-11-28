using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class LoadMeshFile : MonoBehaviour {

    List<string> filesNames;
    public GameObject fbxGuy;
    SkinnedMeshRenderer smr;

    public GameObject yMouth, zMouth;
    float defaultYMouthValue, defaultZMouthValue;

    // Use this for initialization
    void Start () {

        filesNames = new List<string>();
        listFiles();
        smr = fbxGuy.GetComponent<SkinnedMeshRenderer>();

        Slider ys = yMouth.GetComponent<Slider>();
        Slider zs = zMouth.GetComponent<Slider>();

        defaultYMouthValue = ys.value;
        defaultZMouthValue = zs.value;


    }

    // Update is called once per frame
    void Update () {
	
	}

    void listFiles()
    {
        // Process the list of files found in the directory.
        string[] filePaths = Directory.GetFiles(@"Assets\MeshFiles\");

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

    public void loadFile(string fileName)
    {




    }

    //READ MESH file
    void readFile(string fileName)
    {
        //print("read mesh file");
        //List config files
        string[] lines = System.IO.File.ReadAllLines(@"Assets\" + fileName);// dirty because Assets is inside Assets....<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        print(lines.Length);

        print(fileName);


        //int nbLine = 0;
        //int nbResult = 0;

        //Mesh loadedMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        //List<int> mtriangles = new List<int>();




        // Display the file contents by using a foreach loop.
        foreach (string line in lines)
        {
            string[] values = line.Split('_');
            //string input = values[1];// Index Value


            string elementType = values[0];


            if (elementType[0] == 'v')
            {
                float x = float.Parse(values[1]);
                float y = float.Parse(values[2]);
                float z = float.Parse(values[3]);

                vertices.Add(new Vector3(x, y, z));


            }
    



        }


        print("vertices Count " + vertices.Count);

        Mesh mh = smr.sharedMesh;
        mh.SetVertices(vertices);
        mh.RecalculateBounds();
        mh.RecalculateNormals();
        smr.sharedMesh = mh;

        Slider ys = yMouth.GetComponent<Slider>();
        Slider zs = zMouth.GetComponent<Slider>();
        ys.value = defaultYMouthValue;
        zs.value = defaultZMouthValue;

       // print("reload default mesh");

        // WHEN WE READ A MESH FILE , WE SYNC THE MESH VERTICES TO THE FACE MODELLER

        SkinnedFaceModeller faceModeller = GetComponent<SkinnedFaceModeller>();
        //skinnedAnimator.StartAnimation();
        //skinnedAnimator.setVerticesAfterLoadingFile();
        faceModeller.syncVertices(vertices);

    }

    void OnGUI()
    {

        int ww = 200;
        int sx = Screen.width - ww*2;
        int sy = 60;
        int sw = ww;
        int sh = 30;

        int i = 0;

        SkinnedGUIOptions skinnedGUI = GetComponent<SkinnedGUIOptions>();

        //WE LOAD MESH ONLY IN OBJECT MODE
        if(skinnedGUI.returnModeValue() == 0)
        {

            foreach (string fileName in filesNames)
            {

                if (GUI.Button(new Rect(sx, sy + i * sh, sw, sh), fileName))
                {
                    readFile(fileName);
                }

                i++;

            }

            if (GUI.Button(new Rect(sx, 0, 150, 60), "Load Mesh"))
            {

            }



        }



    }




}
