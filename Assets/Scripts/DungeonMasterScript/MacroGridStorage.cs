using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacroGridStorage : MonoBehaviour
{
    public GameObject[,] macroGridPoints = new GameObject[100, 100];
    public Dictionary<Vector3, GameObject> moduleDictionary = new Dictionary<Vector3, GameObject>();
}