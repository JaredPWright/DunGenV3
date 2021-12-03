using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMacroGrid : MonoBehaviour
{
    private MacroGridStorage macroGridStorage;
    private BuildPath buildPath;
    public float lengthWidth = 100.0f;
    public float gridIntervals = 10.0f;
    public Vector3 macroGridWaypointCoordinates = Vector3.zero;
    public GameObject waypointTemplate;

    public void BuildMacroGrid()
    {
        macroGridStorage = GetComponent<MacroGridStorage>();
        buildPath = GetComponent<BuildPath>();
        //Blank GameObject that acts as a template for instantiation.
        //It'll be destroyed at the end of generation. 
        
        waypointTemplate.transform.position = new Vector3(1000f, 1000f, 1000f);

        GameObject macroGridHolder = new GameObject();
        macroGridHolder.name = "MacroGridParent";
        macroGridHolder.transform.position = new Vector3(500f, 500f, 500f);

        //Creates rows and columns, adjusting the position by the given intervals each time.
        //Basically creates a big square.
        for(int i = 0; i < lengthWidth; i++)
        {
            //Create a parent for each row of waypoints. Name it appropriately. 
            GameObject rowFather = new GameObject();
            rowFather.name = "Row" + i.ToString();
            rowFather.transform.position = new Vector3(-1000f, -1000f, -1000f);
            rowFather.transform.SetParent(macroGridHolder.transform);

            for(int j = 0; j < lengthWidth; j++)
            {
                //Create a waypoint, set it to the proper position, parent it to the current rowFather
                GameObject tempMacroWaypoint = Instantiate(waypointTemplate, macroGridWaypointCoordinates, Quaternion.identity, rowFather.transform);
                tempMacroWaypoint.name = "Point" + (j+1).ToString() + "_Row" + (i+1).ToString();
                macroGridStorage.macroGridPoints[i,j] = tempMacroWaypoint;

                macroGridWaypointCoordinates.y += gridIntervals;
            }
            
            //Bump the x coordinates, reset the z.
            macroGridWaypointCoordinates.y = 0.0f;
            macroGridWaypointCoordinates.x += gridIntervals;
        }

        StartCoroutine(buildPath.Build());
    }
}