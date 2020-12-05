using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Relations))]
public class Node : MonoBehaviour
{
    public bool m_IsBomb = false;
    public bool m_IsEmpty = false;
    public bool m_IsNumber = false;
    public bool m_IsSet = false;

    private List<CellID> m_AdjacentIds;
    private List<Node> m_AdjacentNodes;
    private List<CellID> m_BombNodes; // If this cell is a number cell, it also holds information on the cells that holds a bomb.
    private List<CellID> m_NumberNodes; // If this cell is a bomb, hold information on the number cells it contributes to.

    private SpriteRenderer m_NodeSpriteRenderer;

    public int m_NodeNumber = -1;

    public int m_NumOfSurroundingBombs = -1;

    private Relations m_Relations;

    private BoardManager m_BoardManager;

    public void Init()
    {
        m_BoardManager = FindObjectOfType<BoardManager>();

        m_NodeSpriteRenderer = transform.Find("Node").gameObject.GetComponent<SpriteRenderer>();

        m_AdjacentNodes = new List<Node>();
        m_Relations = GetComponent<Relations>();
        m_AdjacentIds = m_Relations.GetAdjacents();
        //Debug.Log("for node : " + gameObject.name + " there are adjacents : " + m_AdjacentIds.Count);
        foreach(CellID id in m_AdjacentIds)
        {
            try
            {
                Node adjacentNode = m_BoardManager.GetNodeWithId(id);
                //Debug.Log("Node ID : " + gameObject.name + " and adjacent node is : " + adjacentNode.gameObject.name);
                m_AdjacentNodes.Add(adjacentNode);
            }
            catch
            {
                Debug.Log("Exception happened when checking for : " + gameObject.name + " and adjacent id : " + id.hor + ", " + id.ver);
            }
        }
    }

    private Buff GetBuff()
    {
        int buffsLength = System.Enum.GetValues(typeof(Buff)).Length;
        int index = Random.Range(0, buffsLength);
        return (Buff)index;
    }

    /// <summary>
    /// Set the node image, whether a number, or empty
    /// </summary>
    public void SetNodeDisplay()
    {

    }

    /// <summary>
    /// Called after setting node as a number.
    /// </summary>
    public void SetAdjacents(List<Node> unsetAdjacents, int numOfBombsRemainingToSet)
    {
        // These adjacents have the possibility to become a bomb, or empty. (TODO : also add the possibility of being another number)
        int numOfUnsetAdjacents = unsetAdjacents.Count;

        // Set the adjacent node to empty or bomb
        foreach(Node node in unsetAdjacents) {
            node.m_IsSet = true;

            if (numOfBombsRemainingToSet > 0)
            {
                // If the number of unset adjacents is more than bombs to set, randomize. Otherwise, set as bomb
                if (numOfUnsetAdjacents > numOfBombsRemainingToSet)
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        node.SetEmpty();
                    }
                    else
                    {
                        node.SetBomb();
                        numOfBombsRemainingToSet--;
                    }
                }
                else
                {
                    node.SetBomb();
                    numOfBombsRemainingToSet--;
                }
            }
            else
            {
                node.SetEmpty();
            }
            numOfUnsetAdjacents--;
        }
    }

    public bool GetNodeSetStatus()
    {
        return m_IsSet;
    }

    public bool GetNodeBombStatus()
    {
        return m_IsBomb;
    }

    public bool GetNodeEmptyStatus()
    {
        return m_IsEmpty;
    }

    public bool GetNodeNumberStatus()
    {
        return m_IsNumber;
    }

    public int GetNumberOfSurroundingBombs()
    {
        return m_NumOfSurroundingBombs;
    }

    public List<CellID> GetListOfCellsWithBombs()
    {
        return m_BombNodes;
    }

    public void SetEmptyOrNumber()
    {
        if (m_IsSet) return;
        m_IsSet = true;

        int random = Random.Range(0, 2);
        switch (random)
        {
            case 0:
                //Set to empty
                SetEmpty();
                break;
            case 1:
                // Set to number;
                SetNumber();
                break;
            default:
                // Set to empty
                SetEmpty();
                break;
        } 
    }

    void SetEmpty()
    {
        m_IsEmpty = true;
        m_NodeSpriteRenderer.sprite = m_BoardManager.GetSprite(0);
    }

    void SetBomb()
    {
        m_IsBomb = true;
        m_NodeSpriteRenderer.sprite = m_BoardManager.GetSprite(12);
    }

    void SetNumber()
    {
        m_IsNumber = true;

        Debug.Log("Setting node : " + gameObject.name + " into a number");

        // check how many unset adjacents are there, and their characteristics
        int unsetNode = 0;
        int numberNode = 0;
        int bombNode = 0;
        int emptyNode = 0;

        Debug.Log("Node : " + gameObject.name + " has " + m_AdjacentNodes.Count + " adjacent nodes");

        List<Node> unsetAdjacents = new List<Node>();
        foreach(Node node in m_AdjacentNodes)
        {
            if (!node.m_IsSet)
            {
                unsetNode++;
                unsetAdjacents.Add(node);
            }
            if (node.m_IsBomb) bombNode++;
            if (node.m_IsNumber) numberNode++;
            if (node.m_IsEmpty) emptyNode++;
        }

        if(unsetNode + numberNode + bombNode + emptyNode != m_AdjacentNodes.Count)
        {
            Debug.LogError("The node numbers don't match when setting node number for node : "  + gameObject.name + ". Where adjacent count = " + m_AdjacentNodes.Count + " while total of unset nodes are = " + (unsetNode + numberNode + bombNode + emptyNode));
        }

        int maxNumberCanBeSetTo = bombNode + unsetNode;
        int minNumberCanBeSetTo = bombNode > 0 ? bombNode : 1;

        int thisNodeNumber = Random.Range(minNumberCanBeSetTo, maxNumberCanBeSetTo + 1);
        m_NodeNumber = thisNodeNumber;
        m_NodeSpriteRenderer.sprite = m_BoardManager.GetSprite(thisNodeNumber);
        SetAdjacents(unsetAdjacents, thisNodeNumber - bombNode);
    }

}
