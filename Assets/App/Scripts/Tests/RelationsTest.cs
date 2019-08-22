using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationsTest : MonoBehaviour
{
    public Relations relations;

    [SerializeField]
    public CellID target;

    // Start is called before the first frame update
    void Start()
    {
        relations.m_Id = target;
        relations.Initializations(15, 11);
        printAdjacentNodes(target);
    }
    
    void printAdjacentNodes(CellID target)
    {
        List<CellID> adjacents = new List<CellID>();
        relations.FindAdjacents();
        adjacents = relations.GetAdjacents();

        for(int i=0; i<adjacents.Count; i++)
        {
            Debug.Log("Adjacent cell : " + "[" + adjacents[i].hor + ", " + adjacents[i].ver + "]");
        }

    }

}
