using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class BakedAnimationController : MonoBehaviour {

    SkinnedGUIOptions skinnedGui;
    SkinnedFaceModeller skinnedModeller;
    int nbFrames = 200;
    int frame = 0;

    float sx, sy, sw, sh;
    List<AnimationField> animationFields;
    int selected = 0;

    int animMode = 2;
    public bool isPlaying = false;

    public GameObject yMouth, zMoouth;

    int intervalTime = 1;

    //List<int> animationVerticesIndexes;


    public GameObject groupCenterObject;

    // Use this for initialization
    void Start () {

        skinnedGui = GetComponent<SkinnedGUIOptions>();
        skinnedModeller = GetComponent<SkinnedFaceModeller>();

        sw = Screen.width / 1.25f;
        sh = Screen.height / 1.35f;
        sx = (Screen.width - sw)/2;
        sy = sh;

        animationFields = new List<AnimationField>();

        //animationVerticesIndexes = new List<int>();
    }

    // Update is called once per frame
    void Update () {

        
        // FAIRE DEFILER LE TEMPS
        if (Time.frameCount % intervalTime == 0 && isPlaying)
        {
            frame++;
            if (frame >= nbFrames) frame -= nbFrames;

        }

        if(animationFields.Count > 0 && isPlaying)
        {

            //AnimationField af = animationFields[0];
            //float currentYval = af.getCurrentModificationValue();           
            //yMouth.GetComponent<Slider>().value = currentYval;
       
        }


        //int a = 0;

        foreach (AnimationField af in animationFields)
        {/*
            GUI.color = Color.white;
            if (selected == a) GUI.color = Color.yellow;

            if (GUI.Button(new Rect(sx - afw, afy, afw, afh), af.name))
                selected = a;
            */


            if (isPlaying)
            {

                if (Time.frameCount % intervalTime == 0) af.makeTime(nbFrames);

                List<int[]> allVertexIndexes = getAnimationIndexesLinks();
                float[] values = getAnimationValues();
                List<int[]> allStrengthValues = getAnimationStrengths();

                skinnedModeller.modelateVerticeList(allVertexIndexes, values,allStrengthValues,getAnimationCenters());


                //foreach (FrameValue fv in frameValues)
                //{
                //  GUI
                //GUI.color = Color.red;
                //GUI.Button(new Rect(sx + fv.frame * sw / nbFrames, sy, 60, 20), "" + fv.value);

                //skinnedModeller.modelateVerticeList()

                // }
                //foreach (AnimationField af in animationFields)
                //{


                // }

                // af.debug();

            }

            //  a++;


        }


        }

    List<int[]> getAnimationIndexesLinks()
    {
        List<int[]> indexes = new List<int[]>();

        foreach (AnimationField af in animationFields)
        {
            indexes.Add(af.indexArray);


         }

        return indexes;

    }


    List<int[]> getAnimationStrengths()
    {
        List<int[]> strengths = new List<int[]>();

        foreach (AnimationField af in animationFields)
        {
            strengths.Add(af.valuesArray);


        }

        return strengths;

    }


    List<Vector3> getAnimationCenters()
    {
        List<Vector3> centers = new List<Vector3>();

        foreach (AnimationField af in animationFields)
        {
            centers.Add(af.groupCenter);

        }

        return centers;

    }
    float[] getAnimationValues()
    {
        float[] values = new float[animationFields.Count];

        int c = 0;
        foreach (AnimationField af in animationFields)
        {
            values[c]= af.getCurrentModificationValue();
            c++;

        }




        return values;

    }


    void OnGUI()
    {


        if(skinnedGui.returnModeValue() == 3)
        {

            float sY = Screen.height - Screen.height / 8;
            float mH = 25;

            GUI.color = Color.white;


            if (GUI.Button(new Rect(5, sY - 50, 150, mH), "Insert Keyframe"))
            {
                AnimationField afSelect = animationFields[selected];
                float yVal = yMouth.GetComponent<Slider>().value;
                afSelect.addKeyframe(frame, yVal);
            }

            if (animMode == 0) GUI.color = Color.green;

            if (GUI.Button(new Rect(5, sY, 80, mH), "Play"))
            {
                //  if (!isPlaying) currentFaceIndex = 0;
                if (animMode == 2)
                {
                    frame = 0;
                    animationFields[0].initTime();
                }

                skinnedModeller.calculateAllDistances(getAnimationIndexesLinks(), getAnimationCenters());

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

            if (GUI.Button(new Rect(5, sY + mH * 2, 80, mH), "Stop"))
            {
                isPlaying = false;
                frame = 0;
                animMode = 2;

            }

            GUI.color = Color.white;





        }



        if (skinnedGui.returnModeValue() == 3)
        {
          
            Rect r = new Rect(sx,sy, sw, sh);
            if (GUI.Button(r, ""))
            {
                setFrame();
            }

            GUI.Button(new Rect(50, 450, 100, 30),"Frame : "+frame);

            GUI.Button(new Rect(sx + frame * sw / nbFrames,sy,20,20), "");
            //  GUI.Button(new Rect(sx + frame , sy, 20, 20), "");

            float afw = 100;
            float afy = sy;
            float afh = 30;

            int a = 0;


        foreach(AnimationField af in animationFields)
            {
                GUI.color = Color.white;
                if (selected == a) GUI.color = Color.yellow;

                if (GUI.Button(new Rect(sx -afw , afy , afw , afh), af.name))
                    selected = a;



                if (isPlaying) af.debug();



                List<FrameValue> frameValues = af.modificationValues;
                foreach (FrameValue fv in frameValues)
                {
                    //  GUI
                    GUI.color = Color.red;
                    GUI.Button(new Rect(sx + fv.frame * sw / nbFrames, afy, 60, 20), "" + fv.value);

                }



                a++;
                afy += afh;


             
            }
         



        }



    }
    

    void setFrame()
    {
        float f = (Input.mousePosition.x - sx)/(float)sw * nbFrames;
        frame = (int)f;

    }




    public void addTrackingGroupToAnimation(string name,int[] indexArray,int[] valuesArray,Vector3 groupCenter)
    {
        animationFields.Add(new AnimationField(name,indexArray,valuesArray,groupCenter,groupCenterObject));

        print("name is " + name);

        selected = animationFields.Count - 1;
        frame = 0;



    }

    class AnimationField
    {
        public string name;
        public int[] indexArray, valuesArray;
        public List<FrameValue> modificationValues;
        int step = -1;
        //float currentValue = 0;
        int timeToNextStep = 0;
        float distance = 0;
        //int saveTime = 0;
        int time = 0;

        int interpolationTime = 0;

        public Vector3 groupCenter;

        public AnimationField(string n , int[] indexes, int[] values,Vector3 center,GameObject groupCenterObject)
        {
            name = n;
            indexArray = indexes;
            valuesArray = values;
            modificationValues = new List<FrameValue>();
            groupCenter = center;

            GameObject newCenterObject = GameObject.Instantiate(groupCenterObject);
            newCenterObject.transform.position = center;
        }



        public void initTime()
        {
            step = 0;
            FrameValue currentFrameValue = modificationValues[0];
            FrameValue nextFrameValue = modificationValues[1];

            timeToNextStep = nextFrameValue.frame - currentFrameValue.frame;
            distance = nextFrameValue.value - currentFrameValue.value;

        }
        public void makeTime(int nbFrames)
        {

            //time++;
            //if (time == nbFrames) time = 0;

           interpolationTime++;
           if (interpolationTime >= timeToNextStep) startAnimation(time,nbFrames);
           

            



        }

 

        public void startAnimation(int time, int nbFrames)
        {
            if (modificationValues.Count == 0) return;

            step++;
            step %= (modificationValues.Count);

            int next = step + 1;
            next %= modificationValues.Count;

            FrameValue currentFrameValue = modificationValues[step];
            FrameValue nextFrameValue = modificationValues[next];

            distance = nextFrameValue.value - currentFrameValue.value;
            timeToNextStep = nextFrameValue.frame - currentFrameValue.frame;
            if (timeToNextStep < 0) timeToNextStep += nbFrames;

           // print(" StartAnim with:  timeToNext "+timeToNextStep+" step is "+step+" nextStep is "+next);

            interpolationTime = 0;
           
        }

        public void addKeyframe(int f,float v)
        {
            modificationValues.Add(new FrameValue(f,v));

        }

        public bool checkIfCycleIsOver(int time,int nbFrames)
        {        
            int c = 0;

           foreach(FrameValue fv in modificationValues)
            {
                if(fv.frame == time)
                {
                    print("do it "+c);
                    startAnimation(time,nbFrames);
                    // c++;
                    return true;
                }

            }

            return false;
        }

        public void debug()
        {

            GUI.Button(new Rect(500, 20, 400, 30), "Step is " + step+" Time is "+time+" interpolationTime "+interpolationTime+" TimeToNext "+timeToNextStep);
            
        }

        public float getCurrentModificationValue()
        {
            if (step < 0) return 0;
            FrameValue currentFrameValue = modificationValues[step];
            float mapTime = interpolationTime / (float)timeToNextStep;

 
                return currentFrameValue.value + (distance * mapTime);
        }

   

    }

    class FrameValue
    {
        public int frame;
        public float value;


        public FrameValue(int f ,float v)
        {
            frame = f;
            value = v;

        }




    }

}
