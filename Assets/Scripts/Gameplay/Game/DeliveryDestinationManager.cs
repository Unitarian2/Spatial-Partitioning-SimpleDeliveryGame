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
        // Her bir binadan yalnýzca bir tane olacak þekilde filtrele
        buildingList = buildings.GroupBy(b => b.GetType()).Select(g => g.First()).ToList();
    }

    public DeliveryDestination GetDeliveryDestination()
    {
        // Liste içerisinden iki farklý bina tipi seç
        IBuilding buildingStart = GetRandomBuilding();
        IBuilding buildingEnd;

        do
        {
            buildingEnd = GetRandomBuilding();
        } while (buildingStart == buildingEnd); // Ýki tip farklý olmalý

        return new DeliveryDestination(buildingStart, buildingEnd);
    }

    private IBuilding GetRandomBuilding()
    {
        int randomIndex = UnityEngine.Random.Range(0, buildingList.Count);
        return buildingList[randomIndex];
    }

}
