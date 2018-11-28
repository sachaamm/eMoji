﻿using UnityEngine;
using System.Collections;
//using UnityEditor;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Material))]
//[CanEditMultipleObjects]




public class HairGenerator_Modulate : MonoBehaviour
    {

    Vector3[] vertices;
    int[] faces;

    public Mesh m;
    [Header("Faces Settings")]
    public bool rectoVerso = true;
   

    public bool grided = true;

    public int nbModule = 3;
   // int prevNbModule;


    [Header("Mesh Size")]

    public float xGap = 1;
    public float zGap = 1;

    [Header("Modulo")]

    //
    public int moduloX = 3;
    public float moduloXValue = 1;

    
    //
    public int moduloY = 2;
    public float moduloYValue = 1;


    [Header("Forehead Scaling")]

    public float foreheadDistance = 1;

    [Header("Trigonometry Twisting")]

    public float sinTwistDistance = 0;
    public float divideSin = 1;
    public float modulateSin = 100;
    public float cosTwistDistance = 0;
    public float divideCos = 1;
    public float modulateCos = 100;
    public float tanTwistDistance = 0;
    public int modulateTangent = 4;
    public float divideTangent = 1000;


    public float XcosineHeight = 1;
    public float ZcosineHeight = 2;
    public float circleCosineHeight = 4;

    /*
    float prevXcosineH = 0;
    float prevZcosineH = 0;
    */
   // bool updateBuild = true;


    
    Vector3[] saveVertices;


    Vector3[] foreheadDistances;
    Vector3[] distances;
    /*
    Vector3[] saveLeft;
    Vector3[] saveRight;
    Vector3[] saveTop, saveBottom;*/

    [Header("Side Clipping")]

    public float resetLeftValue = 1;
    public float resetTopValue = 1;
    public float resetBottomValue = 1;
    public float moveBottomValue = 0;


    [Header("Additional Frequencies")]


    public float doubleSineStrength = 1;
    public float thirdFrequency = 4;
    public float thirdSineStrength = 1;


    Vector3 foreheadCenter;

    [Header("Evolution")]

    public float limitEvolutionFrequency = 1;
    public float evolutionFrequency = 1;
    public float evolutionFactorFrequency = 1;
    public float evolutionFactor = 1;
    public AnimationCurve sideEvolutionClip;
    public float[] evolutionValues;
    public float evolutionThreshold = 0.8f;
    float[] curveEvolution;

    // Use this for initialization
    void Start () {

        // VERTICES
        saveVertices = new Vector3[nbModule * nbModule];
        vertices = new Vector3[nbModule * nbModule];


        // VERTICES SIDES
        /*
        saveLeft = new Vector3[nbModule];
        saveRight = new Vector3[nbModule];
        saveTop = new Vector3[nbModule];
        saveBottom = new Vector3[nbModule];*/

        // DISTANCES
        foreheadDistances = new Vector3[nbModule];
        distances = new Vector3[nbModule * nbModule];
       

        // MODULATORS
        curveEvolution = new float[nbModule];
        evolutionValues = new float[nbModule];


        m = GetComponent<MeshFilter>().mesh;
        /*
        prevXcosineH = XcosineHeight;
        prevZcosineH = ZcosineHeight;*/
       // prevNbModule = nbModule;


        print("Side evolution Length " + sideEvolutionClip.length);

        if(sideEvolutionClip.length != nbModule)
        {

            for(int i = 0; i < sideEvolutionClip.length; i++)
            {

                sideEvolutionClip.RemoveKey(i);

            }


            for (int i = 0; i <nbModule; i++)
            {
                Keyframe k = new Keyframe();
                k.time = i / (float)nbModule;
                k.value = 0.5f;
                sideEvolutionClip.AddKey(k);

            }



        }




        Build();
        AssignMesh();

       


        for (int i = 0; i < saveVertices.Length; i++)
        {
            saveVertices[i] = m.vertices[i];
        }


        calculEvolution();
        calculDistances();
        calculForeHeadDistances();

        

    }

    void calculEvolution()
    {

        for (int i = 0; i < nbModule; i++)
        {
            curveEvolution[i] = Mathf.Sin((i / (float)nbModule) * Mathf.PI * evolutionFrequency) * limitEvolutionFrequency;
        }

        for (int i = 0; i < nbModule; i++)
        {
            evolutionValues[i] = Mathf.Abs( Mathf.Cos((i / (float)nbModule) * Mathf.PI * evolutionFactorFrequency) ) * evolutionFactor ;
            if (evolutionValues[i] < evolutionThreshold * evolutionFactor) evolutionValues[i] = 0;
        }


    }

    void calculForeHeadDistances()
    {
        Vector3[] foreheadVertices = returnBottomVertices();
        /*
        saveLeft = returnLeftVertices();
        saveRight = returnRightVertices();
        saveTop = returnTopVertices();
        saveBottom = returnBottomVertices();
        */



        print("calcul forhead");



        for(int i = 0; i < foreheadVertices.Length; i++)
        {
            //GameObject nD = GameObject.Instantiate(debug);            // DEBUG
            //nD.transform.position = left[i];
           // nD.transform.parent = transform.parent;
            foreheadDistances[i] = getForeheadCenter(foreheadVertices) - foreheadVertices[i];

        }


    }



    Vector3 getForeheadCenter(Vector3[] foreheadPoints)
    {

        Vector3 v = new Vector3(0, 0, 0);


        for (int i = 0; i < foreheadPoints.Length; i++)
        {
            v += foreheadPoints[i];

        }

        foreheadCenter = v/ foreheadPoints.Length;

        return foreheadCenter;

    }

    void resetLeft()
    {

        Vector3[] left = returnLeftVertices();
        Vector3[] right = returnRightVertices();
        Vector3[] top = returnTopVertices();
        Vector3[] bottom = returnBottomVertices();


        for (int i = 0; i < left.Length; i++)
        {

             left[i] -= new Vector3(0, curveEvolution[i] * resetLeftValue, 0);
             right[i] -= new Vector3(0, curveEvolution[i] * resetLeftValue, 0);
             top[i] -= new Vector3(0, curveEvolution[i] * resetTopValue, 0);
             bottom[i] -= new Vector3(0, curveEvolution[i] * resetBottomValue, 0);


        }

        setLeftVertices(left, new Vector3(0, 00, 0));
        setRightVertices(right, new Vector3(0, 0, 0));
        setTopVertices(top, new Vector3(0, 0, 0));
        setBottomVertices(bottom, new Vector3(0, 0, moveBottomValue));

    }

    void calculDistances()
    {

        for (int i = 0; i < vertices.Length; i++)
        {
            //GameObject nD = GameObject.Instantiate(debug);            // DEBUG
            //nD.transform.position = top[i];
            distances[i] = foreheadCenter- vertices[i];

        }


    }
    public void setForeheadVertices()
    {
        Vector3[] foreheadVertices = returnBottomVertices();

        for (int i = 0; i < foreheadVertices.Length; i++)
        {
            //foreheadVertices[i] = foreheadDistances[i] * foreheadDistance - foreheadCenter.transform.position;// GLITCH INVERSION

           // foreheadVertices[i] = foreheadCenter - foreheadDistances[i] * foreheadDistance ;

            foreheadVertices[i] +=  foreheadDistances[i] * foreheadDistance;
        }

        setBottomVertices(foreheadVertices,new Vector3(0,0,0));

    }


    public void Build()
    {
        vertices = new Vector3[nbModule * nbModule];
        faces = new int[(nbModule - 1) * (nbModule - 1) * 3 * 2];
        BuildTerrain();
    }




    void BuildTerrain()
    {
        vertices = new Vector3[nbModule * nbModule];
        int nbF = (nbModule - 1) * (nbModule - 1) * 3 * 2;
        if (rectoVerso) nbF *= 2;

        faces = new int[nbF];
        //if (saveVertices != null) vertices = saveVertices;

        Vector3 v = transform.position;

        float angle = Mathf.PI;

        float distanceAngle = 0;

        float doubleSineAngle = 0;
        float thirdSineAngle = 0;

        int selectCurrentAnimationCurveValue = -1;

        //ADD NOISE
        for (int i = 0; i < nbModule * nbModule; i++)
        {
            if (i % nbModule == 0)
            {

                thirdSineAngle += Mathf.PI * thirdFrequency / nbModule;

                angle = Mathf.PI;

                selectCurrentAnimationCurveValue++;
            }

            distanceAngle += Mathf.PI * 2 / (float)nbModule;

           
            float zVal = ((i - (i % nbModule)) / nbModule);

            float angle2 = Mathf.PI + zVal / nbModule * Mathf.PI*2 ;
            float angle3 = angle2 + Mathf.PI + (i% nbModule)  / nbModule * Mathf.PI*2 ;
            



            angle += Mathf.PI*2 / nbModule;
            doubleSineAngle += Mathf.PI * 4 / nbModule;


            float addYCosine = Mathf.Cos(angle) * XcosineHeight;
            float addYCosine2 = Mathf.Cos(angle2) * ZcosineHeight;
            float circleY = Mathf.Cos(angle3) * circleCosineHeight;
            float addDoubleSine = Mathf.Cos(doubleSineAngle) * doubleSineStrength;
            float addThirdSine = Mathf.Cos(thirdSineAngle) * thirdSineStrength;

            
            Keyframe ratioAnimationCurve = sideEvolutionClip.keys[selectCurrentAnimationCurveValue];

            float addEvolutionCurve =( evolutionValues[i % nbModule] * ratioAnimationCurve.value ) * evolutionFactor;


            float moduloYVal = 0;
            float moduloXVal = 0;

            if (moduloX < 1) moduloX = 1;
            if (moduloY < 1) moduloY = 1;


            if (i % moduloX == 0) moduloXVal = moduloXValue;
            if (i % moduloY == 0) moduloYVal = moduloYValue;


            float startX = -((nbModule-1) / 2) * xGap;
            float startZ = -((nbModule-1) / 2) * zGap;
          

            vertices[i] = 
                new Vector3(startX + v.x + (i % nbModule) * xGap + moduloXVal,
                v.y + addYCosine + addYCosine2 + circleY + moduloYVal + addDoubleSine + addThirdSine + addEvolutionCurve ,
                startZ + v.z + zVal * zGap);


            vertices[i] += distances[i] * ( ( Mathf.Sin(distanceAngle) * sinTwistDistance ) % modulateSin ) / divideSin;
            vertices[i] += distances[i] * ( ( Mathf.Cos(distanceAngle) * cosTwistDistance ) % modulateCos ) / divideCos;
            vertices[i] += distances[i] * ( ( Mathf.Tan(distanceAngle) * tanTwistDistance) % modulateTangent ) / divideTangent ;

            //vertices[i] += new Vector3(Mathf.Cos(distanceAngle) , 0, 0);
            //vertices[i] += new Vector3(0,noiseValues[i],0);

        }

        // BUILD FACES
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

                if (rectoVerso)
                {
                    faces[nbF/2 + a * 6] = j + i * nbModule + 1;
                    faces[nbF / 2 + a * 6 + 2] = j + i * nbModule;
                    faces[nbF / 2 + a * 6 + 1] = j + i * nbModule + nbModule;


                    if (grided)
                    {

                        faces[nbF / 2 + a * 6 + 5] = j + i * nbModule + 1;
                        faces[nbF / 2 + a * 6 + 3] = j + i * nbModule + nbModule;
                        faces[nbF / 2 + a * 6 + 4] = j + i * nbModule + nbModule + 1;

                    }
                    else
                    {

                        faces[nbF / 2 + a * 6 + 4] = j + i * nbModule + 1;
                        faces[nbF / 2 + a * 6 + 3] = j + i * nbModule + nbModule;
                        faces[nbF / 2 + a * 6 + 5] = j + i * nbModule + nbModule + 1;


                    }
                  
                }

                a++;
            }

        }




    }


    // Update is called once per frame
    void Update () {


        calculEvolution();

        BuildTerrain();


        resetLeft();


        setForeheadVertices();


        calculateUv();



        m.RecalculateBounds();
        m.RecalculateNormals();





       
        AssignMesh();


    


        

    }




    void calculateUv()
    {
        Vector2[] uvs = new Vector2[m.vertices.Length];
        
        for(int i = 0; i < uvs.Length; i++)
        {


            uvs[i] = new Vector2(m.vertices[i].x, m.vertices[i].y);


        }

        m.uv = uvs;


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



    public void setRightVertices(Vector3[] right, Vector3 pos)
    {

        int j = 0;
        for (int i = nbModule-1; i < nbModule * nbModule; i += nbModule)
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

    }



    public void setTopVertices(Vector3[] top, Vector3 pos)
    {

        int a = 0;

        for (int i = ((nbModule - 1) * nbModule); i < (nbModule * nbModule); i++)
        {
            vertices[i] = top[a];
            a++;

        }

    }



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

        //GetComponent<MeshCollider>().sharedMesh = m;

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
