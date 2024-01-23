using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class School : MonoBehaviour, IBuilding
{
    public string BuildingName => "School";
    public GameObject GameObject => gameObject;
    public Type Type => typeof(School);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
