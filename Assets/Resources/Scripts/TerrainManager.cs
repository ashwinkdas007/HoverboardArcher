using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    public LineRenderer line;
    

    float xLength;
    public float vertexSpacing = 1f;

    public float baseAmplitude = 2;
    public float baseWavelength = 3;

    public float numOfOctaves = 3;
    public float lacunarity = 2;

    public int seed = 100;
    float shift = 0.51f;
    static float offset = 100000;

    public EdgeCollider2D coll;

    Vector3[] terrainVertices;
    int numOfVertices;

    public float rateOfVerticeShift = 1;
    float lineResetPeriod = 12.0f;
    float timer = 0.0f;

    // Use this for initialization
    void Start () {
        CalculateTerrainVertices((ProbabilityFunctions.DiceRoll(50)+1)*1000);
        SetLineRenderer();
       // SetCollider();
    }

    // Update is called once per frame
    void Update()
    {
        moveTerrainVertices();
        timer += Time.deltaTime;

        if (timer >= lineResetPeriod)
        {
            timer = 0.0f;
            line.positionCount = 0;
            CalculateTerrainVertices(40);
            SetLineRenderer();
        }
    }

    void CalculateTerrainVertices(float offsetModifier = 0.0f)
    {
        float x, y, currentAmplitude, currentWavelength, vieportHalfHeight, vieportHalfWidth;

        vieportHalfHeight = Camera.main.orthographicSize;
        vieportHalfWidth = Camera.main.aspect * vieportHalfHeight;

        xLength = vieportHalfWidth * 3;

        numOfVertices = Mathf.CeilToInt(xLength / vertexSpacing);
        //Debug.Log("vertexSpacing : " + vertexSpacing);
        terrainVertices = new Vector3[numOfVertices];

        offset += offsetModifier;


        for (int i = 0; i < numOfVertices; i++)
        {
            x = i * vertexSpacing - xLength / 2;
            y = 0;
            //y = Mathf.Sin(i * vertexSpacing / wavelength) * amplitude;
            for (int oct = 0; oct < numOfOctaves; oct++)
            {
                currentAmplitude = baseAmplitude / Mathf.Pow(lacunarity, oct);
                currentWavelength = baseWavelength / Mathf.Pow(lacunarity, oct);

                y += currentAmplitude * (Mathf.PerlinNoise((i * vertexSpacing / currentWavelength) + offset + shift, seed + shift) - 0.5f);
            }
            terrainVertices[i] = new Vector3(x + (vieportHalfWidth/2.1f), y, 0.0f);
            //line.SetPosition(i, new Vector3(x, y, 0.0f));
        }
    }

    void SetLineRenderer()
    {
        line.positionCount = numOfVertices;
        line.SetPositions(terrainVertices);
    }

    //void SetCollider()
    //{
    //    Vector2[] vertex2s = new Vector2[numOfVertices];
    //    for (int i = 0; i < numOfVertices; i++)
    //    {
    //        vertex2s[i] = terrainVertices[i];
    //    }
    //    coll.points = vertex2s;
    //}

    void moveTerrainVertices()
    {
        Vector3[] vertices = new Vector3[numOfVertices];
        line.GetPositions(vertices);

        for(int i = 0; i < numOfVertices; i++)
            vertices[i].x -= rateOfVerticeShift * Time.deltaTime;

        line.SetPositions(vertices);
    }
}
