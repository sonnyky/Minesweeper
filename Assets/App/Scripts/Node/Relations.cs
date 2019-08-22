using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CellID
{
    public int hor;
    public int ver;
   
    public CellID(int v1, int v2) : this()
    {
        hor = v1;
        ver = v2;
    }
}

/// <summary>
/// Defines the relationships between adjacent nodes
/// </summary>
public class Relations : MonoBehaviour
{

    // There will be maximum eight adjacent nodes. Nodes at the corners will have less adjacent nodes.
    private List<CellID> m_AdjacentNodeIds;

    public CellID m_Id { get; set; }

    private int m_NumOfRows = 0;
    private int m_NumOfColumns = 0;

    public void Initializations(int rows, int columns)
    {
        m_AdjacentNodeIds = new List<CellID>();
        m_NumOfRows = rows;
        m_NumOfColumns = columns;
    }

    public void FindAdjacents()
    {
        // We know that we want to have 5 (odd) lines of adjacency.
        int lines = 5;
        int currentLineIndex = 0;
        int midRowIndex = (lines - 1) / 2;

        for(int i = m_Id.hor - 2; i <= m_Id.hor + 2; i++)
        {
            if(currentLineIndex <= midRowIndex)
            {

                for (int j = 0; j <= currentLineIndex; j++)
                {
                    int converted = 0;
                    if(j == 0)
                    {
                        converted = j / 2;
                    }
                    else
                    {
                        converted = ((j / 2) + 1);
                        if (j % 2 == 0) converted = -j / 2;
                        if (m_Id.hor % 2 == 0) converted *= -1;
                    }
                    CellID oneCell = new CellID(i, m_Id.ver + converted);

                    if (!CheckAdjacentValidity(oneCell)) continue;

                    m_AdjacentNodeIds.Add(oneCell);
                }
            }
            else
            {
                // After the middle row
                for (int j = lines - 1 - currentLineIndex; j >= 0; j--)
                {
                    int converted = 0;
                    if (j % 2 == 0)
                    {
                        converted = j / 2;
                    }
                    else
                    {
                        converted = ((j / 2) + 1);
                        if (j % 2 == 0) converted = -j / 2;
                        if (m_Id.hor % 2 == 0) converted *= -1;
                    }
                    CellID oneCell = new CellID(i, m_Id.ver + converted);

                    if (!CheckAdjacentValidity(oneCell)) continue;

                    m_AdjacentNodeIds.Add(oneCell);
                }
            }

            currentLineIndex++;
        }
    }

    bool CheckAdjacentValidity(CellID cellId)
    {
        if (cellId.Equals(m_Id)) return false;
        if (cellId.ver < 0 || cellId.hor < 0) return false;
        if (cellId.ver > m_NumOfColumns - 1 || cellId.hor > m_NumOfRows - 1) return false;
        return true;
    }

    public List<CellID> GetAdjacents()
    {
        return m_AdjacentNodeIds;
    }
}
