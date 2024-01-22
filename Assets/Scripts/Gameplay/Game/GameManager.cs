using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<IBuilding> buildingList = new();
    [SerializeField] private Transform buildingParentObject;

    DeliveryDestinationManager deliveryDestinationManager;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform childTransform in buildingParentObject)
        {
            if (childTransform.gameObject.TryGetComponent<IBuilding>(out IBuilding building))
            {
                buildingList.Add(building);
            }
        }

        InitDeliverySystem();
        SetNewDelivery();
        
    }

    private void InitDeliverySystem()
    {
        deliveryDestinationManager = new DeliveryDestinationManager(buildingList);
    }

    public void SetNewDelivery()
    {
        DeliveryDestination newDelivery = deliveryDestinationManager.GetDeliveryDestination();
    }
}
