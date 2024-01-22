using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryDestinationManager
{
    private List<IBuilding> buildingList;

    public DeliveryDestinationManager(List<IBuilding> buildings)
    {
        // Her bir binadan yaln�zca bir tane olacak �ekilde filtrele
        buildingList = buildings.GroupBy(b => b.GetType()).Select(g => g.First()).ToList();
    }

    public DeliveryDestination GetDeliveryDestination()
    {
        // Liste i�erisinden iki farkl� bina tipi se�
        IBuilding buildingStart = GetRandomBuilding();
        IBuilding buildingEnd;

        do
        {
            buildingEnd = GetRandomBuilding();
        } while (buildingStart == buildingEnd); // �ki tip farkl� olmal�

        return new DeliveryDestination(buildingStart, buildingEnd);
    }

    private IBuilding GetRandomBuilding()
    {
        int randomIndex = UnityEngine.Random.Range(0, buildingList.Count);
        return buildingList[randomIndex];
    }

}
