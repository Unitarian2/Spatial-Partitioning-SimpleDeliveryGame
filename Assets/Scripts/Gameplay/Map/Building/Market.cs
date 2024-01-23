using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour, IBuilding
{
    public string BuildingName => "Market";
    public GameObject GameObject => gameObject;
    public Type Type => typeof(Market);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
