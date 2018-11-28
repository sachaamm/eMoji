using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;



public class SkinnedLoadFile : MonoBehaviour {


    List<string> filesNames;
    //string selectFileName = "";
    SkinnedFaceTracing faceTracing;
    SkinnedFaceModeller faceModeller;
    //
    // Use this for initialization

    bool loaded = false;

    Texture trackingExemple;
    //public Texture[] trackingScreenshots;
    string preparedFileName;
    int selected = -1;

    //List<GameObject> previewTrackingPoints;

    List<TrackingPoint> trackingPoints;


    void Start()
    {

        filesNames = new List<string>();
       // previewTrackingPoints = new List<GameObject>();
        faceTracing = GetComponent<SkinnedFaceTracing>();
        faceModeller = GetComponent<SkinnedFaceModeller>();
        listFiles();


    }

    // Update is called once per frame
    void Update()
    {

    }

    void listFiles()
    {
        // Process the list of files found in the directory.
        string[] filePaths = Directory.GetFiles(@"Assets\ConfigFiles\");

        filesNames = new List<string>();

        foreach (string fileName in filePaths)
        {
            string[] spl = fileName.Split('.');

            if (spl.Length == 2)
            {
                string cleanName = cleanInput(fileName, 7);
                filesNames.Add(cleanName);
            }

        }//

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

    // WE Load A VERTEX GROUP 

    void readFile(string fileName)
    {
        faceTracing.resetPreview();

        faceTracing.clearTracking();
        faceModeller.clearTracker();
        print("LOAD TRACKING GROUP");
        //List config files

        /*
        string[] lines = System.IO.File.ReadAllLines(@"Assets\" + fileName);// dirty because Assets is inside Assets....<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


        SkinnedGUIOptions skinnedGUI = GetComponent<SkinnedGUIOptions>();

        //WE LOAD GROUP ONLY IN EDIT MODE
        if (skinnedGUI.returnModeValue() == 1)
        {

            // Display the file contents by using a foreach loop.
            foreach (string line in lines)
            {
                string[] values = line.Split(' ');
                string input = values[0];// Index Value
                int i;
                bool result = int.TryParse(input, out i);//Check If Input is Parsable

                if (result)
                {
                    //print(" ADD INDEX "+System.Int32.Parse(values[0]));
                    int index = int.Parse(values[0]);
                    int value = int.Parse(values[1]);
                    faceTracing.addTrackingPoint(index, value);
                    faceTracing.addPreviewPoint(index);

                }

            }

        }

        // faceTracing.MoveWithIndexes();
        // faceTracing.LinkToMesh();

    */

        List<int> loadedIndexes = new List<int>();
        List<int> loadedValues = new List<int>();

        BakedAnimationController animationController = GetComponent<BakedAnimationController>();

        


    // WE LOAD TRACKING POINTS , THEY ARE READY 
        foreach(TrackingPoint tp in trackingPoints)
        {

            faceTracing.addTrackingPoint(tp.index, tp.value);
            loadedIndexes.Add(tp.index);
            loadedValues.Add(tp.value);


        }


        animationController.addTrackingGroupToAnimation(fileName, loadedIndexes.ToArray(), loadedValues.ToArray(), faceModeller.getRealTrackingCenter(loadedIndexes));

        faceModeller.StartAnimation();

    }

    void unread()
    {
        //Clear list

    }


    void prepare(string fileName)
    {
        loaded = true;
        //  trackingExemple = trackingScreenshots[index];
        preparedFileName = fileName;
        faceTracing.resetPreview();

        trackingPoints = new List<TrackingPoint>();


        string[] lines = System.IO.File.ReadAllLines(@"Assets\" + fileName);// dirty because Assets is inside Assets....<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


        SkinnedGUIOptions skinnedGUI = GetComponent<SkinnedGUIOptions>();

        //WE LOAD GROUP ONLY IN EDIT MODE OR IN BAKE MODE
        if (skinnedGUI.returnModeValue() == 1 || skinnedGUI.returnModeValue() == 3)
        {

            // Display the file contents by using a foreach loop.
            foreach (string line in lines)
            {
                string[] values = line.Split(' ');
                string input = values[0];// Index Value
                int i;
                bool result = int.TryParse(input, out i);//Check If Input is Parsable

                if (result)
                {
        
                    int index = int.Parse(values[0]);
                    int value = int.Parse(values[1]);

                    faceTracing.addPreviewPoint(index);

                    trackingPoints.Add(new TrackingPoint(index, value));

                }



            }



        }


    }

       void OnGUI(){

        int ww = 200;
        int sx = Screen.width - ww;
        int sy = 60;
        int sw = ww;
        int sh = 30;

        int i = 0;

        SkinnedGUIOptions skinnedGui = GetComponent<SkinnedGUIOptions>();
        int guiMode = skinnedGui.returnModeValue();

        // WE LOAD TRACKING IN EDIT MODE
        if(guiMode % 2 == 1)
        {

            float lX = Screen.width / 1.39f;
            float d = 0.7f;

            if (loaded)
            {

                //GUI.DrawTexture(new Rect(lX, 0, 400 * d, 600*d), trackingExemple);

                if(GUI.Button(new Rect(lX, 600 * d, 100, 30), "LoadTracking"))
                {
                    readFile(preparedFileName);
                    loaded = false;
                  
                }


                if (GUI.Button(new Rect(lX + 120, 600 * d, 100, 30), "Cancel"))
                {
                    //readFile(preparedFileName);
                    loaded = false;
                    selected = -1;
                }

            }

      
            foreach (string fileName in filesNames)
            {
                GUI.color = Color.white;
                if (i == selected) GUI.color = Color.green;


                if (GUI.Button(new Rect(sx, sy + i * sh, sw, sh), fileName))
                {
                    // readFile(fileName);
                    prepare(fileName);
                    selected = i;
                }

                i++;

            }



            GUI.color = Color.white;


            if (GUI.Button(new Rect(sx, 0, 150, 60), "Load Tracking Group"))
            {
                listFiles();
            }


        }
      


    }


    class TrackingPoint {

        public int index, value;

        public TrackingPoint(int ind , int val)
        {

            index = ind; value = val;


        }










    }










}

