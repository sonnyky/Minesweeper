using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    private BaseManager m_BaseManager;
    private int m_NumOfNodesInALine;
    private int m_NumberOfLines;


    public void InitLevel()
    {
        m_BaseManager = GetComponent<BaseManager>();
    }

    public void DecideNodeCharacteristics(Node node)
    {
      
    }

}
