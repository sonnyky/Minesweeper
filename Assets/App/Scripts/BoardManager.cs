using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Defines and spawns the necessary cell groups
/// </summary>

public class BoardManager : MonoBehaviour {

    public Sprite[] m_NodeNumbers;

    public GameObject m_CellGroupPrefab;

    // There are 11 nodes in one line.
    public int m_NumberOfLines = 0;
    private int m_NodesInALine = 11; // Hardcode for now.

    private List<Node> m_AllNodes;

    Vector3 m_StartingPosition = new Vector3(0, 20, 0);
    float m_VerticalStep = 8f;
    float m_HorizontalStep;

	// Use this for initialization
	void Start () {

        m_AllNodes = new List<Node>();

        // Generate the line cells

		for(int i = 0; i < m_NumberOfLines; i++)
        {
            Vector3 instantiatePosition = m_StartingPosition;
            GameObject oneLine = Instantiate(m_CellGroupPrefab);
            oneLine.name = "CellLineGroup" + i;

            instantiatePosition.y -= (i * 2.5f);
            if (i%2 == 0)
            {
                instantiatePosition.x -= 4.25f;
            }
            oneLine.transform.position = instantiatePosition;


            int nodeIndex = 0;
            // Get all children nodes and assign ids
            foreach(Transform child in oneLine.transform)
            {
                Relations relation = child.gameObject.GetComponent<Relations>();
                relation.Initializations(m_NumberOfLines, m_NodesInALine);
                CellID thisNodeId = new CellID(i, nodeIndex);
                relation.m_Id = thisNodeId;

                nodeIndex++;
                child.gameObject.name = "Node_" + thisNodeId.hor + "_" + thisNodeId.ver;
                Node thisNode = child.gameObject.GetComponent<Node>();
                m_AllNodes.Add(thisNode);
            }
        }
        // should we get adjacents after spawning all the nodes? easier to get non existing nodes. Or just make sure the last line nodes
        // do not add more nodes to the adjacent list

        foreach(Node node in m_AllNodes)
        {
            Relations rel = node.gameObject.GetComponent<Relations>();
            // Find adjacents of the node
            rel.FindAdjacents();
            node.Init();
        }

        // Randomly assign node characteristics
        foreach (Node node in m_AllNodes)
        {
            node.SetEmptyOrNumber();
        }

    }


    public void SetCellId()
    {

    }

    public int GetNumberOfLines()
    {
        return m_NumberOfLines;
    }

    public int GetNumberOfNodesInALine()
    {
        return m_NodesInALine;
    }

    public void UpdateNodeInformationAt(Node node)
    {

    }

    public Node GetNodeWithId(CellID nodeId)
    {
        //Debug.Log("[GetNodeWithId] nodeId : " + nodeId.hor + ", " + nodeId.ver + ". m_AllNodes.Count : " + m_AllNodes.Count + ". index: " + ((m_NodesInALine * nodeId.hor) + nodeId.ver));
        return m_AllNodes[(m_NodesInALine * nodeId.hor) + nodeId.ver];
    }

    public Sprite GetSprite(int spriteCode)
    {
        return m_NodeNumbers[spriteCode];
    }
}
