using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Relations))]
public class Node : MonoBehaviour
{
    private bool m_IsBomb;
    private bool m_IsEmpty;
    private bool m_IsNumber;

    private List<CellID> m_Adjacents;
    private List<CellID> m_BombNodes; // If this cell is a number cell, it also holds information on the cells that holds a bomb.

    private Relations m_Relations;

    private void Start()
    {
        m_Relations = GetComponent<Relations>();
    }

    private Buff GetBuff()
    {
        int buffsLength = System.Enum.GetValues(typeof(Buff)).Length;
        int index = Random.Range(0, buffsLength);
        return (Buff)index;
    }

    public void SetNodeNumber()
    {

    }

}
