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

    // ---Resources---
    GameObject inLineTwoCells;
    GameObject inLineThreeCells;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
        cellGroups = new List<CellGroup>();
        _cellManager = GetComponent<CellManager>();
        LoadResources();
    }

    private void Start()
    {
        RectTransform gridRect = gridParent.GetComponent<RectTransform>();
        xStart = transform.position.x - (transform.position.x / 2);
        yStart = transform.position.y * 1.25f;
        grid = new GameObject[gridRows,gridCols];
        GenerateGrid();
    }

    void LoadResources()
    {
        inLineTwoCells = Resources.Load<GameObject>("Prefabs/MultiCells/InLineTwoCellsPrefab");
        inLineThreeCells = Resources.Load<GameObject>("Prefabs/MultiCells/InLineThreeCellsPrefab");
        if (inLineTwoCells == null || inLineTwoCells == null)
        {
            Debug.Log("Null!");
        }
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

    public void CreateCellGroupDebug(List<Tuple<int,int>> coordinates, string name, GameObject go, Grid_Item item)
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

    public void FillCell(Grid_Item item, Grid_Item item2) // Debug
    {
        grid[0,0].GetComponent<GridCell>().FillCell(item);
        grid[2,1].GetComponent<GridCell>().FillCell(item2);
    }

    public void CreateCellGroup(int occupiedCells, bool inLine, Grid_Item item)
    {
        if (!CheckIfEnoughSpace(occupiedCells))
        {
            Debug.Log("Not enough cells!");
            return;
        }

        if (inLine)
        {
            List<Tuple<int,int>> coordinates = CreateInLine(occupiedCells);
            if (coordinates == null)
            {
                Debug.Log("Enough cells but not in the correct order!");
                return;
            }
            GameObject prefab = null;

            switch (occupiedCells)
            {
                case 2:
                    prefab = inLineTwoCells;
                    break;
                case 3:
                    prefab = inLineThreeCells;
                    break;
                default:
                    break;
            }

            InstantiateCellGroup(coordinates, item, prefab);
        }

        CreateSquare();
    }

    void InstantiateCellGroup(List<Tuple<int, int>> coordinates, Grid_Item item, GameObject prefab)
    {
        List<GridCell> cells = new List<GridCell>();
        Vector3 position = Vector3.zero;


        for (int i = 0; i < coordinates.Count; i++)
        {
            cells.Add(grid[coordinates[i].Item1, coordinates[i].Item2].GetComponent<GridCell>());
            position += cells[i].GetComponent<RectTransform>().position;
        }

        position /= 2;

        GameObject cellGO = Instantiate(prefab, position, Quaternion.identity, imageParent);
        cellGO.name = item.name;

        CellGroup newCellGroup = new CellGroup(item.name, coordinates, cells, cellGO, item);

        cellGroups.Add(newCellGroup);
    }

    bool CheckIfEnoughSpace(int occupiedCells)
    {
        int emptyCell = 0;
        GridCell cell;

        foreach (GameObject cellGO in grid)
        {
            cell = cellGO.GetComponent<GridCell>();
            if (!cell.occupied)
            {
                emptyCell++;
            }
        }

        if (emptyCell < occupiedCells)
        {
            return false;
        }

        return true;
    }

    List<Tuple<int, int>> CreateInLine(int occupiedCells)
    {

        List<Tuple<int,int>> coordinates = new List<Tuple<int,int>>();

        // Controllo orizzontale (righe)
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            int freeCellsInARow = 0;

            for (int col = 0; col < grid.GetLength(1); col++)
            {
                GridCell cell = grid[row, col].GetComponent<GridCell>();

                if (!cell.occupied)
                {
                    freeCellsInARow++;  // Incrementa se la cella � libera
                    coordinates.Add(Tuple.Create(row,col));
                }
                else
                {
                    freeCellsInARow = 0;  // Resetta se la cella � occupata
                    coordinates.Clear();
                }

                // Se abbiamo trovato abbastanza celle libere consecutive
                if (freeCellsInARow >= occupiedCells)
                {
                    return coordinates;  // Troviamo un possibile spazio
                }
            }
        }

        // Controllo verticale (colonne)
        for (int col = 0; col < grid.GetLength(1); col++)
        {
            int freeCellsInAColumn = 0;

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                GridCell cell = grid[row, col].GetComponent<GridCell>();

                if (!cell.occupied)
                {
                    freeCellsInAColumn++;  // Incrementa se la cella � libera
                    coordinates.Add(Tuple.Create(row, col));
                }
                else
                {
                    freeCellsInAColumn = 0;  // Resetta se la cella � occupata
                    coordinates.Clear();
                }

                // Se abbiamo trovato abbastanza celle libere consecutive
                if (freeCellsInAColumn >= occupiedCells)
                {
                    return coordinates;  // Troviamo un possibile spazio
                }
            }
        }

        return null;  // Se non abbiamo trovato spazio, ritorna false
    }

    bool CreateSquare()
    {
        return true;
    }

    public GameObject InstantiateSelectionGO()
    {
        GameObject go = Instantiate(_cellManager.selectionGO_prefab, GameObject.FindWithTag("InventoryGrid").transform);
        return go;
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