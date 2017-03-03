﻿using UnityEngine;
using System.Collections;

public class CheckRangeChouchin : MonoBehaviour {
    [SerializeField]
    ChouchinMove moveBlock;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {

        
        string layerName = LayerMask.LayerToName(col.gameObject.layer);
        if (layerName == "Player")
        {

            moveBlock.isInRange = true;
        }
    }
}
