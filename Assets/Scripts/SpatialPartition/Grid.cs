using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public const int NUM_CELLS = 20;

    public const int CELL_SIZE = 15;

    private Unit[,] cells = new Unit[NUM_CELLS, NUM_CELLS];
    private Unit[,] playerCells = new Unit[NUM_CELLS, NUM_CELLS];

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

    public void AddBuilding(Unit newUnit, bool isNewUnit = false)
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

    public void AddPlayer(Unit playerUnit)
    {
        //Player'�n hangi cell'de oldu�unu buluyoruz.
        Vector2Int cellPos = ConvertFromWorldToCell(playerUnit.transform.position);
        playerCells[cellPos.x, cellPos.y] = playerUnit;//Player'� ilgili cell'e at�yoruz.
        Debug.Log("PlayerCell : "+ cellPos.x+"/"+ cellPos.y);
    }

    public void CheckPlayerMovement(Unit playerUnit, Vector3 oldPos, Vector3 newPos)
    {
        //See what cell it was in before we assign the new position
        Vector2Int oldCellPos = ConvertFromWorldToCell(oldPos);

        //See which cell it's moving to
        Vector2Int newCellPos = ConvertFromWorldToCell(newPos);

        //If it didn't change cell, we are done
        if (oldCellPos.x == newCellPos.x && oldCellPos.y == newCellPos.y)
        {
            return;
        }

        //Remove it from the old cell
        playerCells[oldCellPos.x, oldCellPos.y] = null;

        //Add it back to the grid at its new cell
        AddPlayer(playerUnit);
    }

    public List<IBuilding> GetNearbyBuildings(Vector3 playerPos)
    {
        List<IBuilding> nearbyBuildingList = new();
        List<Vector2Int> nearbyCells = new();
        //�nce player'�n bulundu�u cell'i buluyoruz.
        Vector2Int playerCellPos = ConvertFromWorldToCell(playerPos);

        //Ard�ndan nearby h�creleri al�yoruz.
        Vector2Int currentCell = new Vector2Int(playerCellPos.x, playerCellPos.y); nearbyCells.Add(currentCell);
        Vector2Int nCell = new Vector2Int(playerCellPos.x, playerCellPos.y + 1); nearbyCells.Add(nCell);
        Vector2Int sCell = new Vector2Int(playerCellPos.x, playerCellPos.y - 1); nearbyCells.Add(sCell);
        Vector2Int wCell = new Vector2Int(playerCellPos.x - 1, playerCellPos.y); nearbyCells.Add(wCell);
        Vector2Int eCell = new Vector2Int(playerCellPos.x + 1, playerCellPos.y); nearbyCells.Add(eCell);
        Vector2Int nwCell = new Vector2Int(playerCellPos.x - 1, playerCellPos.y + 1); nearbyCells.Add(nwCell);
        Vector2Int neCell = new Vector2Int(playerCellPos.x + 1, playerCellPos.y + 1); nearbyCells.Add(neCell);
        Vector2Int swCell = new Vector2Int(playerCellPos.x - 1, playerCellPos.y - 1); nearbyCells.Add(swCell);
        Vector2Int seCell = new Vector2Int(playerCellPos.x + 1, playerCellPos.y - 1); nearbyCells.Add(seCell);

        //Nearby h�crelerin hepsinde d�n�p, i�erisinde bulunan t�m building'leri listemize ekliyoruz.
        foreach (Vector2Int dir in nearbyCells)
        {
            if (cells[dir.x,dir.y] != null)
            {
                //LinkedList i�erisinde bir bir ilerliyoruz ve her eleman� listeye ekliyoruz.
                var currentNode = cells[dir.x, dir.y];
                while ((currentNode != null))
                {
                    //nearbyBuildingList.Add(cells[dir.x, dir.y].gameObject.GetComponent<IBuilding>());
                    nearbyBuildingList.Add(currentNode.gameObject.GetComponent<IBuilding>());
                    currentNode = currentNode.next;
                }
            }
        }

        return nearbyBuildingList;

    }

    public List<IBuilding> GetAllBuildings()
    {
        List<IBuilding> allBuildingList = new();

        int width = cells.GetLength(0);
        int height = cells.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //LinkedList i�erisinde bir bir ilerliyoruz ve her eleman� listeye ekliyoruz.
                var currentNode = cells[x, y];
                while ((currentNode != null))
                {
                    allBuildingList.Add(cells[x, y].gameObject.GetComponent<IBuilding>());
                    currentNode = currentNode.next;
                }
                
            }
        }
        return allBuildingList;

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
