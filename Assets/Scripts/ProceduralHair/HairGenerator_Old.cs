using UnityEngine;
using System.Collections;

public class HairGenerator_Old : MonoBehaviour {

    Vector3[] vertices;
    int[] faces;

    Mesh m;


    public int nbModule = 3;
    public float noiseAmplitude = 1.0f;

    public float yMin, yMax;
    public float tailleModule = 1;

    float defaultY = 0;
    //float[] noiseA,noiseB;

    public bool autoLoad = false;
    //public float defaultY = 
    public float XcosineHeight = 1;
    public float ZcosineHeight = 2;
    public float circleCosineHeight = 4;

    float prevXcosineH = 0;
    float prevZcosineH = 0;

    bool updateBuild = true;

    Vector3[] saveVertices;
    public float[] noiseValues;
    
    // Use this for initialization
    void Start () {



        m = GetComponent<MeshFilter>().mesh;



        prevXcosineH = XcosineHeight;
        prevZcosineH = ZcosineHeight;


        saveVertices = new Vector3[nbModule*nbModule];
        noiseValues = new float[nbModule*nbModule];


        vertices = new Vector3[nbModule*nbModule];


        init(defaultY, yMin, yMax, noiseAmplitude);




        if (autoLoad)
        {
            
           Build();
           createWave();
            AssignMesh();
        }



        for (int i = 0; i < saveVertices.Length; i++)
        {
            saveVertices[i] = m.vertices[i];
            noiseValues[i] = 0;
        }


    }

    public void init(float _default,float _yMin, float _yMax,float noise)
    {

        //rightYaverage = _rightAverage;
        //topYaverage = _topAverage;

        defaultY = _default;
        

        yMin = _yMin;
        yMax = _yMax;
        noiseAmplitude = noise;

        for(int i = 0; i < noiseValues.Length; i++)
        {
            noiseValues[i] = 0;


        }

    }

    public void Build()
    {
        vertices = new Vector3[nbModule * nbModule];
        faces = new int[(nbModule - 1) * (nbModule - 1) * 3 * 2];



        m = GetComponent<MeshFilter>().mesh;

        //noiseA = new float[nbModule];
        //noiseB = new float[nbModule];

        BuildTerrain();
        // AddNoise();
        //createWave();
        //AssignMesh();
       


    }



    // A CE MOMENT LA , IL FAUDRAIT QUE TOP Y AVERAGE SOIT DEJA DEFINI
    public void createWave()
    {

        //noiseA[0] = 0;
        //noiseB[0] = 0;

        print("create Wave");

        for(int i = 0; i < nbModule* nbModule; i++)
        {

            noiseValues[i] =  Random.Range(yMin, yMax);


        }

        for (int i = 0; i < nbModule * nbModule; i++)
        {
            int index = i;
            //   print("nbModule " + nbModule + " query " + index+" vertices Length "+vertices.Length+" noise Value "+noiseValues[i+j*nbModule]);
            vertices[index] += new Vector3(0, noiseValues[index], 0);



        }
        /*
        for (int i = 1; i < nbModule; i++)
        {

            for (int j = 0; j < nbModule; j++)
            {
                //noiseA[i] = noiseA[i - 1] + Random.Range(yMin,yMax);
                //noiseB[i] = noiseB[i - 1] + Random.Range(yMin,yMax);


                 noiseValues[i+j*nbModule-1] = noiseValues[i + j * nbModule] + Random.Range(yMin, yMax);
                 //if(j>0)noiseValues[i+j*nbModule] = noiseValues[i + j * nbModule - 1 - nbModule] + Random.Range(yMin, yMax);
               // noiseValues[i + j * nbModule] = 0;
            }

        }
        */


        /*
        //ADD WAVE
        for (int i = 0; i < nbModule; i++)
        {
            for (int j = 0; j < nbModule; j++)
            {

                //vertices[i + j * nbModule] += new Vector3(0, noiseA[j], 0);

                //vertices[i + j * nbModule] += new Vector3(0, noiseB[i], 0);

              
                int index = i + j * nbModule;
             //   print("nbModule " + nbModule + " query " + index+" vertices Length "+vertices.Length+" noise Value "+noiseValues[i+j*nbModule]);
                vertices[index] += new Vector3(0, noiseValues[index], 0);
            }

        }

        */


    }


