using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hospital : MonoBehaviour, IBuilding
{
    public string BuildingName => "Hospital";
    public GameObject GameObject => gameObject;
    public Type Type => typeof(Hospital);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
