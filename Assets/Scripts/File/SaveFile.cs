using UnityEngine;
using System.Collections;

public class SaveFile : MonoBehaviour {

    public void saveGroupFile(string[] lines , string name)
    {
        print("saveFile");
        System.IO.File.WriteAllLines("Assets/ConfigFiles/"+name+".txt", lines);

        
    }
}
