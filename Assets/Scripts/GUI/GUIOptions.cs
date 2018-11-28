using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GUIOptions : MonoBehaviour {

    public Material basicSkin, translucentSkin;
    public GameObject guy;
    MeshRenderer skin;
    int skinId = 0;

    HumanGrid gridScript;
    HumanGrid2 gridScript2;

    bool saveGroup = false;
    bool editable = false;
    //string groupName = "Group";


    public GameObject inputField;

    int power = 100;
    public Color low, high;

    public GameObject colorTest;
    MeshRenderer mr;

    int modificatorModeValue = 0;

    public Material transparent,opaque;
    int opacityArg = 0;

    List<string> modificatorModes;
    // Use this for initialization
    void Start () {

        skin = guy.GetComponent<MeshRenderer>();
        gridScript = GetComponent<HumanGrid>();
        gridScript2 = GetComponent<HumanGrid2>();
       // if (gridScript == null) gridScript = GetComponent<HumanGrid2>();

        modificatorModes = new List<string>();


        inputField.SetActive(false);

        mr = colorTest.GetComponent<MeshRenderer>();

        modificatorModes.Add("Y/Z translate");
        modificatorModes.Add("Y Curve");

    }

    // Update is called once per frame
    void Update () {

        mr.material.color = mixColor();
      
	}

    void OnGUI()
    {


        if (GUI.Button(new Rect(20, 220, 220, 30), "Editable "+editable))
        {
            editable = !editable;

        }
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
            FaceTracing faceTracing = GetComponent<FaceTracing>();

            Material opacityMaterial;
            if(opacityArg % 2 == 1)
            {
                opacityMaterial = transparent;

            }
            else
            {

                opacityMaterial = opaque;

            }

            faceTracing.switchOpacity(opacityMaterial, opacityArg);
           
        }

        if (GUI.Button(new Rect(20, 100, 100, 30), "Show Grid"))
        {
            if (gridScript != null)
            {
                gridScript.SwitchVisibleStatus();
                gridScript.SwitchVisibleTracking();

            }


            if(gridScript2!=null)
            {
                gridScript2.SwitchVisibleStatus();
                gridScript2.SwitchVisibleTracking();

            }

        }

        if (GUI.Button(new Rect(20, 150, 100, 30), "Save Group"))
        {
            //gridScript.SwitchVisibleStatus();
            saveGroup = true;
            inputField.SetActive(true);
            //InputField field = inputField.GetComponent<InputField>();

            
            //field.text = groupName;
            
        }

        if (saveGroup)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                             
                InputField field = inputField.GetComponent<InputField>();
                string fieldText = field.text;

                if (fieldText.Length > 0)
                {
                    //save
                    FaceTracing faceTracing = GetComponent<FaceTracing>();
                    saveGroupFile(faceTracing.returnGroupIndexes(), faceTracing.returnTrackingStrengths(), fieldText);
      
                }

                saveGroup = false;
                inputField.SetActive(false);
            }
        }


        GUI.Button(new Rect(20, 250, 150, 30), "Strength:"+power);

       
        
        if (Input.GetKey(KeyCode.X))
        {
            if(power > 0 ) power--;
        }

        if (Input.GetKey(KeyCode.C))
        {
            if (power < 100 ) power++;
        }

        float hM = 30;
        float wM = 120;
        float yM = Screen.height - modificatorModes.Count*hM;
        for(int i = 0; i <modificatorModes.Count; i++)
        {

            if(GUI.Button(new Rect(0, yM, wM, hM), modificatorModes[i]))
            {
                modificatorModeValue = i;

            }

            yM += hM ;
        }
        

    }


    public bool returnEditable()
    {
        return editable;

    }
   
    public int returnModificatorModeValue()
    {
        return modificatorModeValue;
    }

    public Color getColorWithThisValue(int _value)
    {
        return mixColor(_value);
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
