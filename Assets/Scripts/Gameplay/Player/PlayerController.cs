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

    private IBuilding deliveryStartBuildingType;
    private IBuilding deliveryEndBuildingType;

    private IBuilding deliveryStartBuilding;
    private IBuilding deliveryEndBuilding;

    private PlayerDeliveryType playerDeliveryState = PlayerDeliveryType.Waiting;

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
        deliveryStartBuildingType = newDelivery.BuildingStart;
        deliveryEndBuildingType = newDelivery.BuildingEnd;
       

        BuildingFinder buildingFinderStart = new(newDelivery.BuildingStart, grid.GetNearbyBuildings(gameObject.transform.position));
        IBuilding closestBuildingStart = buildingFinderStart.FindSameBuildingsByType().FindClosestBuilding(gameObject.transform.position);

        BuildingFinder buildingFinderEnd = new(newDelivery.BuildingEnd, grid.GetNearbyBuildings(closestBuildingStart.GameObject.transform.position));
        IBuilding closestBuildingEnd = buildingFinderEnd.FindSameBuildingsByType().FindClosestBuilding(closestBuildingStart.GameObject.transform.position);

        if (closestBuildingStart != null && closestBuildingEnd != null)
        {
            //Yakýnda bina bulmuþuz
            Debug.Log("Route Found : "+ closestBuildingStart.GameObject.name + " to "+ closestBuildingEnd.GameObject.name);
            deliveryStartBuilding = closestBuildingStart;
            deliveryEndBuilding = closestBuildingEnd;
        }
        else
        {
            //Yakýnda bina bulamamýþýz. Tüm haritada arayacaðýz.
            Debug.Log("Building Not Found");
        }

        
    }
    
    public void StartToDeliver()
    {       
        SetDestination(deliveryStartBuilding);
        playerDeliveryState = PlayerDeliveryType.MovingToStart;
    }

    private void SetDestination(IBuilding buildingDestination)
    {
        agent.SetDestination(buildingDestination.GameObject.transform.position);
    }

    

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log("Mouse clicked");
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if(Physics.Raycast(ray,out hit))
        //    {
        //        agent.SetDestination(hit.point);
        //    }
        //}



        oldPos = newPos;
        newPos = transform.position;
        grid.CheckPlayerMovement(playerUnit,oldPos,newPos);


    }

    void OnTriggerEnter(Collider other)
    {
        if(playerDeliveryState != PlayerDeliveryType.Waiting)
        {
            Debug.Log("Player reached: " + other.gameObject.name);
            if (other.gameObject.TryGetComponent<IBuilding>(out IBuilding building))
            {
                if (building == deliveryStartBuilding && playerDeliveryState == PlayerDeliveryType.MovingToStart)
                {
                    Debug.Log("Start Building'e ulaþýldý!");
                    playerDeliveryState = PlayerDeliveryType.Waiting;
                    SetDestination(deliveryEndBuilding);
                    playerDeliveryState = PlayerDeliveryType.MovingToEnd;
                }
                else if (building == deliveryEndBuilding && playerDeliveryState == PlayerDeliveryType.MovingToEnd)
                {
                    Debug.Log("End Building'e ulaþýldý!");
                    playerDeliveryState = PlayerDeliveryType.Waiting;
                    //agent.isStopped = true;
                }
            }
        }
        
        
    }
}

public enum PlayerDeliveryType
{
    Waiting,
    MovingToStart,
    MovingToEnd
}

