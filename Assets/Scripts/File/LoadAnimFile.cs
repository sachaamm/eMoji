using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class LoadAnimFile : MonoBehaviour {

    List<string> filesNames;
    int selected = -1;

	// Use this for initialization
	void Start () {

        filesNames = new List<string>();
        //selected = 0;
        listFiles();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {

        int ww = 200;
        int sx = Screen.width - ww;
        int sy = 60;
        int sw = ww;
        int sh = 30;

        int i = 0;

        SkinnedGUIOptions skinnedGui = GetComponent<SkinnedGUIOptions>();
        int guiMode = skinnedGui.returnModeValue();

        // WE LOAD TRACKING IN EDIT MODE
        if (guiMode == 2)
        {


            foreach (string fileName in filesNames)
            {
                GUI.color = Color.white;
                if (i == selected) GUI.color = Color.green;


                if (GUI.Button(new Rect(sx, sy + i * sh, sw, sh), fileName))
                {
                    readFile(fileName);
                    //prepare(i, fileName);
                    selected = i;
                }

                i++;

            }


            if (GUI.Button(new Rect(sx, 0, 150, 60), "Load Anim"))
            {

            }


        }

    }

    void readFile(string fileName)
    {
        //List config files
        string[] lines = System.IO.File.ReadAllLines(@"Assets\" + fileName);// dirty because Assets is inside Assets....<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        ReadFaceFile readFace = GetComponent<ReadFaceFile>();
        SkinnedFaceTracing skinnedFace = GetComponent<SkinnedFaceTracing>();
        FaceMorpher faceMorpher = GetComponent<FaceMorpher>();

        print(" <<<Load Anim File>>> ");

        faceMorpher.resetFaceStates();

        for(int i = 1; i < lines.Length; i++)
        {

            string[] spl = lines[i].Split('_');
            string faceName = spl[0];
            int frame = int.Parse(spl[1]);

            string cleanName = cleanInput(faceName, 7);

            readFace.loadFace(cleanName);
            Vector3[] faceV = skinnedFace.returnCurrentFace();
            faceMorpher.addFaceState(frame, faceV, faceName);
            //print("WE GET FACE V LENGTH " + faceV.Length);

        }




    }




    void listFiles()
    {
        // Process the list of files found in the directory.
        string[] filePaths = Directory.GetFiles(@"Assets\AnimFiles\");

        foreach (string fileName in filePaths)
        {
            string[] spl = fileName.Split('.');

            //print("fileName in file Path " + fileName);

            if (spl.Length == 2)
            {
                string cleanName = cleanInput(fileName, 7);
                filesNames.Add(cleanName);
            }

        }

       // print("file Paths Length "+filePaths.Length);


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
}
