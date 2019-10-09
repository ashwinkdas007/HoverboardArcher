using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    public LineRenderer line, line2;

    private Camera mainCamera;
    private Vector2 screenBounds;
    float vieportHalfHeight, vieportHalfWidth;

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
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        vieportHalfHeight = mainCamera.orthographicSize;
        vieportHalfWidth = mainCamera.aspect * vieportHalfHeight;
        xLength = vieportHalfWidth * 2;
        numOfVertices = Mathf.CeilToInt(xLength / vertexSpacing);
        terrainVertices = new Vector3[numOfVertices];
        //line2 = Instantiate<LineRenderer>(line);


        CalculateTerrainVertices(ref terrainVertices, (ProbabilityFunctions.DiceRoll(50)+1)*1000);
        SetTerrain(line);
        CalculateTerrainVertices(ref terrainVertices, offset);
        SetTerrain(line2);
        // SetCollider();
    }

    // Update is called once per frame
    void Update()
    {
        moveTerrainVertices(line);
        moveTerrainVertices(line2);
        repositionTerrain(line, line2);
        //timer += Time.deltaTime;

        //if (timer >= lineResetPeriod)
        //{
            //timer = 0.0f;
           // CalculateTerrainVertices(xLength); //, line.GetPosition(numOfVertices-1)
            //line.positionCount = 0;
            //SetLineRenderer();
        //}
    }

    void CalculateTerrainVertices(ref Vector3[] terrainVertices, float offsetModifier = 0.0f)
    {
        float x, y, currentAmplitude, currentWavelength;
        Vector3[] vertices = new Vector3[numOfVertices];

        offset += offsetModifier;

        for (int i = 0; i < numOfVertices; i++)
        {
            x =  (i * vertexSpacing  -xLength / 2); //
            y = 0;

            for (int oct = 0; oct < numOfOctaves; oct++)
            {
                currentAmplitude = baseAmplitude / Mathf.Pow(lacunarity, oct);
                currentWavelength = baseWavelength / Mathf.Pow(lacunarity, oct);

                y += currentAmplitude * (Mathf.PerlinNoise((x / currentWavelength) + offset + shift, seed + shift) - 0.5f);
            }
            x += ((terrainVertices.Length > 0 && terrainVertices != null) ? terrainVertices[terrainVertices.Length - 1].x : 0); //+ xLength / 3
            //y += ((terrainVertices.Length > 0 && terrainVertices != null) ? terrainVertices[terrainVertices.Length - 1].y : 0);
            vertices[i] = new Vector3(x , y, 0.0f);
        }
        terrainVertices = vertices;
    }

    void SetTerrain(LineRenderer l)
    {
        l.positionCount = numOfVertices;
        l.SetPositions(terrainVertices);
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

    void moveTerrainVertices(LineRenderer l)
    {
        Vector3[] vertices = new Vector3[numOfVertices];
        l.GetPositions(vertices);

        for (int i = 0; i < numOfVertices; i++)
            vertices[i].x -= rateOfVerticeShift * Time.deltaTime;

        l.SetPositions(vertices);
    }

    /*void loadTerrain()
    {
        LineRenderer c;
        for (int i = 0; i <= 1; i++)
        {
            c = gameObject.AddComponent<LineRenderer>();
            c.transform.SetParent(line.transform);
            c.transform.position = line.GetPosition();
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }*/

   void repositionTerrain(LineRenderer first, LineRenderer second)
    {
        float halfObjectWidth = xLength / 2;

        if (mainCamera.transform.position.x + screenBounds.x > second.GetPosition(numOfVertices - 1).x) //+ halfObjectWidth
        {
            //first.transform.SetAsLastSibling();
            //first.transform.position = new Vector3(second.transform.position.x + halfObjectWidth * 2, second.transform.position.y, second.transform.position.z);
            CalculateTerrainVertices(ref terrainVertices, offset);
            first.positionCount = 0;
            SetTerrain(first);
        }
        else if (mainCamera.transform.position.x - screenBounds.x < first.GetPosition(numOfVertices - 1).x)
        {
            //second.transform.SetAsFirstSibling();
            //second.transform.position = new Vector3(first.transform.position.x - halfObjectWidth * 2, first.transform.position.y, first.transform.position.z);
            CalculateTerrainVertices(ref terrainVertices, offset);
            second.positionCount = 0;
            SetTerrain(second);
        }
        
    }
}