    void BuildTerrain()
    {
       // print("BUILD TERRAIN");

        vertices = new Vector3[nbModule * nbModule];

        for(int i = 0; i < nbModule* nbModule; i++)
        {
            vertices[i] = new Vector3(0, 0, 0);


        }

        if (saveVertices != null)
        {
          //  print("save Vertices Length " + saveVertices.Length);
            vertices = saveVertices;
        }
        faces = new int[(nbModule - 1) * (nbModule - 1) * 3 * 2];

        Vector3 v = transform.position;


        float angle = Mathf.PI;
        //float angle2 = Mathf.PI;




        //ADD NOISE
        for (int i = 0; i < nbModule * nbModule; i++)
        {
            if (i % nbModule == 0) angle = Mathf.PI;
           

            float zVal = ((i - (i % nbModule)) / nbModule);

            float angle2 = Mathf.PI + zVal / nbModule * Mathf.PI*2 ;
            float angle3 = angle2 + Mathf.PI + (i% nbModule)  / nbModule * Mathf.PI*2 ;



            
             angle += Mathf.PI*2 / nbModule;
             

            float addYCosine = Mathf.Cos(angle) * XcosineHeight;
            float addYCosine2 = Mathf.Cos(angle2) * ZcosineHeight;
            float circleY = Mathf.Cos(angle3) * circleCosineHeight;

             vertices[i] = new Vector3(v.x + (i % nbModule) * tailleModule, v.y + defaultY + addYCosine + addYCosine2 + circleY, v.z + zVal * tailleModule);
             vertices[i] += new Vector3(0,noiseValues[i],0);

           // vertices[i] += new Vector3(0, 0, 0);
        }

        //
        int a = 0;
        for(int i = 0; i < nbModule-1; i++)
        {
            
            for (int j = 0; j < nbModule - 1; j++)
            {

                faces[a * 6] = j + i * nbModule + 1;
                faces[a * 6 + 1] = j + i * nbModule;
                faces[a * 6 + 2] = j + i * nbModule + nbModule;
                faces[a * 6 + 3] = j + i * nbModule + 1;
                faces[a * 6 + 4] = j + i * nbModule +nbModule;
                faces[a * 6 + 5] = j + i * nbModule + nbModule + 1;
                a++;

            }

        }




    }


    void AddNoise()
    {
    
        //ADD NOISE
        for (int i = 0; i < nbModule * nbModule; i++)
        {
            vertices[i] += new Vector3(0, Random.Range(0.0f, noiseAmplitude) * tailleModule, 0);
        }

    }
    // Update is called once per frame
    void Update () {

        

        m.RecalculateNormals();

        if (Input.GetKeyDown(KeyCode.O))
        {

          //  updateBuild = true;


        }

   

        BuildTerrain();

        // createWave();
        if (prevXcosineH != XcosineHeight || prevZcosineH != ZcosineHeight)
        {

            createWave(); // ADD NOISE
        }

        prevXcosineH = XcosineHeight;
        prevZcosineH = ZcosineHeight;




        AssignMesh();


        if (updateBuild)
        {


            BuildTerrain();
            //AssignMesh();

        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //   addWave();

            AddNoise();
            AssignMesh();

          //  updateBuild = false;

        }

        

    }









    public Vector3[] returnRightVertices()
    {

        Vector3[] right = new Vector3[nbModule];

        int j = 0;
        for(int i = nbModule-1; i < nbModule * nbModule; i += nbModule)
        {
            right[j] = vertices[i];
            j++;
        }

        return right;
    }

    public Vector3[] returnLeftVertices()
    {

        Vector3[] left = new Vector3[nbModule];

        int j = 0;
        for (int i = 0; i < nbModule * nbModule; i += nbModule)
        {
            left[j] = vertices[i];
            j++;
        }


        return left;

    }


    public Vector3[] returnBottomVertices()
    {
        Vector3[] bottom = new Vector3[nbModule];
        int a = 0;

        for(int i = 0; i < nbModule; i++)
        {
            bottom[i] = vertices[i];
            a++;
        }

        return bottom;

    }




