using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetupScene : MonoBehaviour
{
    [SerializeField]private GameObject hexPrefab;

    public static int rowsGrid = 10;
    public static int columnsGrid = 20;
    private static int numberOfBees = 30;
    public Transform LayerCells;
    private static GameObject[,] hexGrid;
    
    // Start is called before the first frame update
    void Start()
    {
        hexGrid = new GameObject[rowsGrid, columnsGrid];
        CreateHexGrid();
    }

    private void CreateHexGrid()
    {
        float width = hexPrefab.GetComponent<MeshCollider>().bounds.size.x;
        float height = hexPrefab.GetComponent<MeshCollider>().bounds.size.z;
        float widthOffset = width / 2;
        float heightOffset = height / 4;
        for (int i = 0; i < rowsGrid; i++)
        {
            for (int j = 0; j < columnsGrid; j++)
            {
                hexGrid[i, j] = Instantiate(hexPrefab);
                hexGrid[i, j].transform.parent = LayerCells;
                hexGrid[i, j].GetComponent<Cell>().CellRowIndex = i;
                hexGrid[i, j].GetComponent<Cell>().CellColumnIndex = j;
                hexGrid[i, j].transform.position = new Vector3(j * width, 0,i * (height - heightOffset));

                if (i % 2 == 1)
                {
                    hexGrid[i, j].transform.position = new Vector3(j * width + widthOffset, 0,i * (height - heightOffset));
                }
            }
        }
    }

    public static int getCountBee()
    {
        return numberOfBees;
    }
    public static void InsertBees(int rowIndex, int columnIndex)
    {
        while (numberOfBees != 0)
        {
            int randomRowIndex = Random.Range(0, rowsGrid); 
            int randomColumnIndex = Random.Range(0, columnsGrid);

            if (hexGrid[randomRowIndex, randomColumnIndex].GetComponent<Cell>().CellValue != -1 && IsBeeNearToStart(rowIndex, columnIndex, randomRowIndex, randomColumnIndex) == false)
            {
                hexGrid[randomRowIndex, randomColumnIndex].GetComponent<Cell>().CellValue = -1;
                numberOfBees--;
            }
        }
    
        for (int i = 0; i < rowsGrid; i++)
        {
            for (int j = 0; j < columnsGrid; j++)
            {
               hexGrid[i,j].GetComponent<Cell>().CheckSurroundingCellsForBees();
            }
        }
    }

    private static bool IsBeeNearToStart(int startRow, int startCol, int beeRow, int beeCol)
    {
        if (startRow == beeRow && startCol == beeCol)
        {
            return true;
        }

        bool isAdjacentRow = beeRow > startRow + 2 || beeRow < startRow - 2 ? false : true;
        bool isAdjacentColumn = beeCol > startCol + 2 || beeCol < startCol - 2 ? false : true;

        if (isAdjacentRow == true && isAdjacentColumn == true)
        {
            return true;
        }
        return false;
    }

    public static bool checkBee(int rowIndex, int columnIndex)
    {
        if (hexGrid[rowIndex, columnIndex].GetComponent<Cell>().CellValue == -1)
        {
            return true;
        }

        return false;
    }

    public static void ShowCells(int rowIndex, int ColumnIndex)
    {
        Cell currentCell = hexGrid[rowIndex, ColumnIndex].GetComponent<Cell>();

        if (currentCell.CellValue == -1 || currentCell.isHexOpen == true)
        {
            return;
        }

        if (currentCell.CellValue > 0)
        {
            currentCell.UpdateUI();
            return;
        }
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                if (currentCell.CellRowIndex % 2 == 1) 
                {
                    if ((i == -1 && j == -1) || (i == 1 && j == -1))
                    {
                        continue;
                    }
                }
                else
                {
                    if ((i == -1 && j == 1) || (i == 1 && j == 1))
                    {
                        continue;
                    }
                }

                int a = ColumnIndex + j;
                int b = rowIndex + i;
                
                if (b > -1 && b < rowsGrid && a > -1 && a < columnsGrid)
                {
                    if (hexGrid[b,a].GetComponent<Cell>().flagged == false && checkBee(b, a) == false && hexGrid[b, a].GetComponent<Cell>().isHexOpen == false)
                    {
                        currentCell.isHexOpen = true;
                        currentCell.UpdateUI();
                        ShowCells(b,a);
                    }
                }
            }

            if (currentCell.CellValue == 0)
            {
                currentCell.UpdateUI();
            }
        }
    }

    public static void ShowAllBees()
    {
        for (int i = 0; i < rowsGrid; i++)
        {
            for (int j = 0; j < columnsGrid; j++)
            {
                if (checkBee(i, j))
                {
                    IsGhostBeeToggle.CheckGhostBeeToggle = false;
                    hexGrid[i, j].GetComponent<Cell>().flagged = false;
                    hexGrid[i, j].GetComponent<Cell>().UpdateUI();
                    hexGrid[i, j].GetComponent<Cell>().ShowBee();
                }
            }
        }
    }
}
