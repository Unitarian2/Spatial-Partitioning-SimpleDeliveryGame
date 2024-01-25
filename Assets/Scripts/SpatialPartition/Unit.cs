using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Grid grid;

    public string unitGameObjectName;

    public string prevName;
    public string nextName;

    [System.NonSerialized] public Unit prev;
    [System.NonSerialized] public Unit next;

    public void InitUnit(Grid grid, Vector3 startPos)
    {
        unitGameObjectName = gameObject.name;

        this.grid = grid;
        transform.position = startPos;//Bu �imdilik duplicate olarak position belirliyor ama ilerde unit'leri rastgele �ekilde spawn edersek bu laz�m olacak.

        prev = null; next = null;

        grid.AddBuilding(this, isNewUnit: true);


    }

    public void InitPlayerUnit(Grid grid, Vector3 startPos)
    {
        unitGameObjectName = gameObject.name;

        this.grid = grid;
        transform.position = startPos;//Bu �imdilik duplicate olarak position belirliyor ama ilerde unit'leri rastgele �ekilde spawn edersek bu laz�m olacak.

        prev = null; next = null;

        grid.AddPlayer(this);


    }

}
