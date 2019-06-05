using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private bool isBomb;
    private Buff GetBuff()
    {
        int buffsLength = System.Enum.GetValues(typeof(Buff)).Length;
        int index = Random.Range(0, buffsLength);
        return (Buff)index;
    }
}
