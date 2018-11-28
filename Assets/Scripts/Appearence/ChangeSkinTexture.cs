using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeSkinTexture : MonoBehaviour {

    SkinnedGUIOptions skinnedGui;

    public Texture[] skinTextures;
    int selected = 0;
    Material skinMaterial;
    public GameObject fbxGuy;

    public GameObject xTiling, yTiling;
    Slider xTilingSlider, yTilingSlider;

	// Use this for initialization
	void Start () {

        skinnedGui = GetComponent<SkinnedGUIOptions>();
        skinMaterial = fbxGuy.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        xTilingSlider = xTiling.GetComponent<Slider>();
        yTilingSlider = yTiling.GetComponent<Slider>();

        xTilingSlider.value = skinMaterial.mainTextureScale.x;
        yTilingSlider.value = skinMaterial.mainTextureScale.y;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        // SKIN 
        if (skinnedGui.returnModeValue() == 4)
        {


            float xTiling = xTilingSlider.value;
            float yTiling = yTilingSlider.value;

            //skinMaterial.SetTextureOffset("_MainTex",new Vector2(xTiling, yTiling)  );
            skinMaterial.SetTextureScale("_MainTex", new Vector2(xTiling, yTiling));
           
            // skinMaterial.mainTextureOffset.y = yTilingSlider;




            float sw = 200;
            float sy = 20;
            for (int i = 0; i < skinTextures.Length; i++)
            {
                Rect r = new Rect(Screen.width - sw, sy, sw, sw);

                if (selected == i)
                {
                    GUI.color = Color.yellow;
                }
                else
                {

                    GUI.color = Color.white;
                }
                if (GUI.Button(r, ""))
                {
                    selected = i;

                }

                GUI.DrawTexture(r, skinTextures[i]);

                sy += sw;
            }


            if (GUI.Button(new Rect(Screen.width - sw, 0, sw, 20), "Load Texture"))
            {
                //print("ok");
                skinMaterial.mainTexture = skinTextures[selected];

            }


        }

    }

}
