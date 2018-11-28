using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//ATTACH THIS SCRIPT TO MAIN CAMERA
public class SkinnedFaceTracingOld : MonoBehaviour {

    Camera main;
    public GameObject gizmo, vertice, pointCube, pointCubeBigger;

    public GameObject guy;
    public GameObject fbxGuy;
    GameObject rootGuy;

    Vector3 worldPos;

    Vector3[] vertices;
    //Mesh m;
    Mesh fbxMesh;

    
    List<GameObject> trackingPointsGameObjects;
    List<int> trackingIndexes;
    List<int> trackingStrengths;

    List<int> indexLinks;// Get relative index keys 

    
    public GameObject verticesContainer;
    public GameObject myHitPoint;

   // List<TrackingPoint> trackingPoin
    // Use this for initialization
    void Start () {

        main = transform.GetComponent<Camera>();
        worldPos = transform.forward;

        //MeshFilter mf = guy.transform.GetComponent<MeshFilter>();
         //m = mf.mesh;
        SkinnedMeshRenderer smr = fbxGuy.transform.GetComponent<SkinnedMeshRenderer>();
        fbxMesh = smr.sharedMesh; 

        vertices = fbxMesh.vertices;

        rootGuy = guy.transform.parent.gameObject;

        trackingPointsGameObjects = new List<GameObject>();
        trackingIndexes = new List<int>();
        trackingStrengths = new List<int>();

        indexLinks = new List<int>();
	}

   
    public void addIndexLink(int value)
    {

        indexLinks.Add(value);

    }

    public void removeLastIndexLink()
    {

        indexLinks.Remove(indexLinks[indexLinks.Count - 1]);
    }

    public void LinkToMesh()
    {
        //linkToTrackingPosition();
    }

    
    public void linkToTrackingPosition()
    {
       for(int i = 0; i < trackingIndexes.Count; i++)
        {
            int index = trackingIndexes[i];
            fbxMesh.vertices[index] = trackingPointsGameObjects[i].transform.position;// ICI IL FAUDRA DIVISER PAR SCALING
        }
    }
    


    public List<int> returnVertexIndexes()
    {
        return trackingIndexes;
    }

    /*
    public List<int> returnIndexLinks()
    {
        return indexLinks;
    }*/


    public int[] returnTrackingValues()
    {
        return trackingStrengths.ToArray();

    }


