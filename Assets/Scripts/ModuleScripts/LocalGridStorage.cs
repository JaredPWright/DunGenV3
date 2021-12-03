using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DunGenDataTypes;

public class LocalGridStorage : MonoBehaviour
{
    public LocalGridDataType[,] localGridStorage = new LocalGridDataType[100, 100];

    public void AddToLocalGrid(int masterGridXKey, int masterGridYKey, LocalGridDataType addWaypoint)
    {
        localGridStorage[masterGridXKey, masterGridYKey] = addWaypoint;
    }
}
