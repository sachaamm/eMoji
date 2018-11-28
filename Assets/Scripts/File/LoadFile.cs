using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LoadFile : MonoBehaviour {

    List<string> filesNames;
    //string selectFileName = "";
    FaceTracing faceTracing;
    TrackerAnimator animator;
    //
    // Use this for initialization
    void Start () {
        filesNames = new List<string>();
        faceTracing = GetComponent<FaceTracing>();
        animator = GetComponent<TrackerAnimator>();
        listFiles();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void listFiles()
    {
        // Process the list of files found in the directory.
       string[] filePaths = Directory.GetFiles(@"Assets\ConfigFiles\");

      foreach(string fileName in filePaths)
        {
            string[] spl = fileName.Split('.');

            if (spl.Length == 2)
            {        
                string cleanName = cleanInput(fileName,7);
                filesNames.Add(cleanName);
            }

        }

    }


    string cleanInput(string input,int nbCh)
    {
        char[] ch = new char[input.Length - nbCh];

        for(int i = 0; i < input.Length-nbCh; i++)
        {
            ch[i] = input[i+nbCh];
        }

        return new string(ch);
    }
    void readFile(string fileName)
    {

        faceTracing.clearTracking();
        animator.clearTracker();

        //List config files
       string[] lines = System.IO.File.ReadAllLines(@"Assets\"+fileName);// dirty because Assets is inside Assets....<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        // Display the file contents by using a foreach loop.
        foreach (string line in lines)
        {
            string[] values = line.Split(' ');
            string input = values[0];// Index Value
            int i;
            bool result = int.TryParse(input,out i);//Check If Input is Parsable
            print(input);

            if (result)
            {
                print(" ADD INDEX "+System.Int32.Parse(values[0]));
                int index = int.Parse(values[0]);
                int value = int.Parse(values[1]);
                faceTracing.addTrackingPoint(index,value);

            }
         
        }

        // faceTracing.MoveWithIndexes();
        // faceTracing.LinkToMesh();
        animator.StartAnimation();

    }

    void unread()
    {
        //Clear list

    }

    void OnGUI()
    {

        int ww = 200;
        int sx = Screen.width - ww;
        int sy = 60;
        int sw = ww;
        int sh = 30;

        int i = 0;

        foreach (string fileName in filesNames)
        {

            if (GUI.Button(new Rect(sx, sy + i * sh, sw,sh), fileName))
            {
                readFile(fileName);
            }

            i++;

        }



        if(GUI.Button(new Rect(sx, 0,150,60),"Load File"))
        {

        }


    }
}
