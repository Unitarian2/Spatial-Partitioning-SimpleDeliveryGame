using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryDestinationManager
{
    private List<IBuilding> uniqueBuildingList;

    public DeliveryDestinationManager(List<IBuilding> buildings)
    {
        // Her bir binadan yaln�zca bir tane olacak �ekilde filtrele
        uniqueBuildingList = buildings.GroupBy(b => b.GetType()).Select(g => g.First()).ToList();
    }

    public DeliveryDestination GetDeliveryDestination(Type typeToAvoid)
    {
        // Liste i�erisinden iki farkl� bina tipi se�
        IBuilding buildingStart;
        if (typeToAvoid != null)
        {
            do
            {
                buildingStart = GetRandomBuilding();
            } while (buildingStart.Type == typeToAvoid);
        }
        else
        {
            buildingStart = GetRandomBuilding();
        }
         
        
        IBuilding buildingEnd;
        
        do
        {
            buildingEnd = GetRandomBuilding();
        } while (buildingStart.Type == buildingEnd.Type); // �ki tip farkl� olmal�

        return new DeliveryDestination(buildingStart, buildingEnd);
    }

    private IBuilding GetRandomBuilding()
    {
        int randomIndex = UnityEngine.Random.Range(0, uniqueBuildingList.Count);
        return uniqueBuildingList[randomIndex];
    }

}
