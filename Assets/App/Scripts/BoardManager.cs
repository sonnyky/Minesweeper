using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines and spawns the necessary cell groups
/// </summary>

public class BoardManager : MonoBehaviour {

    public GameObject m_CellGroupPrefab;
    public int m_NumberOfLines = 0;

    Vector3 m_StartingPosition;
    float m_VerticalStep = 8f;
    float m_HorizontalStep;

	// Use this for initialization
	void Start () {

        // Generate the line cells
        m_StartingPosition = new Vector3(0, 0, 0);

		for(int i = 0; i < m_NumberOfLines; i++)
        {
            GameObject oneLine = Instantiate(m_CellGroupPrefab);
            oneLine.name = "CellLineGroup" + i;
            m_StartingPosition = oneLine.transform.position;
            
            if(i%2 != 0)
            {
                m_StartingPosition.x += 4.25f;
                m_StartingPosition.y += 20f - (i * 2.5f);
            }
            else
            {
                m_StartingPosition.y += 20f - ((i/2) * 5f);
            }
            oneLine.transform.position = m_StartingPosition;

        }
	}
	

    public void SetCellId()
    {

    }

}
