using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop : MonoBehaviour, IBuilding
{
    public string BuildingName => "Workshop";
    public GameObject GameObject => this.GameObject;
    public Type Type => typeof(Workshop);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
