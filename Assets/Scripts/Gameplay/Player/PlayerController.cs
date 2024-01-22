using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    Grid grid;
    Unit playerUnit;

    Vector3 oldPos;
    Vector3 newPos;

    private IBuilding deliveryStartBuilding;
    private IBuilding deliveryEndBuilding;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void InitPlayerMapInfo(Grid grid, Unit playerUnit)
    {
        this.grid = grid;
        this.playerUnit = playerUnit;
    }

    public void SetNewDelivery(DeliveryDestination newDelivery)
    {
        deliveryStartBuilding = newDelivery.BuildingStart;
        deliveryEndBuilding = newDelivery.BuildingEnd;

        BuildingFinder buildingFinder = new(deliveryStartBuilding, grid.GetNearbyBuildings(gameObject.transform.position));
        IBuilding closestBuilding = buildingFinder.FindSameBuildingsByType().FindClosestBuilding(gameObject.transform.position);

        if(closestBuilding != null)
        {
            //Yakýnda bina bulmuþuz
        }
        else
        {
            //Yakýnda bina bulamamýþýz. Tüm haritada arayacaðýz.
        }

        
    }
    
    public void StartToDeliver()
    {
        
        SetDestination(deliveryStartBuilding);
    }

    private void SetDestination(IBuilding buildingDestination)
    {
        agent.SetDestination(buildingDestination.GameObject.transform.position);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse clicked");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                agent.SetDestination(hit.point);
            }
        }



        oldPos = newPos;
        newPos = transform.position;
        grid.CheckPlayerMovement(playerUnit,oldPos,newPos);


    }
}

