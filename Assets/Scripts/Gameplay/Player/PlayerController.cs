using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    //Dependencies
    Grid grid;
    Unit playerUnit;
    GameManager gameManager;

    Vector3 oldPos;
    Vector3 newPos;

    private IBuilding deliveryStartBuilding;
    private IBuilding deliveryEndBuilding;

    private PlayerDeliveryType playerDeliveryState = PlayerDeliveryType.Waiting;

    //For Debug Purposes
    private IBuilding deliveryStartBuildingType;
    private IBuilding deliveryEndBuildingType;
    private List<IBuilding> nearbyBuildingsStart = new();
    private List<IBuilding> nearbyBuildingsEnd = new();
    public List<GameObject> nearbyBuildingsStartObject = new();
    public List<GameObject> nearbyBuildingsEndObject = new();

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
        //deliveryStartBuildingType = newDelivery.BuildingStart;
        //deliveryEndBuildingType = newDelivery.BuildingEnd;
        Debug.LogWarning("Start : " + newDelivery.BuildingStart.BuildingName + " / "+ "End : "+ newDelivery.BuildingEnd.BuildingName);

        //nearbyBuildingsStart = grid.GetNearbyBuildings(gameObject.transform.position);
        //AddObjects(nearbyBuildingsStart, nearbyBuildingsStartObject);

        BuildingFinder buildingFinderStart = new(newDelivery.BuildingStart, grid.GetNearbyBuildings(gameObject.transform.position));
        IBuilding closestBuildingStart = buildingFinderStart.FindSameBuildingsByType().FindClosestBuilding(gameObject.transform.position);
        if(closestBuildingStart == null)
        {
            Debug.Log("Building Not Found, Searching All Buildings");
            BuildingFinder buildingFinderStartAll = new(newDelivery.BuildingStart, grid.GetAllBuildings());
            closestBuildingStart = buildingFinderStartAll.FindSameBuildingsByType().FindClosestBuilding(gameObject.transform.position);
        }

        //nearbyBuildingsEnd = grid.GetNearbyBuildings(closestBuildingStart.EntryPoint.transform.position);
        //AddObjects(nearbyBuildingsEnd, nearbyBuildingsEndObject);

        BuildingFinder buildingFinderEnd = new(newDelivery.BuildingEnd, grid.GetNearbyBuildings(closestBuildingStart.EntryPoint.transform.position));
        IBuilding closestBuildingEnd = buildingFinderEnd.FindSameBuildingsByType().FindClosestBuilding(closestBuildingStart.EntryPoint.transform.position);
        if (closestBuildingEnd == null)
        {
            Debug.Log("Building Not Found, Searching All Buildings");
            BuildingFinder buildingFinderEndAll = new(newDelivery.BuildingEnd, grid.GetAllBuildings());
            closestBuildingEnd = buildingFinderEndAll.FindSameBuildingsByType().FindClosestBuilding(closestBuildingStart.EntryPoint.transform.position);
        }


        if (closestBuildingStart != null && closestBuildingEnd != null)
        {
            //Yak�nda bina bulmu�uz
            Debug.Log("Route Found : "+ closestBuildingStart.GameObject.name + " to "+ closestBuildingEnd.GameObject.name);
            deliveryStartBuilding = closestBuildingStart;
            deliveryEndBuilding = closestBuildingEnd;
        }
        else
        {
            //Yak�nda bina bulamam���z. T�m haritada arayaca��z.
            Debug.Log("Building Not Found, Searching All Buildings");
            BuildingFinder buildingFinderStartAll = new(newDelivery.BuildingStart, grid.GetAllBuildings());
            deliveryStartBuilding = buildingFinderStartAll.FindSameBuildingsByType().FindClosestBuilding(gameObject.transform.position);

            BuildingFinder buildingFinderEndAll = new(newDelivery.BuildingEnd, grid.GetAllBuildings());
            deliveryEndBuilding = buildingFinderEndAll.FindSameBuildingsByType().FindClosestBuilding(deliveryStartBuilding.EntryPoint.transform.position);

            if (closestBuildingStart == null || closestBuildingEnd == null)
            {
                Debug.LogError("Invalid Route, Starting a New Delivery");
                GameManager.Instance.StartSingleDelivery(null);
            }
            else
            {
                Debug.Log("Route Found : " + closestBuildingStart.GameObject.name + " to " + closestBuildingEnd.GameObject.name);
            }
        }

        
    }

    private void AddObjects(List<IBuilding> buildings,List<GameObject> list)
    {
        foreach (IBuilding building in buildings)
        {
            list.Add(building.GameObject);
        }
        
    }
    
    public void StartToDeliver()
    {       
        SetDestination(deliveryStartBuilding);
        playerDeliveryState = PlayerDeliveryType.MovingToStart;
    }

    private void SetDestination(IBuilding buildingDestination)
    {
        agent.SetDestination(buildingDestination.EntryPoint.transform.position);
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
            //Debug.Log("Player reached: " + other.gameObject.name);
            if (other.gameObject.transform.parent.TryGetComponent<IBuilding>(out IBuilding building))
            {
                if (building == deliveryStartBuilding && playerDeliveryState == PlayerDeliveryType.MovingToStart)
                {
                    Debug.Log("Start Building'e ula��ld�!");
                    playerDeliveryState = PlayerDeliveryType.Waiting;
                    SetDestination(deliveryEndBuilding);
                    playerDeliveryState = PlayerDeliveryType.MovingToEnd;
                }
                else if (building == deliveryEndBuilding && playerDeliveryState == PlayerDeliveryType.MovingToEnd)
                {
                    Debug.Log("End Building'e ula��ld�!");
                    playerDeliveryState = PlayerDeliveryType.Waiting;
                    //agent.isStopped = true;
                    GameManager.Instance.StartSingleDelivery(deliveryEndBuilding.Type);
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

