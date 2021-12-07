using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DunGenDataTypes;

public class LocalGridSpawner_Hall : MonoBehaviour
{
     private LocalGridStorage localGridStorage;
    public float lengthWidth = 10.0f;
    public float gridIntervals = 1.0f;
    public Vector3 macroGridWaypointCoordinates;
    public GameObject waypointTemplate;
    public bool amRotated = false;
    public LocalGridDataType[,] localGrid = new LocalGridDataType[10,10];

    public void BuildLocalGrid()
    {
        localGridStorage = GameObject.Find("DungeonMaster").GetComponent<LocalGridStorage>();

        //Set the starting position relative to the module, store a copy of it as a default. 
        macroGridWaypointCoordinates = transform.position;
        macroGridWaypointCoordinates -= new Vector3(4.5f, 4.5f, 0.0f);
        Vector3 defaultVal = macroGridWaypointCoordinates;

        //Blank GameObject that acts as a template for instantiation.
        //It'll be destroyed at the end of generation. 
        waypointTemplate.transform.position = new Vector3(1000f, 1000f, 1000f);

        GameObject macroGridHolder = new GameObject();
        macroGridHolder.name = "LocalGridParent";
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
                GameObject tempLocalWaypoint = Instantiate(waypointTemplate, macroGridWaypointCoordinates, Quaternion.identity, rowFather.transform);
                tempLocalWaypoint.name = "Point" + (j+1).ToString() + "_Row" + (i+1).ToString();
                if(amRotated)
                {
                    Debug.Log("I am a rotated hall and have made my local waypoints turn accordingly");
                    if(j <=2 || j >= 7)
                    {
                        localGrid[i,j] = new LocalGridDataType(tempLocalWaypoint, false);   
                    }else
                    {
                        localGrid[i,j] = new LocalGridDataType(tempLocalWaypoint, true);
                    }
                }else
                {
                    if(i <= 2 || i >= 7)
                    {localGrid[i,j] = new LocalGridDataType(tempLocalWaypoint, false);}
                    else
                    {localGrid[i,j] = new LocalGridDataType(tempLocalWaypoint, true);}
                }

                macroGridWaypointCoordinates.y += gridIntervals;
            }
            
            //Bump the x coordinates, reset the z.
            macroGridWaypointCoordinates.y = defaultVal.y;
            macroGridWaypointCoordinates.x += gridIntervals;
        }
    }
}
