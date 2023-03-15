using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    public GameObject Bee;
    public int CellRowIndex;
    public int CellColumnIndex;
    private int cellValue;
    public TextMeshPro text;
    public bool isHexOpen;
    public bool flagged;
    public Sprite GhostBeeSprite;
    public Sprite BeeSprite;
    public Material HexOpen;

    private static bool hasGameBegun;
    public int CellValue
    {
        get
        {
            return cellValue;
        }
        set
        {
            if (value < -1)
            {
                cellValue = -1;
            } else if (value > 6)
            {
                cellValue = 6;
            }
            else cellValue = value;

                // if (value == -1)
            // {
            //     Bee.SetActive(true);
            // }
        }
    }
    public void OnMouseDown()
    {
        Debug.Log("count bee" + SetupScene.getCountBee());
        if (IsGhostBeeToggle.CheckGhostBeeToggle == true)
        {
            UpdateUI();
        }
        else
        {
            if (hasGameBegun == false)
            {
                SetupScene.InsertBees(CellRowIndex, CellColumnIndex);
                hasGameBegun = true;
            }
            if (flagged == false)
            {
                if (CellValue == -1)
                {
                    SetupScene.ShowAllBees();
                }
                else
                {
                    SetupScene.ShowCells(CellRowIndex, CellColumnIndex);
                }
            }
        }
    }
    public void CheckSurroundingCellsForBees()
    {
        if (cellValue == -1)
        {
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

                if (CellRowIndex % 2 == 1) 
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

                int a = CellColumnIndex + j;
                int b = CellRowIndex + i;
                
                if (b > -1 && b < SetupScene.rowsGrid && a > -1 && a < SetupScene.columnsGrid)
                {
                    if (SetupScene.checkBee(b,a))
                    {
                        CellValue++;
                    }
                }
            }
        }
    }

    public void UpdateUI()
    {
        if (IsGhostBeeToggle.CheckGhostBeeToggle == true)
        {
            if (flagged == false)
            {
                if (isHexOpen == false)
                {
                    flagged = true;
                    Bee.SetActive(true);
                    Bee.GetComponent<SpriteRenderer>().sprite = GhostBeeSprite;
                    GetComponent<Renderer>().material.color = Color.blue;
                }
            }
            else
            {
                flagged = false;
                Bee.SetActive(false);
                Bee.GetComponent<SpriteRenderer>().sprite = BeeSprite;
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
        else
        {
            if (flagged == false)
            {
                GetComponent<Renderer>().material = HexOpen;
        
                if (cellValue > 0)
                {
                    text.gameObject.SetActive(true);
                    text.text = cellValue.ToString();
                }
            }
        }
    }

    public void ShowBee()
    {
        Bee.SetActive(true);
        Bee.GetComponent<SpriteRenderer>().sprite = GhostBeeSprite;
        GetComponent<Renderer>().material.color = Color.red;
    }
}
