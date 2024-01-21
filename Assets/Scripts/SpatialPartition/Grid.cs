using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public const int NUM_CELLS = 20;

    public const int CELL_SIZE = 30;

    private Unit[,] cells = new Unit[NUM_CELLS, NUM_CELLS];

    //How many units do we have on the grid, which should be faster than to iterate through all cells and count them
    public int unitCount { get; private set; }

    public Grid()
    {
        //Clear the grid
        for (int x = 0; x < NUM_CELLS; x++)
        {
            for (int y = 0; y < NUM_CELLS; y++)
            {
                cells[x, y] = null;
            }
        }
    }

    public void Add(Unit newUnit, bool isNewUnit = false)
    {
        Vector2Int cellPos = ConvertFromWorldToCell(newUnit.transform.position);

        //Add the unit to the front of list for the cell it's in
        newUnit.prev = null;
        newUnit.next = cells[cellPos.x, cellPos.y];

        //Associate the cell with this unit
        cells[cellPos.x, cellPos.y] = newUnit;

        //If there already was a unit in this cell, it should point to the new unit
        if (newUnit.next != null)
        {
            Unit nextUnit = newUnit.next;

            nextUnit.prev = newUnit;
            nextUnit.prevName = newUnit.gameObject.name;
        }

        if (isNewUnit)
        {
            unitCount += 1;
        }

        if (newUnit.prev != null) newUnit.prevName = newUnit.prev.gameObject.name;
        if (newUnit.next != null) newUnit.nextName = newUnit.next.gameObject.name;
    }

    public Vector2Int ConvertFromWorldToCell(Vector3 pos)
    {
        //Dividing coordinate by cell size converts from world space to cell space
        //Casting to int converts from cell space to cell index
        //int cellX = (int)(pos.x / CELL_SIZE);
        //int cellY = (int)(pos.z / CELL_SIZE); //z instead of y because y is up in Unity's coordinate system

        //Casting to int in C# doesnt work in same way as in C++ so we have to use FloorToInt instead
        //It works like this if cell size is 2:
        //pos.x is 1.8, then cellX will be 1.8/2 = 0.9 -> 0
        //pos.x is 2.1, then cellX will be 2.1/2 = 1.05 -> 1
        int cellX = Mathf.FloorToInt(pos.x / CELL_SIZE);
        int cellY = Mathf.FloorToInt(pos.z / CELL_SIZE); //z instead of y because y is up in Unity's coordinate system

        Vector2Int cellPos = new Vector2Int(cellX, cellY);

        return cellPos;
    }

    
}
