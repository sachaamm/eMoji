using UnityEngine;
using System.Collections;

public class RiggingController : MonoBehaviour {
    public GameObject rootGuy;
    public GameObject fbxGuy,newFbxGuy;
    Animator animator;
    bool rigAnim = false;

    SkinnedGUIOptions skinnedGui;
	// Use this for initialization
	void Start () {

        animator = rootGuy.GetComponent<Animator>();
        animator.enabled = rigAnim;

        skinnedGui = GetComponent<SkinnedGUIOptions>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {

        if(skinnedGui.returnModeValue() == 2)
        {

            if (GUI.Button(new Rect(0, 0, 100, 20), "RigAnim:" + rigAnim))
            {

                rigAnim = !rigAnim;
                animator.enabled = rigAnim;

                //float normalizeScaling = fbxGuy.transform.localScale.x / newFbxGuy.transform.localScale.x;
                //rootGuy.transform.localScale *= normalizeScaling;


            }



        }
     


    }



    
}
