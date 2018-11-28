using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//ATTACH THIS SCRIPT TO MAIN CAMERA
public class FaceTracing : MonoBehaviour {

    Camera main;
    public GameObject gizmo, vertice, pointCube, pointCubeBigger;

  //  public GameObject bigCube;

    public GameObject guy, parent;
    GameObject rootGuy;

    Vector3 worldPos;

    Vector3[] vertices;
    Mesh m;

    List<GameObject> trackingPoints;
    List<int> trackingIndexes;
    List<int> trackingStrengths;

    //public GameObject trsGizmo;
    //TRS_Gizmo trsGizmoScript;


    bool animate = false;


    // Use this for initialization
    void Start () {

        main = transform.GetComponent<Camera>();
        worldPos = transform.forward;

        MeshFilter mf = guy.transform.GetComponent<MeshFilter>();
        m = mf.mesh; 
        vertices = m.vertices;

        rootGuy = guy.transform.parent.gameObject;

        trackingPoints = new List<GameObject>();
        trackingIndexes = new List<int>();
        trackingStrengths = new List<int>();

        //trsGizmoScript = trsGizmo.GetComponent<TRS_Gizmo>();
        //trsGizmoScript.startTRSGizmo(rootGuy);
	}

   


    public void LinkToMesh()
    {
        linkToTrackingPosition();

    }
    public void linkToTrackingPosition()
    {
       for(int i = 0; i < trackingIndexes.Count; i++)
        {
            int index = trackingIndexes[i];
            m.vertices[index] = trackingPoints[i].transform.position;
        }
    }

    public void MoveWithIndexes()
    {
        print("Move with INdexes with Count of "+trackingIndexes.Count);

        MeshFilter mf = guy.GetComponent<MeshFilter>();
        Mesh currentMesh = mf.mesh;

        List<Vector3> vertices = new List<Vector3>();
        for(int i = 0; i < currentMesh.vertices.Length; i++)
        {
            vertices.Add(currentMesh.vertices[i]);
        }
            
            
         //   currentMesh.vertices.OfType<Vector3>().ToList();


        for (int i = 0; i < trackingIndexes.Count; i++)
        {
            int index = trackingIndexes[i];
          vertices[index] +=new Vector3(0,0.1f,0);
        }

        
        print("nbV >>>" + currentMesh.vertices.Length);
        currentMesh.SetVertices(vertices);
        currentMesh.RecalculateBounds();
        currentMesh.RecalculateNormals();
       
        mf.mesh = currentMesh;
   
    }

    public List<int> returnVertexIndexes()
    {
        return trackingIndexes;
    }

    public int[] returnTrackingValues()
    {
        return trackingStrengths.ToArray();

    }


    public void clearTracking()
    {

        trackingIndexes = new List<int>();

        for (int i = 0; i < trackingPoints.Count; i++)
        {
            Destroy(trackingPoints[i]);

        }
        trackingPoints = new List<GameObject>();

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
        Vector3 trackingPointPos = m.vertices[indexVertice] + rootGuy.transform.position;
    
        GameObject newVertice = GameObject.Instantiate(pointCubeBigger);
        newVertice.transform.position = trackingPointPos;
        MeshRenderer mrv = newVertice.GetComponent<MeshRenderer>();
        GUIOptions guiOptions = GetComponent<GUIOptions>();
        mrv.material = guiOptions.returnOpacityMaterial();
        mrv.material.color = guiOptions.getColorWithThisValue(value);
        //print("trackingPoint " + trackingPointPos);


        trackingPoints.Add(newVertice);
        trackingIndexes.Add(indexVertice);
        trackingStrengths.Add(value);

        Transform cleaner = GameObject.Find("Cleaner").transform;
        newVertice.transform.parent = cleaner;
      
    }



	
	// Update is called once per frame
	void Update () {



        // Debug.DrawRay(transform.position, worldPos - transform.position, Color.green);

        // linkToTrackingPosition();
        GUIOptions guiOptions = GetComponent<GUIOptions>();


        if (Input.GetMouseButtonDown(0) && guiOptions.returnEditable())
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
                    Vector3 vertexPos = vertices[closestVertice] + rootGuy.transform.position;
                       
                    GameObject newVertice = GameObject.Instantiate(vertice);
                    newVertice.transform.position = vertexPos;
                    MeshRenderer mrv = newVertice.GetComponent<MeshRenderer>();
                    mrv.material = guiOptions.returnOpacityMaterial();
                    mrv.material.color = guiOptions.returnPowerColor();

                    trackingPoints.Add(newVertice);
                    trackingIndexes.Add(closestVertice);
                    trackingStrengths.Add(guiOptions.returnPower());

                }
          

            }



        }

        if (Input.GetKeyDown(KeyCode.Delete)) removeTrackingPoints(false);

        if (Input.GetKeyDown(KeyCode.Z)) removeTrackingPoints(true);


        if (Input.GetKeyDown(KeyCode.Y)) animate = !animate;

        if (animate)
        {



        }






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


    void removeTrackingPoints(bool justLast){

        for (int i = 0; i < trackingPoints.Count; i++)
        {
            if (!justLast)
            {
                Destroy(trackingPoints[i]);
            }
            else
            {
                if (i == trackingPoints.Count - 1) Destroy(trackingPoints[i]);

            }
        
        }

        if (!justLast)
        {
            trackingPoints = new List<GameObject>();
            trackingIndexes = new List<int>();
        }
        else
        {
            trackingPoints.Remove(trackingPoints[trackingPoints.Count-1]);
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
            

            for(int i = 0; i < trackingPoints.Count; i++)
            {
                MeshRenderer mr = trackingPoints[i].GetComponent<MeshRenderer>();
                mr.material = m;
                Color strengthColor = guiOptions.mixColor(trackingStrengths[i]);
                mr.material.color = strengthColor;

            }
        


    }
}