    public void clearTracking()
    {
        trackingIndexes = new List<int>();

        for (int i = 0; i < trackingPointsGameObjects.Count; i++)
        { Destroy(trackingPointsGameObjects[i]);
        }

        trackingPointsGameObjects = new List<GameObject>();

        Transform cleaner = GameObject.Find("Cleaner").transform;

        for (int i = 0; i < cleaner.childCount; i++)
        {
            //  cleaner
            Transform child = cleaner.GetChild(i);
            Destroy(child.gameObject);
        }

    }
    public void addTrackingPoint(int indexVertice,int value)
    {

        //int indexVertice = 0;
        float scaling = fbxGuy.transform.localScale.x;
        Vector3 trackingPointPos = fbxMesh.vertices[indexVertice] * scaling + rootGuy.transform.position;
    
        GameObject newVertice = GameObject.Instantiate(pointCubeBigger);
        newVertice.transform.position = trackingPointPos;
        MeshRenderer mrv = newVertice.GetComponent<MeshRenderer>();
        SkinnedGUIOptions guiOptions = GetComponent<SkinnedGUIOptions>();
        mrv.material = guiOptions.returnOpacityMaterial();
        mrv.material.color = guiOptions.getColorWithThisValue(value);
        //print("trackingPoint " + trackingPointPos);




        trackingPointsGameObjects.Add(newVertice);
        trackingIndexes.Add(indexVertice);
        trackingStrengths.Add(value);

        
        Transform cleaner = GameObject.Find("Cleaner").transform;
        newVertice.transform.parent = cleaner;
      
    }



	
	// Update is called once per frame
	void Update () {



        Debug.DrawRay(transform.position, worldPos - transform.position, Color.green);

        // linkToTrackingPosition();
        SkinnedGUIOptions guiOptions = GetComponent<SkinnedGUIOptions>();



        if (Input.GetMouseButtonDown(0) && guiOptions.returnModeValue() == 0)
        {

            Vector2 mousePos = Input.mousePosition;

            float fwd = 10;

            Vector3 mPos = new Vector3(mousePos.x, mousePos.y, fwd);
            worldPos = main.ScreenToWorldPoint(mPos);

            if (Physics.Raycast(transform.position, worldPos - transform.position, fwd))
            {
                //print("There is something in front of the object!");
                Debug.DrawRay(transform.position, worldPos);

            }


            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider != null)
                {

                    Vector3 hitPoint = hit.point;



                    int closestVertice = indexClosestVertice(hitPoint);
                    //vertexPos is the closest Vertice to collision point
                    Vector3 fbxPos = vertices[closestVertice]   + rootGuy.transform.position;// * SCALING

                    GameObject newVertice = GameObject.Instantiate(vertice);
                    //newVertice.transform.position = vertexPos;

                    //print("closest Vertice "+closestVertice+" indexLinks Count"+indexLinks.Count);

                  
                    //Vector3 fbxPos = fbxMesh.vertices[indexVertice];

                    print(fbxGuy.transform.localScale.x);
                    //newVertice.transform.position = fbxPos * fbxGuy.transform.localScale.x;
                    newVertice.transform.position = fbxPos ;


                    MeshRenderer mrv = newVertice.GetComponent<MeshRenderer>();
                    mrv.material = guiOptions.returnOpacityMaterial();
                    mrv.material.color = guiOptions.returnPowerColor();



                    if (!trackingIndexesContains(indexLinks[closestVertice]))
                    {
                        trackingPointsGameObjects.Add(newVertice);
                        //trackingIndexes.Add(closestVertice);
                        trackingStrengths.Add(guiOptions.returnPower());
                        trackingIndexes.Add(indexLinks[closestVertice]);

                        GameObject newHitPoint = GameObject.Instantiate(myHitPoint);
                        newHitPoint.transform.position = hitPoint;

                    }




                }


            }



        }

        if (Input.GetKeyDown(KeyCode.Delete)) removeTrackingPoints(false);

        if (Input.GetKeyDown(KeyCode.Z))
        {

            removeTrackingPoints(true);
            //removeLastIndexLink();
        }




    }

    bool trackingIndexesContains(int index)
    {

        for(int i = 0; i < trackingIndexes.Count; i++)
        {

            if (trackingIndexes[i] == index) return true;

        }

        return false;
    }

    int indexClosestVertice(Vector3 pos)
    {

        int currentIndex = 0;

        float minDist = Mathf.Infinity;

        for(int i = 0; i < vertices.Length; i++)
        {
            float currentDistance = Vector3.Distance(pos, vertices[i] + guy.transform.position);

            if(currentDistance < minDist) {
                minDist = currentDistance;
                currentIndex = i;                    
                    }

        }


        return currentIndex;
    }



    
    public void setVertices()// WRONG WAY CUZ WE GONNA LOOSE MESH INDEXATION
    {
        for(int i = 0; i < verticesContainer.transform.childCount; i++)
        {
             vertices[i] = verticesContainer.transform.GetChild(i).transform.position;
            // vertices[i] = new Vector3(0, 0, 0);
        }

    }
    



























    void removeTrackingPoints(bool justLast){

        for (int i = 0; i < trackingPointsGameObjects.Count; i++)
        {
            if (!justLast)
            {
                Destroy(trackingPointsGameObjects[i]);
            }
            else
            {
                if (i == trackingPointsGameObjects.Count - 1) Destroy(trackingPointsGameObjects[i]);

            }
        
        }

        if (!justLast)
        {
            trackingPointsGameObjects = new List<GameObject>();
            trackingIndexes = new List<int>();
        }
        else
        {
            trackingPointsGameObjects.Remove(trackingPointsGameObjects[trackingPointsGameObjects.Count-1]);
            trackingIndexes.Remove(trackingIndexes[trackingIndexes.Count-1]);
        }
    }

    public List<int> returnGroupIndexes()
    {
        return trackingIndexes;
    }

    public List<int> returnTrackingStrengths()
    {
        return trackingStrengths;
    }

    public void switchOpacity(Material m, int arg)
    {

            GUIOptions guiOptions = GetComponent<GUIOptions>();
            
            for(int i = 0; i < trackingPointsGameObjects.Count; i++)
            {
                MeshRenderer mr = trackingPointsGameObjects[i].GetComponent<MeshRenderer>();
                mr.material = m;
                Color strengthColor = guiOptions.mixColor(trackingStrengths[i]);
                mr.material.color = strengthColor;

            }
        
    }




    class TrackingPoint
    {
        public GameObject point;
        public int indexValue;
        public int strength;

        public TrackingPoint(GameObject _p,int index,int str)
        {
            point = _p;
            indexValue = index;
            strength = str;


        }

        public void DestroyPoint()
        {

            Destroy(point);
        }






    }

}
