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
    public bool m_Reserved = false;
    public bool m_ToSetAsNumber = false;
    private CellID m_Reserver = new CellID(-1, -1);

    private List<CellID> m_AdjacentIds;
    private List<Node> m_AdjacentNodes;
    private List<CellID> m_BombNodes; // If this cell is a number cell, it also holds information on the cells that holds a bomb.
    private List<CellID> m_NumberNodes; // If this cell is a bomb, hold information on the number cells it contributes to.

    private SpriteRenderer m_NodeSpriteRenderer;

    public int m_NodeNumber = -1;

    public int m_NumOfSurroundingBombs = 0;

    private Relations m_Relations;

    private BoardManager m_BoardManager;

    private List<Node> m_UnsetAdjacents;

    public void Init()
    {
        m_BoardManager = FindObjectOfType<BoardManager>();

        m_NodeSpriteRenderer = transform.Find("Node").gameObject.GetComponent<SpriteRenderer>();

        m_AdjacentNodes = new List<Node>();
        m_UnsetAdjacents = new List<Node>();
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

    public CellID GetNodeId()
    {
        return m_Relations.m_Id;
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

    public void PreventOtherNodesToSetNode(CellID _reserver)
    {
        m_Reserved = true;
        m_Reserver = _reserver;
    }
    private void ReserveAdjacentNodes(List<Node> _nodesToReserve)
    {
        foreach (Node node in _nodesToReserve)
        {
            node.PreventOtherNodesToSetNode(GetNodeId());
        }
    }

    /// <summary>
    /// Called after setting node as a number.
    /// </summary>
    public void SetAdjacents(List<Node> unsetAdjacents, int numOfBombsRemainingToSet, CellID _cellId)
    {
        // These adjacents have the possibility to become a number, bomb, or empty.
        int numOfUnsetAdjacents = unsetAdjacents.Count;
        Debug.Log("unsetAdjacents: " + unsetAdjacents.Count + " and num of bombs to set: " + numOfBombsRemainingToSet);
        // Set the adjacent node to empty or bomb
        foreach(Node node in unsetAdjacents) {
            Debug.Log(node.GetNodeId().hor + ", " + node.GetNodeId().ver );
            if (numOfBombsRemainingToSet > 0)
            {
                Debug.Log("bombs to set");
                // If the number of unset adjacents is more than bombs to set, randomize. Otherwise, set as bomb
                if (numOfUnsetAdjacents > numOfBombsRemainingToSet)
                {
                    Debug.Log("more adjacents");
                    int rand = Random.Range(0, 3);
                    if (rand == 0)
                    {
                        Debug.Log("as empty");
                        numOfUnsetAdjacents--;
                        node.SetEmpty(_cellId);

                        if (numOfUnsetAdjacents == numOfBombsRemainingToSet) // Set remaining tiles to bombs
                        {
                            foreach (Node bombNode in m_AdjacentNodes)
                            {
                                if (!bombNode.m_IsSet && numOfBombsRemainingToSet > 0)
                                {
                                    numOfUnsetAdjacents--;
                                    bombNode.SetBomb(_cellId);
                                    m_NumOfSurroundingBombs++;
                                    numOfBombsRemainingToSet--;
                                }
                            }
                        }
                    }
                    else if (rand == 1)
                    {
                        Debug.Log("as number");
                        // Before setting as number, update own unset tiles, so the adjacent node cannot use common tile if number of tiles become insufficient for bombs
                        numOfUnsetAdjacents--;
                        node.m_ToSetAsNumber = true;

                        foreach (Node bombNode in m_AdjacentNodes)
                        {
                            if (!bombNode.m_IsSet && !bombNode.m_ToSetAsNumber && numOfBombsRemainingToSet > 0)
                            {
                                numOfUnsetAdjacents--;
                                bombNode.SetBomb(_cellId);
                                m_NumOfSurroundingBombs++;
                                numOfBombsRemainingToSet--;
                            }
                        }
                        node.SetNumber(_cellId);
                    }
                    else
                    {
                        Debug.Log("as bomb");
                        numOfUnsetAdjacents--;
                        node.SetBomb(_cellId);
                        m_NumOfSurroundingBombs++;
                        numOfBombsRemainingToSet--;

                        if (numOfUnsetAdjacents == numOfBombsRemainingToSet) // Set remaining tiles to bombs
                        {
                            foreach (Node bombNode in m_AdjacentNodes)
                            {
                                if (!bombNode.m_IsSet && numOfBombsRemainingToSet > 0)
                                {
                                    numOfUnsetAdjacents--;
                                    bombNode.SetBomb(_cellId);
                                    m_NumOfSurroundingBombs++;
                                    numOfBombsRemainingToSet--;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("num of adjacents is equal to unset bombs");
                    numOfUnsetAdjacents--;
                    node.SetBomb(_cellId);
                    numOfBombsRemainingToSet--;
                    m_NumOfSurroundingBombs++;
                }
            }
            else
            {
                Debug.Log("No more bombs to set");
                numOfUnsetAdjacents--;
                node.SetEmpty(_cellId);
            }
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

    public int GetNodeNumber()
    {
        return m_NodeNumber;
    }
    public int GetNumberOfSurroundingBombs()
    {
        return m_NumOfSurroundingBombs;
    }

    public List<CellID> GetListOfCellsWithBombs()
    {
        return m_BombNodes;
    }

    public void SetEmptyOrNumber(CellID _cellId)
    {
        if (m_IsSet) return;
        //m_IsSet = true;

        int random = Random.Range(0, 2);
        switch (random)
        {
            case 0:
                //Set to empty
                SetEmpty(_cellId);
                break;
            case 1:
                // Set to number;
                SetNumber(_cellId);
                break;
            default:
                // Set to empty
                SetEmpty(_cellId);
                break;
        } 
    }

    void SetEmpty(CellID _reserver)
    {
        if (m_IsSet) return;
        Debug.Log("Setting " + GetNodeId().hor + ", " + GetNodeId().ver + " as empty");
        if (m_Reserver.hor != _reserver.hor && m_Reserver.ver != _reserver.ver && m_Reserved) return;
        m_IsEmpty = true;
        m_IsSet = true;

        m_NodeSpriteRenderer.sprite = m_BoardManager.GetSprite(0);
    }

    void SetBomb(CellID _reserver)
    {
        Debug.Log("Setting " + GetNodeId().hor + ", " + GetNodeId().ver + " as bomb");
        if (m_IsSet) return;
        if (m_Reserver.hor != _reserver.hor && m_Reserver.ver != _reserver.ver && m_Reserved) return;
        if (!CanAddBombAroundNumber()) return;
        m_IsBomb = true;
        m_IsSet = true;

        m_NodeSpriteRenderer.sprite = m_BoardManager.GetSprite(12);
    }

    public bool CanAddBombAroundNumber()
    {
        bool success = true;
        foreach (Node node in m_AdjacentNodes)
        {
            if(node.GetNodeNumberStatus() && node.GetNumberOfSurroundingBombs() + 1 > node.GetNodeNumber())
            {
                success = false;
            }
        }
        return success;
    }

    void SetNumber(CellID _reserver)
    {
        if (m_IsSet) return;
        if (m_Reserver.hor != _reserver.hor && m_Reserver.ver != _reserver.ver && m_Reserved) return;
        m_IsNumber = true;
        m_IsSet = true;


        //Debug.Log("Setting node : " + gameObject.name + " into a number");

        // check how many unset adjacents are there, and their characteristics
        int unsetNode = 0;
        int numberNode = 0;
        int bombNode = 0;
        int emptyNode = 0;

        //Debug.Log("Node : " + gameObject.name + " has " + m_AdjacentNodes.Count + " adjacent nodes");

        List<Node> unsetAdjacents = new List<Node>();
        foreach(Node node in m_AdjacentNodes)
        {
            CellID thisNode = node.GetNodeId();
            //Debug.Log(thisNode.hor + ", " + thisNode.ver + " and rserved: " + node.m_Reserved);

            if (!node.m_IsSet && !node.m_Reserved)
            {
                unsetNode++;
                unsetAdjacents.Add(node);
            }
            if (node.m_IsBomb) bombNode++;
            if (node.m_IsNumber) numberNode++;
            if (node.m_IsEmpty) emptyNode++;
        }

        ReserveAdjacentNodes(unsetAdjacents);

        m_NumOfSurroundingBombs = bombNode;

        int maxNumberCanBeSetTo = bombNode + unsetNode;
        int minNumberCanBeSetTo = bombNode > 0 ? bombNode : 1;

        int thisNodeNumber = Random.Range(minNumberCanBeSetTo, maxNumberCanBeSetTo + 1);
        m_NodeNumber = thisNodeNumber;
        m_NodeSpriteRenderer.sprite = m_BoardManager.GetSprite(thisNodeNumber);
        //if (unsetNode + numberNode + bombNode + emptyNode != m_AdjacentNodes.Count)
        //{
        //    Debug.LogError("node : " + gameObject.name + ". Where adjacent count = " + m_AdjacentNodes.Count + " while total of set/unset nodes are = " + (unsetNode + numberNode + bombNode + emptyNode));
        //    Debug.LogError("unset node: " + unsetNode);
        //    Debug.LogError("bomb node: " + bombNode);
        //    Debug.LogError("empty node: " + emptyNode);
        //    Debug.LogError("number node: " + numberNode);
        //    Debug.LogError("number set to: " + m_NodeNumber);
        //    m_NodeSpriteRenderer.sprite = m_BoardManager.GetSprite(11);
        //}
        Debug.Log("thisnodenumber: " + thisNodeNumber + " and bombNode: " + bombNode);
        SetAdjacents(unsetAdjacents, thisNodeNumber - bombNode, GetNodeId());
    }

}
