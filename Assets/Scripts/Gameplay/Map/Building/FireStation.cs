using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStation : MonoBehaviour, IBuilding
{
    public string BuildingName => "FireStation";
    public GameObject GameObject => this.GameObject;
    public Type Type => typeof(FireStation);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
