using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class ChangeGridCubeSize : MonoBehaviour {

    SkinnedGUIOptions skinnedGui;

    bool enable = false;
    public GameObject cubeSizeSlider;
    public GameObject childGroup;


	// Use this for initialization
	void Start () {
        skinnedGui = GetComponent<SkinnedGUIOptions>();
        cubeSizeSlider.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {


        if (skinnedGui.returnModeValue() == 1 && enable)
        {

            Slider slider = cubeSizeSlider.GetComponent<Slider>();

            float v = slider.value;

            int nbChild = childGroup.transform.childCount;

            for (int i = 0; i < nbChild; i++)
            {
                childGroup.transform.GetChild(i).localScale = new Vector3(v, v, v);
            }


        }

        if (skinnedGui.returnModeValue() != 1 )
        {
            cubeSizeSlider.SetActive(false);
        }

    }

    void OnGUI()
    {
        if(skinnedGui.returnModeValue() == 1)
        {
            if (enable)
            {
                GUI.color = Color.yellow;
            }
            else
            {
                GUI.color = Color.white;

            }


            if(GUI.Button(new Rect(300, 320, 150, 50),"Change Grid-Cube Size")){

                enable = !enable;

                cubeSizeSlider.SetActive(enable);
            }

        }



    }
}
