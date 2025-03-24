using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtificialGrid : MonoBehaviour
{
    public static ArtificialGrid Instance;
    
    public GameObject cellPrefab;
    public Transform gridParent;
    public Transform imageParent;

    CellManager _cellManager;

    public GameObject[,] grid;
    public int gridRows;
    public int gridCols;
    List<CellGroup> cellGroups;

    public float xStart;
    public float yStart;

    //float gridSpacingX = 10f;
    //float gridSpacingY = 20f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
        cellGroups = new List<CellGroup>();
        _cellManager = GetComponent<CellManager>();
    }

    private void Start()
    {
        RectTransform gridRect = gridParent.GetComponent<RectTransform>();
        xStart = transform.position.x - (transform.position.x / 2);
        yStart = transform.position.y * 1.25f;
        grid = new GameObject[gridRows,gridCols];
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float original_xStart = xStart;
        for (int i = 0; i < gridRows; i++)
        {
            for (int j = 0; j < gridCols; j++)
            {
                grid[i,j] = Instantiate(cellPrefab, new Vector3(xStart, yStart, 0f), Quaternion.identity, gridParent);
                grid[i, j].name = i + " - " + j;
                xStart += grid[0, 0].GetComponent<RectTransform>().sizeDelta.x / 2;
            }
            xStart = original_xStart;
            yStart -= grid[0,0].GetComponent<RectTransform>().sizeDelta.y / 2;
        }
        _cellManager.SetFirstCell(grid[0, 0].GetComponent<GridCell>());
    }

    public void CreateCellGroup(List<Tuple<int,int>> coordinates, string name, GameObject go, Grid_Item item)
    {
        List<GridCell> cells = new List<GridCell>();
        Vector3 position = Vector3.zero;


        for (int i = 0; i < coordinates.Count; i++)
        {
            cells.Add(grid[coordinates[i].Item1, coordinates[i].Item2].GetComponent<GridCell>());
            position += cells[i].GetComponent<RectTransform>().position;
        }

        position /= 2;

        GameObject cellGO = Instantiate(go, position, Quaternion.identity, imageParent);

        CellGroup newCellGroup = new CellGroup(name, coordinates, cells, cellGO, item);

        cellGroups.Add(newCellGroup);
    } // Debug

    public void FillCell(Grid_Item item) // Debug
    {
        grid[0,0].GetComponent<GridCell>().FillCell(item);
    }

    public List<GameObject> CheckForCollisions(Vector2 startPos, Vector2 endPos)
    {
        List<GameObject> collisions = new List<GameObject>();
        RectTransform rect;
        bool isInsideX;
        bool isInsideY;
        foreach (GameObject cell in grid)
        {
            rect = cell.GetComponent<RectTransform>();

            isInsideX = rect.position.x >= Mathf.Min(startPos.x, endPos.x) && rect.position.x <= Math.Max(startPos.x, endPos.x);
            isInsideY = rect.position.y >= Mathf.Min(startPos.y, endPos.y) && rect.position.y <= Math.Max(startPos.y, endPos.y);

            if(isInsideX && isInsideY)
            {
                collisions.Add(cell);
                //Debug.Log("Collides with: " + cell.name);
            }
        }

        return collisions;
    }
}