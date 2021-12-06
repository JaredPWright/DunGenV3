using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacroGridStorage : MonoBehaviour
{
    public SetMacroGrid setMacroGrid;
    public GameObject[,] macroGridPoints = new GameObject[101, 101];
    public Dictionary<Vector3, GameObject> moduleDictionary = new Dictionary<Vector3, GameObject>();
}