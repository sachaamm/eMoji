using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//ATTACH THIS SCRIPT TO MAIN CAMERA
public class SkinnedFaceTracing : MonoBehaviour {

    public GameObject gizmo, vertice, pointCube, pointCubeBigger;

    public GameObject guy;
    public GameObject fbxGuy;
    GameObject rootGuy;

    Vector3 worldPos;

    Vector3[] vertices;
    Mesh fbxMesh;


    List<TrackingPoint> trackingPoints;

    List<int> indexLinks;// Get relative index keys 
 
    public GameObject verticesContainer;
    public GameObject myHitPoint;

    SkinnedMeshRenderer smr;

    public GameObject previewPointCube;
    List<GameObject> preview;

    void Start () {

     
        worldPos = transform.forward;

        initVertices();
        rootGuy = guy.transform.parent.gameObject;

        preview = new List<GameObject>();

	}

    public void resetPreview()
    {

        for(int i = 0; i < preview.Count; i++)
        {
            Destroy(preview[i]);
        }

        preview = new List<GameObject>();

    }

    public void addPreviewPoint(int index)
    {
        GameObject previewCube = GameObject.Instantiate(previewPointCube);
        //preview.Add
        previewCube.transform.position = vertices[index];
        preview.Add(previewCube);

    }


    void initVertices()
    {
        trackingPoints = new List<TrackingPoint>();

        smr = fbxGuy.transform.GetComponent<SkinnedMeshRenderer>();
        fbxMesh = smr.sharedMesh;

        vertices = fbxMesh.vertices;


        //AJOUTE
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] *= fbxGuy.transform.localScale.x;

        }

    }

    public void resetIndexLinks()
    {
        indexLinks = new List<int>();

    }

    public void addIndexLink(int value)
    {
       // if(indexLinks== null) indexLinks = new List<int>();
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
       for(int i = 0; i < trackingPoints.Count; i++)
        {
            int index = trackingPoints[i].indexValue;
            fbxMesh.vertices[index] = trackingPoints[i].point.transform.position;// ICI IL FAUDRA DIVISER PAR SCALING
        }

        print("link to tracking position , trackingPoints Count :" + trackingPoints.Count);
    }
    



    public void clearTracking()
    {

        for(int i = 0; i < trackingPoints.Count; i++)
        {
            trackingPoints[i].DestroyPoint();


        }

        trackingPoints = new List<TrackingPoint>();

   
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

        float scaling = fbxGuy.transform.localScale.x;
        Vector3 trackingPointPos = fbxMesh.vertices[indexVertice] * scaling + rootGuy.transform.position;
    
        GameObject newVertice = GameObject.Instantiate(pointCubeBigger);
        newVertice.transform.position = trackingPointPos;
        MeshRenderer mrv = newVertice.GetComponent<MeshRenderer>();
        SkinnedGUIOptions guiOptions = GetComponent<SkinnedGUIOptions>();
        mrv.material = guiOptions.returnOpacityMaterial();
        mrv.material.color = guiOptions.getColorWithThisValue(value);


        trackingPoints.Add(new TrackingPoint(newVertice, indexVertice, value));
        
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
            Camera main = transform.GetComponent<Camera>();
            Vector2 mousePos = Input.mousePosition;

            float fwd = 10;

            Vector3 mPos = new Vector3(mousePos.x, mousePos.y, fwd);
            worldPos = main.ScreenToWorldPoint(mPos);

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider != null)
                {

                    Vector3 hitPoint = hit.point;
                    int closestVertice = indexClosestVertice(hitPoint);
                    Vector3 fbxPos = vertices[closestVertice]  + rootGuy.transform.position;

                    print("WE HIT ( index closest is " + closestVertice);


                        if (!trackingIndexesContains(closestVertice))
                        {

                            GameObject newVertice = GameObject.Instantiate(vertice);
                            newVertice.transform.position = fbxPos;// MODIFIEE
                           // trackingPoints.Add(new TrackingPoint(newVertice, indexLinks[closestVertice], guiOptions.returnPower()));
                            trackingPoints.Add(new TrackingPoint(newVertice, closestVertice, guiOptions.returnPower()));


                            GameObject newHitPoint = GameObject.Instantiate(myHitPoint);
                            newHitPoint.transform.position = hitPoint;

                            MeshRenderer mrv = newVertice.GetComponent<MeshRenderer>();
                            mrv.material = guiOptions.returnOpacityMaterial();
                            mrv.material.color = guiOptions.returnPowerColor();

                        }
                        else
                        {

                            if (Input.GetKey(KeyCode.LeftShift)) removeTrackingPoint(closestVertice);


                        }


                    /*
                    }
                    else
                    {



                        print("out");

                        GameObject newVertice = GameObject.Instantiate(vertice);
                        newVertice.transform.position = fbxPos;// MODIFIEE

                        GameObject newHitPoint = GameObject.Instantiate(myHitPoint);
                        newHitPoint.transform.position = hitPoint;

                    }


    */








                }


            }



        }



        if (Input.GetKeyDown(KeyCode.Delete)) removeTrackingPoints(false);

        if (Input.GetKeyDown(KeyCode.Z)) removeTrackingPoints(true);
       



    }

    void removeTrackingPoint(int index)
    {
        foreach(TrackingPoint tp in trackingPoints)
        {

            if (tp.indexValue == index)
            {

                tp.DestroyPoint();
                trackingPoints.Remove(tp);
                return;
            }
        }

    }

    bool trackingIndexesContains(int index)
    {

        for(int i = 0; i < trackingPoints.Count; i++)
        {

            if (trackingPoints[i].indexValue == index) return true;

        }

        return false;
    }

    int indexClosestVertice(Vector3 pos)
    {

        int currentIndex = 0;

        float minDist = Mathf.Infinity;

        for(int i = 0; i < vertices.Length; i++)
        {
            float currentDistance = Vector3.Distance(pos, vertices[i]  + guy.transform.position); 

            if(currentDistance < minDist) {
                minDist = currentDistance;
                currentIndex = i;                    
                    }

        }


        return currentIndex;
    }



    
    public void setVertices()// WRONG WAY CUZ WE GONNA LOOSE MESH INDEXATION
    {
        if (vertices == null)
        {

            print("vertices are null so we forced initVertices()");
            initVertices();

        }

        for(int i = 0; i < verticesContainer.transform.childCount; i++)
        {
             vertices[i] = verticesContainer.transform.GetChild(i).transform.position;
           
        }

    }
    



























    void removeTrackingPoints(bool justLast){

        if (trackingPoints.Count == 0) return;

        if (justLast)
        {
            TrackingPoint tp = trackingPoints[trackingPoints.Count - 1];
            tp.DestroyPoint();
            trackingPoints.Remove(tp);

        }
        else
        {

            foreach(TrackingPoint tp in trackingPoints)
            {
                tp.DestroyPoint();



            }

            trackingPoints = new List<TrackingPoint>();
        }

    }

  public List<int> returnIndexLinks()
    {
        return indexLinks;
    }

    public List<int> returnVertexIndexes()
    {
        List<int> trackingIndexes = new List<int>();

        for (int i = 0; i < trackingPoints.Count; i++)
        {
            trackingIndexes.Add(trackingPoints[i].indexValue);

        }
        return trackingIndexes;
    }


    public List<int> returnTrackingStrengths()
    {
        List<int> trackingStrength = new List<int>();

        for (int i = 0; i < trackingPoints.Count; i++)
        {
            trackingStrength.Add(trackingPoints[i].strength);

        }
        return trackingStrength;
    }


    public int[] returnTrackingValues()
    {

        int[] trackingStrengths = new int[trackingPoints.Count];
        for (int i = 0; i < trackingPoints.Count; i++)
        {
            trackingStrengths[i] = trackingPoints[i].strength;

        }
        return trackingStrengths;

    }


    public void switchOpacity(Material m, int arg)
    {

            SkinnedGUIOptions guiOptions = GetComponent<SkinnedGUIOptions>();
            
            for(int i = 0; i < trackingPoints.Count; i++)
            {
                MeshRenderer mr = trackingPoints[i].point.GetComponent<MeshRenderer>();
                mr.material = m;
                Color strengthColor = guiOptions.mixColor(trackingPoints[i].strength);
                mr.material.color = strengthColor;

            }
        
    }


    public Vector3[] returnCurrentFace()
    {
        Vector3[] face = new Vector3[indexLinks.Count];
        //print("index Link Count "+indexLinks.Count);
        for(int i = 0; i < indexLinks.Count; i++)
        {

            face[i] = smr.sharedMesh.vertices[indexLinks[i]];

        }

        return face;


    }


    public void setFaceVertices(Vector3[] faceV)
    {

        Vector3[] vertices = smr.sharedMesh.vertices;
        
        for(int i = 0; i < indexLinks.Count; i++)
        {

            vertices[indexLinks[i]] = faceV[i];
        }

        smr.sharedMesh.SetVertices(new List<Vector3>(vertices));
        smr.sharedMesh.RecalculateBounds();
        smr.sharedMesh.RecalculateNormals();



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