    public Vector3[] returnTopVertices()
    {
        Vector3[] top = new Vector3[nbModule];
 
        int a = 0;

        for (int i = ((nbModule - 1) * nbModule); i < (nbModule * nbModule); i++)
        {
            top[a] = vertices[i];    
            a++;

        }

        return top;

    }




    public  void setLeftVertices(Vector3[] right,Vector3 pos)
    {

        int j = 0;
        for (int i = 0 ; i < nbModule * nbModule; i += nbModule)
        {
            vertices[i] = right[j] - pos;
            j++;
        }


    }

    public void setBottomVertices(Vector3[] top, Vector3 pos)
    {

        int j = 0;
        for (int i =0;i<nbModule ; i++)
        {
            vertices[i] = top[j] - pos;
            j++;
        }

       // print(topYaverage);

    }


    
    /*
    public void Debug(GameObject debug)
    {

        Vector3[] right = returnTopVertices();
        for (int i = 0; i < right.Length; i++)
        {
            GameObject o = GameObject.Instantiate(debug);
            o.transform.position = right[i];

        }

    }
    
    */
    /*
    public void Debug(GameObject debug)
    {

       float y = getTopYBase();
       // for (int i = 0; i < right.Length; i++)
       // {
            GameObject o = GameObject.Instantiate(debug);
        o.transform.position = vertices[((nbModule - 1) * nbModule)] + transform.position;

        //}

    }
    */

    public void AssignMesh()
    {

     

      
        m.vertices = vertices;
        m.triangles = faces;
        m.RecalculateBounds();
        m.RecalculateNormals();

        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        m.uv = uvs;

        GetComponent<MeshCollider>().sharedMesh = m;



    }






    public void Debug(int index)
    {

        print("my default is " + defaultY + "my INDEX is " + index);
    }



















































    public float getRightVerticesAverage()
    {

        // Vector3[] right = new Vector3[nbModule];

        float v = 0;

        for (int i = nbModule - 1; i < nbModule * nbModule; i += nbModule)
        {
            v += vertices[i].y;

        }


        return v / nbModule;

    }

    public float getTopVerticesAverage()
    {

        // Vector3[] right = new Vector3[nbModule];

        float v = 0;

        for (int i = ((nbModule - 1) * nbModule); i < (nbModule * nbModule); i++)
        {
            v += vertices[i].y;

        }


        return v / nbModule;


    }


    /*
    public float getDefault()
    {


        return (getRightVerticesYAverage() + getTopVerticesYAverage()) / 2;

    }
    */



    public float getYAverage()
    {



        float v = 0;
        for (int i = 0; i < nbModule * nbModule; i += nbModule)
        {
            v += vertices[i].y;
        }

        return v / (nbModule * nbModule);

    }
    public float getRightYBase()
    {

        return vertices[nbModule - 1].y;
    }

    public float getTopYBase()
    {

        return vertices[((nbModule - 1) * nbModule)].y;
    }











    public void smoothBottom(Vector3[] top, Vector3 pos)
    {

        int currentIndex = 0;
        float smooth = 0;

        for (int k = 0; k < nbModule; k++)
        {
            currentIndex = k * nbModule;
            int j = 0;
            smooth += 1 / (float)nbModule;

            for (int i = currentIndex; i < currentIndex + nbModule; i++)
            {

                float smoothY = ((smooth * top[j].y) - ((1 - smooth) * vertices[i].y));
                vertices[i] += new Vector3(0, smoothY, 0);

                j++;
            }

        }


    }


    public void smoothLeft(Vector3[] right, Vector3 pos)
    {

        int currentIndex = 0;
        float smooth = 0;

        for (int k = 0; k < nbModule; k++)
        {
            currentIndex++;
            int j = 0;
            smooth += 1 / (float)nbModule;

            for (int i = currentIndex; i < nbModule * nbModule; i += nbModule)
            {

                float smoothY = ((smooth * right[j].y) - ((1 - smooth) * vertices[i].y));
                vertices[i] += new Vector3(0, smoothY, 0);


                j++;
            }

        }






    }
}
