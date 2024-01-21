using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapController : MonoBehaviour
{
    Grid grid;

    [SerializeField] private List<Unit> units;

    //Grid material
    private Material gridMaterial;
    //Grid mesh
    private Mesh gridMesh;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid();

        foreach (Unit unit in units)
        {
            unit.InitUnit(grid, unit.gameObject.transform.position);
        }
    }

    private void LateUpdate()
    {
        //Display the grid with lines
        if (gridMaterial == null)
        {
            gridMaterial = new Material(Shader.Find("Unlit/Color"));

            gridMaterial.color = Color.black;
        }

        if (grid == null)
        {
            return;
        }

        if (gridMesh == null)
        {
            gridMesh = InitGridMesh();
        }

        //Display the mesh
        Graphics.DrawMesh(gridMesh, Vector3.zero, Quaternion.identity, gridMaterial, 0, Camera.main, 0);

    }

    

    private Mesh InitGridMesh()
    {
        //Generate the vertices
        List<Vector3> lineVertices = new();

        float battlefieldWidth = Grid.NUM_CELLS * Grid.CELL_SIZE;

        Vector3 linePosX = Vector3.zero;
        Vector3 linePosY = Vector3.zero;

        for (int x = 0; x <= Grid.NUM_CELLS; x++)
        {
            lineVertices.Add(linePosX);
            lineVertices.Add(linePosX + Vector3.right * battlefieldWidth);

            lineVertices.Add(linePosY);
            lineVertices.Add(linePosY + Vector3.forward * battlefieldWidth);

            linePosX += Vector3.forward * Grid.CELL_SIZE;
            linePosY += Vector3.right * Grid.CELL_SIZE;
        }


        //Generate the indices
        List<int> indices = new();

        for (int i = 0; i < lineVertices.Count; i++)
        {
            indices.Add(i);
        }


        //Generate the mesh
        Mesh gridMesh = new();

        gridMesh.SetVertices(lineVertices);
        gridMesh.SetIndices(indices, MeshTopology.Lines, 0);


        return gridMesh;
    }
}
