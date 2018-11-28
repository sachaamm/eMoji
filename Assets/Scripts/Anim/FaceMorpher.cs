using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FaceMorpher : MonoBehaviour {


    public int nbFrames = 200;

  
    SkinnedGUIOptions skinnedGUI;

    bool isPlaying = false;
   
    
    int animMode = 2; // ON DEMARRE SUR STOP

    List<FaceState> faces;

    SkinnedFaceTracing faceTracing;
    public GameObject fbxGuy;

    //MOUSE POS
    //float mx = 0;
    //float my = 0;


    int selected = -1;

    int currentFaceIndex = -1;
    int nextFaceIndex = 0;

    Vector3[] distances;

    int intervalTime = 1;

    int time = 0;
    public int timeToNextStep = 0;
    public int interpolationTime = 0;
    public float mapTime = 0;


    // Use this for initialization
    void Start () {

        skinnedGUI = GetComponent<SkinnedGUIOptions>();
    }

    public void initMorphing()
    {
        faces = new List<FaceState>();
        skinnedGUI = GetComponent<SkinnedGUIOptions>();
        faceTracing = GetComponent<SkinnedFaceTracing>();
        Vector3[] currentFace = faceTracing.returnCurrentFace();
        //faces.Add(new FaceState(0, currentFace));
        distances = new Vector3[currentFace.Length];

    }

    void startMorphing()
    {
        
        interpolationTime = 0;


        //RECALCUL DES INDEXS
        currentFaceIndex++;
        currentFaceIndex %= faces.Count;

        int nextIndex = currentFaceIndex + 1;
        nextFaceIndex = nextIndex % faces.Count;

        print("currentFaceIndex is " + currentFaceIndex);
        print("nextFaceIndex is " + nextFaceIndex);


        FaceState current = faces[currentFaceIndex];
        FaceState next = faces[nextFaceIndex];

        print(" Current Face name is " + current.faceFileName + " the frame number is " + current.frame);
        print(" Next Face name is " + next.faceFileName + " the frame number is " + next.frame);


        // Ranger les face states dans l'ordre
        // Calculer le temps au prochain cycle

        //CALCUL DU TEMPS DE L INTERPOLATION
        timeToNextStep = next.frame - current.frame;

        print("Time to Next Step is : " + timeToNextStep);
        if (timeToNextStep < 0)
        {
            timeToNextStep += nbFrames;
       }

       

        calcInterpolation();

  


    }

    void calcInterpolation()
    {

        FaceState current = faces[currentFaceIndex];
        FaceState next = faces[nextFaceIndex];

        if (distances == null) distances = new Vector3[current.faceVertices.Length];

        for (int i = 0; i < distances.Length; i++)
        {
            distances[i] =  next.faceVertices[i] - current.faceVertices[i];
        }
    }

    Vector3[] getCurrentInterpolation()
    {
        FaceState current;
       

        if (currentFaceIndex != -1)
        {
            current = faces[currentFaceIndex];

        }
        else
        {
            current = faces[0] ;
        }
      
        //FaceState next = faces[nextFaceIndex];

        Vector3[] currentFace = current.faceVertices;
        Vector3[] interpolatedFace = new Vector3[currentFace.Length];

        mapTime = interpolationTime / (float)timeToNextStep;

       for(int i = 0; i < currentFace.Length; i++)
        {
            interpolatedFace[i] = currentFace[i] + (distances[i] * mapTime);
        }

        return interpolatedFace;

    }
	
	// Update is called once per frame
	void Update () {


        if (skinnedGUI.returnModeValue() != 2) return;

        if (!isPlaying) return;


        faceTracing.setFaceVertices(getCurrentInterpolation());


       // mx = Input.mousePosition.x;
       // my = Screen.height - Input.mousePosition.y;

        if(Time.frameCount % intervalTime == 0)
        {
            interpolationTime++;
            if (interpolationTime >= timeToNextStep) startMorphing();

            time++;
            if (time >= nbFrames) time -= nbFrames;

        }



    }

    void OnGUI()
    {
  
        if (skinnedGUI.returnModeValue() == 2)
        {
            //if(faces.Count!=0)GUI.Button(new Rect(150,150,420,35),"Current Step :" + currentFaceIndex + "interpolation Time "+interpolationTime+" timeToNextStep"+timeToNextStep );
            float sX = Screen.width / 20;
            float sW = Screen.width - sX*2;
            float sY = Screen.height - Screen.height / 8;
            float sH = Screen.height / 10;

            float mH = 25;

            // PLAY PAUSE STOP

            GUI.color = Color.white;
            if (animMode == 0) GUI.color = Color.green;

            if (GUI.Button(new Rect(5, sY, 80, mH), "Play"))
            {
                //if (!isPlaying) currentFaceIndex = 0;
                isPlaying = true;
                animMode = 0;
            }

            GUI.color = Color.white;
            if (animMode == 1) GUI.color = Color.green;

            if (GUI.Button(new Rect(5, sY + mH, 80, mH), "Pause"))
            {
                isPlaying = false;
                animMode = 1;
            }

            GUI.color = Color.white;
            if (animMode == 2) GUI.color = Color.green;

            if (GUI.Button(new Rect(5, sY + mH*2, 80, mH), "Stop"))
            {
                isPlaying = false;
                time = 0;
                animMode = 2;

                currentFaceIndex = -1;
                nextFaceIndex = 0;

            }

            GUI.color = Color.white;

            // INSERT KEYFRAME

            if(GUI.Button(new Rect(20, sY - 100, 150, 40),"Insert KeyFrame"))
            {
                if (selected < 0) selected = 0;
                SaveAnimFile saveAnimFile = GetComponent<SaveAnimFile>();
                string faceName = saveAnimFile.returnFaceName();

                Vector3[] currentFace = faceTracing.returnCurrentFace();
                faces.Add(new FaceState(selected, currentFace,faceName));
                selected = -1;
               
            }

            if (GUI.Button(new Rect(20, sY - 180, 150, 40), "Reset Anim"))
            {
                resetFaceStates();

            }

            //GUI.Button(new Rect(sX, sY, sW, sH), "");

            float bW = (sW / nbFrames);

            // KEYFRAMES

            foreach (FaceState fs in faces)
            {

                float fX = sX + fs.frame * bW;
                float fH = 30;
                if(GUI.Button(new Rect(fX, sY - fH, bW*20, fH), fs.faceFileName)){

                }

            }

           
            for (int i = 0; i < nbFrames; i++)
            {
                GUI.color = getRelativeColor();


                if (i == selected) GUI.color = Color.green;


                float bX = sX + bW * i;
                if(GUI.Button(new Rect(bX, sY, bW, sH), ""))
                {
                    //print("ok");
                    selected = i;
                }
            }

            float selX = sX + time * bW;
            if (animMode == 0) GUI.color = Color.green;
            if (animMode == 1) GUI.color = Color.yellow;
            if (animMode == 2) GUI.color = Color.red;

            GUI.Button(new Rect(selX, sY + sH, bW, sH), "");
            
        }
     
    }

    Color getRelativeColor()
    {

        // ON REGARDE ENTRE QUELLE PLAGE ET QUELLE PLAGE SE SITUE LA COULEUR

        return Color.grey;
    }

    bool insideRect()
    {
        return false;
    }

    public List<string> returnFaceNames()
    {

        List<string> names = new List<string>();

        foreach (FaceState fs in faces)
        {
            names.Add(fs.faceFileName);
      

        }

        return names;

    }

    public List<int> returnFrames()
    {

        List<int> frames = new List<int>();

        foreach (FaceState fs in faces)
        {
            frames.Add(fs.frame);



        }

        return frames;

    }

    public void addFaceState(int frame,Vector3[] faceV, string faceName)
    {

        faces.Add(new FaceState(frame, faceV, faceName));

    }

    public void resetFaceStates()
    {

        faces = new List<FaceState>();
        currentFaceIndex = -1;
        nextFaceIndex = 0;


    }


    class FaceState
    {

        public int frame;
        public Vector3[] faceVertices;
        public string faceFileName;

        public FaceState(int _frame, Vector3[] faceV,string _faceName)
        {
            frame = _frame;
            faceVertices = faceV;
            faceFileName = _faceName;
           // print("faceFileName is" + faceFileName);
        }




    }
}
