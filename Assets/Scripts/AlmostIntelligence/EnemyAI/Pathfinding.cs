using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    void Pathfind()
    {
        //     Vector3 closePos;

        //     Vector3[] surroundingWaypoints = new Vector3[4];
        //     for(int i = 0; i < 4; i++)
        //     {
        //         if(i == 0)
        //             surroundingWaypoints[0] = macroGridStorage.macroGridPoints[(accessKeyX + 1), accessKeyY].transform.position;
        //         else if(i==1)
        //             surroundingWaypoints[1] = macroGridStorage.macroGridPoints[accessKeyX, (accessKeyY + 1)].transform.position;
        //         else if(i==2)
        //             surroundingWaypoints[2] = macroGridStorage.macroGridPoints[accessKeyX, (accessKeyY - 1)].transform.position;
        //         else
        //             surroundingWaypoints[3] = macroGridStorage.macroGridPoints[(accessKeyX - 1), accessKeyY].transform.position;
        //     }

        //     closePos = surroundingWaypoints[0];

        //     for(int i = 1; i < 3; i++)
        //     {
        //         if(Vector3.Distance(closePos, endPos) > Vector3.Distance(surroundingWaypoints[i], endPos))
        //         {
        //             closePos = surroundingWaypoints[i];
        //         }
        //     }

        //    if(closePos == surroundingWaypoints[0])
        //    {
        //        accessKeyX++;
        //        rotateHall = true;
        //    }  
        //    else if(closePos == surroundingWaypoints[1])
        //        accessKeyY++;
        //    else if(closePos == surroundingWaypoints[2])
        //        accessKeyY--;
        //    else
        //    {
        //        accessKeyX--;
        //        rotateHall = true;
        //    }
    }
}
