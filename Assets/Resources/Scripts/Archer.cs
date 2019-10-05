using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour {

    GameObject archerSprite;
    Vector3 startPoint = new Vector3(-12, 4, 0);
    // Use this for initialization
    void Start () {
        archerSprite = Resources.Load<GameObject>("SpritesArchers/FantasyArcher_01");
        SpawnArcher();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnArcher()
    {
        Instantiate(archerSprite, startPoint, Quaternion.identity);
    }
}
