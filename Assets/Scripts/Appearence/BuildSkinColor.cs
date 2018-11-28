using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.IO;

public class BuildSkinColor : MonoBehaviour {
    SkinnedGUIOptions skinnedGui;
    public GameObject redSlider, greenSlider, blueSlider;
    public GameObject fbxGuy;
    Material m;

    Slider redSliderObject;
    Slider greenSliderObject;
    Slider blueSliderObject;

    SkinnedMeshRenderer smr;

    public GameObject xTiling, yTiling;
    // Use this for initialization
    void Start()
    {

        redSliderObject = redSlider.GetComponent<Slider>();
        greenSliderObject = greenSlider.GetComponent<Slider>();
        blueSliderObject = blueSlider.GetComponent<Slider>();

        //loadColorFile();

        skinnedGui = GetComponent<SkinnedGUIOptions>();


 

        redSlider.SetActive(false);
        greenSlider.SetActive(false);
        blueSlider.SetActive(false);

        smr = fbxGuy.GetComponent<SkinnedMeshRenderer>();

        m = smr.sharedMaterial;
        print("Mesh material is : "+m.name);
        //loadColorFile();

        //float r = m.color.r;




        redSliderObject.value = m.color.r;
        greenSliderObject.value = m.color.g;
        blueSliderObject.value = m.color.b;

    }

    void loadColorFile()
    {

        // Process the list of files found in the directory.
        string[] filePaths = Directory.GetFiles(@"Assets\ColorFiles\");
        string fileName = filePaths[0];

        string[] lines = System.IO.File.ReadAllLines(@"" + fileName);// dirty because Assets is inside Assets....<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<



        // Display the file contents by using a foreach loop.
        foreach (string line in lines)
        {
            string[] values = line.Split('_');
            //string input = values[1];// Index Value
            float r = float.Parse(values[0]);
            float g = float.Parse(values[1]);
            float b = float.Parse(values[2]);

            Material m = smr.sharedMaterial;
            m.color = new Color(r, g, b);
            smr.sharedMaterial = m;
        }





   }

    public void saveColorFile(string[] lines)
    {

        System.IO.File.WriteAllLines("Assets/ColorFiles/colorSkin.txt", lines);


    }
    // Update is called once per frame
    void Update()
    {

    }
    public void setSliders(bool state)
    {
        redSlider.SetActive(state);

        greenSlider.SetActive(state);

        blueSlider.SetActive(state);

        xTiling.SetActive(state);
        yTiling.SetActive(state);





    }
    void OnGUI()
    {

        if (skinnedGui.returnModeValue() == 4)
        {

            float redValue = redSliderObject.value;
            float greenValue = greenSliderObject.value;
            float blueValue = blueSliderObject.value;


            if (GUI.Button(new Rect(50, 600, 100, 30), "SetSkinColor"))
            {
                Color c = smr.sharedMaterial.color;
                string s = c.r + "_" + c.g + "_" + c.b;
                string[] lines = new string[1];
                lines[0] = s;
                saveColorFile(lines);

            }

            Color skinColor = new Color(redValue, greenValue, blueValue);
            m.color = skinColor;


        }
    
     



    }
}
