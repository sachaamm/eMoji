using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkinnedGUIOptions : MonoBehaviour {


    SkinnedMeshRenderer skin;
    HumanGrid2 gridScript2;
    SkinnedFaceTracing faceTracing;
    MeshRenderer mr;


    public Material basicSkin, translucentSkin;
    public GameObject fbxGuy;
    
    int skinId = 0;

    
    bool saveGroup = false;
    bool saveFace = false;

    bool fieldActive = false;
    bool saveFaceActive = false;


    //bool editable = true;
    //SLIDERS  
    public GameObject yMouth, zMouth, sawStrength;
    public GameObject inputField, faceNameField;

    public GameObject hairValueSlidersParent;


    int power = 100;
    public Color low, high;

    public GameObject colorTest;
    
    int modificatorModeValue = 0;

    public Material transparent,opaque;
    int opacityArg = 0;

    List<string> modificatorModes;
    List<string> modes;
    int modeValue = 1;

    public RenderTexture renderCam;

    float my = 0;

    //float rdiv = 2;
    float rx, ry, rw, rh;

    bool fullScreen = false;

    public GameObject hairObject;
    HairGenerator hairGenerator;

    // Use this for initialization
    void Start () {


        float rdiv = 2;
       rw = Screen.width / rdiv;
       rh = Screen.height / rdiv;
       rx = (Screen.width - rw) / 2;
       ry = (Screen.height - rh) / 2;

        if (hairObject)
        {
            hairGenerator = hairObject.GetComponent<HairGenerator>();
        }


        faceTracing = GetComponent<SkinnedFaceTracing>();

        modes = new List<string>();
        modes.Add("Object");
        modes.Add("Edit");
        modes.Add("Interpolate");
        modes.Add("Bake");
        modes.Add("Skin");
        modes.Add("Hair");


        skin = fbxGuy.GetComponent<SkinnedMeshRenderer>();
        gridScript2 = GetComponent<HumanGrid2>();

        modificatorModes = new List<string>();

        inputField.SetActive(false);
        faceNameField.SetActive(false);

        mr = colorTest.GetComponent<MeshRenderer>();

        modificatorModes.Add("Y/Z translate");
        modificatorModes.Add("Y Saw");
        modificatorModes.Add("Y Curve");
        modificatorModes.Add("Expand");

        //enableDisableSliders(editable);
        setHairSliderStatus(false);

    }


    void setHairSliderStatus(bool b)
    {

        GameObject[] hairValueSliders = new GameObject[hairValueSlidersParent.transform.childCount];    
        
        for(int i  = 0; i <  hairValueSlidersParent.transform.childCount; i++)
        {
            hairValueSliders[i] = hairValueSlidersParent.transform.GetChild(i).gameObject;
        }

        for(int i = 0; i < hairValueSliders.Length; i++)
        {
            hairValueSliders[i].SetActive(b);
        }


    }

    // Update is called once per frame
    void Update () {

        mr.material.color = mixColor();
        my = Screen.height - Input.mousePosition.y;

        if (Input.GetKeyDown("k"))
        {
            switchScreenMode();


        }


        // HAIR MODE
        if(modeValue == 5)
        {
            
            

            Slider[] hairValueSliders = new Slider[hairValueSlidersParent.transform.childCount];
            float[] parameters = new float[33];

            for (int i = 0; i < hairValueSlidersParent.transform.childCount; i++)
            {
                hairValueSliders[i] = hairValueSlidersParent.transform.GetChild(i).GetComponent<Slider>();
                parameters[i] = hairValueSliders[i].value;

            }

            if(hairGenerator)hairGenerator.setHairParameters(parameters);

            

        }
    }

    void switchScreenMode()
    {
        fullScreen = !fullScreen;
        Camera cam = GetComponent<Camera>();
        if (fullScreen)
        {

            cam.targetTexture = null;

        }
        else
        {


            cam.targetTexture = renderCam;
        }


    }

    void enableDisableModellerSliders(bool state)
    {
        yMouth.SetActive(state);
        zMouth.SetActive(state);
        sawStrength.SetActive(state);

    }

    void selectionMode()
    {

        BuildSkinColor skinColor = GetComponent<BuildSkinColor>();

        if (modeValue == 0)
        {
            enableDisableModellerSliders(false);
            skinColor.setSliders(false);
            setHairSliderStatus(false);
        }

        if (modeValue == 1)
        {
            enableDisableModellerSliders(true);
            skinColor.setSliders(false);
            setHairSliderStatus(false);
        }

        if (modeValue == 2)
        {
            enableDisableModellerSliders(false);
            skinColor.setSliders(false);
            setHairSliderStatus(false);

        }

        if (modeValue == 3)
        {
            enableDisableModellerSliders(true);
            skinColor.setSliders(false);
            setHairSliderStatus(false);
        }



        if (modeValue == 4)
        {

            enableDisableModellerSliders(false);
            skinColor.setSliders(true);
            setHairSliderStatus(false);
        }

        if (modeValue == 5)
        {

            enableDisableModellerSliders(false);
            skinColor.setSliders(false);
            setHairSliderStatus(true);

        }

    }



    void OnGUI()
    {


        //float sX = Screen.width / 20;
        //float sW = Screen.width;
        //float sY = Screen.height - Screen.height / 8;
        //float sH = Screen.height / 8;

        //GUI.Button(new Rect(0, sY, sW, sH), "");


       
        Rect renderRect = new Rect(rx,ry,rw,rh);
        if(!fullScreen)GUI.DrawTexture(renderRect, renderCam);


        float modeW = 100;
        float modeY = 140;

        for(int i = 0; i < modes.Count; i++)
        {
            GUI.color = Color.white;
            if (i == modeValue) GUI.color = Color.yellow;
            if (GUI.Button(new Rect(0 + modeW*i, modeY, modeW, 30), modes[i]))
            {
                modeValue = i;
                selectionMode();
            }
            
        }

        GUI.color = Color.white;

        switch (modeValue)
        {

            case 0://SELECTION MODE

                //CHANGE HUMAN SKIN OPACITY
                if (GUI.Button(new Rect(20, 20, 100, 30), "Change Skin"))
                {
                    skinId++;
                    if (skinId % 2 == 1)
                    {
                        skin.material = translucentSkin;
                    }
                    else
                    {
                        skin.material = basicSkin;
                    }

                }

                if (GUI.Button(new Rect(20, 60, 100, 30), "Switch Opacity"))
                {
                    opacityArg++;
                    SkinnedFaceTracing faceTracing = GetComponent<SkinnedFaceTracing>();
                    Material opacityMaterial;
                    opacityMaterial = opaque;
                    if (opacityArg % 2 == 1) opacityMaterial = transparent;
                    faceTracing.switchOpacity(opacityMaterial, opacityArg);

                }



                if (GUI.Button(new Rect(20, 150, 100, 30), "Save Group"))
                {
                    saveGroup = true;
                    fieldActive = !fieldActive;
                    inputField.SetActive(fieldActive);
                    faceNameField.SetActive(false);
                }

                GUI.Button(new Rect(20, 250, 150, 30), "Strength:" + power);

                if (saveGroup)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        fieldActive = false;
                        InputField field = inputField.GetComponent<InputField>();
                        string fieldText = field.text;
                        if (fieldText.Length > 0) saveGroupFile(faceTracing.returnVertexIndexes(), faceTracing.returnTrackingStrengths(), fieldText);
                        saveGroup = false;
                        inputField.SetActive(false);
                    }
                }


                if (GUI.Button(new Rect(20, 280, 100, 30), "Save Face"))
                {
                    saveFace = true;
                    saveFaceActive = !saveFaceActive;
                    faceNameField.SetActive(saveFaceActive);
                    inputField.SetActive(false);
                }

                if (saveFace)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        print("try to");
                        saveFaceActive = false;
                        InputField field = faceNameField.GetComponent<InputField>();
                        string fieldText = field.text;

                        if (fieldText.Length > 0) saveFaceFile(fieldText);

                        saveFace = false;
                        faceNameField.SetActive(false);
                    }
                }


                break;

            
            case 1://EDIT MODE

                if (GUI.Button(new Rect(20, 100, 100, 30), "Show Grid"))
                {
                    gridScript2.SwitchVisibleStatus();
                    gridScript2.SwitchVisibleTracking();
                }

                float hM = 30;
                float wM = 120;
                float yM = Screen.height - modificatorModes.Count * hM;
                for (int i = 0; i < modificatorModes.Count; i++)
                {
                    GUI.color = Color.white;
                    if (i == modificatorModeValue) GUI.color = Color.green;

                    if (GUI.Button(new Rect(0, yM, wM, hM), modificatorModes[i]))
                    {
                        modificatorModeValue = i;
                    }

                    yM += hM;
                }


                break;



            

        }
   
        if (Input.GetKey(KeyCode.X))
        {
            if(power > 0 ) power--;
        }

        if (Input.GetKey(KeyCode.C))
        {
            if (power < 100 ) power++;
        }


    }

    public int returnModeValue()
    {
        return modeValue;
    }

   
        public bool isOverRenderCam()
    {

        float mx = Input.mousePosition.x;
        if (mx > rx && mx < rx + rw && my > ry && my < ry + rh) return true;


        return false;
    }
    public int returnModificatorModeValue()
    {
        return modificatorModeValue;
    }

    public Color getColorWithThisValue(int _value)
    {
        return mixColor(_value);
    }

    void saveFaceFile(string faceName)
    {
        SaveFaceFile saveFace = GetComponent<SaveFaceFile>();
        saveFace.saveFaceFile(faceName);


    }

    void saveGroupFile(List<int> indexes , List<int> strenghts, string name)
    {
        string[] lines = new string[indexes.Count + 1];
        lines[0] = name;

        for(int i = 0; i < indexes.Count; i++)
        {
            lines[i + 1] = ""+indexes[i]+" "+strenghts[i];
        }

        SaveFile saveFile = GetComponent<SaveFile>();
        saveFile.saveGroupFile(lines, name);

    }

    public float returnStrength()
    {
        return power;
    }


    Color mixColor()
    {
        Color mix;
        float mapPower = power / (float)100;

        mix = new Color( (low.r * (1 - mapPower) + high.r * mapPower)/2, (low.g * (1 - mapPower) + high.g * mapPower) / 2, (low.b * (1 - mapPower) + high.b * mapPower) / 2);
        return mix;
        
    }


    public Color mixColor(float value)
    {
        Color mix;
        float mapPower = value / (float)100;

        mix = new Color((low.r * (1 - mapPower) + high.r * mapPower) / 2, (low.g * (1 - mapPower) + high.g * mapPower) / 2, (low.b * (1 - mapPower) + high.b * mapPower) / 2);
        return mix;

    }

    public Color returnPowerColor()
    {
        return mixColor();
    }

    public int returnPower()
    {
        return power;
    }

    public Material returnOpacityMaterial()
    {
        if (opacityArg % 2 == 1) return transparent;

        return opaque;
    }
    
}
