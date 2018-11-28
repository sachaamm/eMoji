using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class SaveAnimFile : MonoBehaviour {

    string faceName;
    SkinnedGUIOptions skinnedGui;

    bool saving = false;

    public GameObject animNameInputField;


	// Use this for initialization
	void Start () {

        animNameInputField.SetActive(false);

        //string fileName = "faceName.txt";

        //string[] lines = System.IO.File.ReadAllLines(@"Assets\" + fileName);// dirty because Assets is inside Assets....<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        //print("set the lines[0]");

        //faceName = lines[0];

        //string[] faceLines = System.IO.File.ReadAllLines(lines[0]);// dirty because Assets is inside Assets....<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        //print(faceLines[0]);
        skinnedGui = GetComponent<SkinnedGUIOptions>();

    }

    public void SetFaceName(string name)
    {

        string[] lines = new string[1];
        lines[0] = name;       

        System.IO.File.WriteAllLines("Assets/faceName.txt", lines);
        faceName = name;
        print(" <<< Set face name :" + name+" >>> ");
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnGUI()
    {

        int modeValue = skinnedGui.returnModeValue();

        if (modeValue == 2)
        {
            if(GUI.Button(new Rect(400,10,150,40),"Save Anim File"))
            {
                saving = !saving;
                animNameInputField.SetActive(saving);
            }

            if (saving)
            {

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    FaceMorpher morpher = GetComponent<FaceMorpher>();
                    List<string> faceNames = morpher.returnFaceNames();
                    List<int> frames = morpher.returnFrames();
                    List<string> lines = new List<string>();

                    lines.Add("" + morpher.nbFrames);

                    for (int i = 0; i < faceNames.Count; i++)
                    {
                        string line = faceNames[i] + "_" + frames[i];
                        lines.Add(line);
                    }

                    InputField field = animNameInputField.GetComponent<InputField>();

                    string fileName = field.text;

                    System.IO.File.WriteAllLines("Assets/AnimFiles/" + fileName + ".txt", lines.ToArray());

                    saving = false;
                    animNameInputField.SetActive(saving);
                }




            }


        }



    }

    public string returnFaceName()
    {
        return faceName;
    }
}
