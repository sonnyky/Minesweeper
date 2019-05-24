using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CellID
{
    public int ver;
    public int hor;
}

/// <summary>
/// Defines the relationships between adjacent nodes
/// </summary>
public class Relations : MonoBehaviour
{

    // There will be maximum eight adjacent nodes. Nodes at the corners will have less adjacent nodes.
    private List<CellID> m_AdjacentNodeIds;

    public CellID m_Id { get; set; }

    public void FindAdjacents()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
