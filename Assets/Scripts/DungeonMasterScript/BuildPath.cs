using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPath : MonoBehaviour
{
    private ModulePrefabs modulePrefabs;
    private MacroGridStorage macroGridStorage;
    private SetMacroGrid setMacroGrid;
    private bool continueBuild = true;
    private bool rotateHall = false;

    int roomOrHallModule = 1;

    public IEnumerator Build()
    {
        modulePrefabs = GetComponent<ModulePrefabs>();
        macroGridStorage = GetComponent<MacroGridStorage>();
        setMacroGrid = GetComponent<SetMacroGrid>();

        float macroGridSize = setMacroGrid.lengthWidth;

        //Declare the random start access points externally so that I can acess and count from them going forward.
        int accessKeyX = MyOwnRandomizer.TwoNumberIntReturn(1.0f, macroGridSize - 1);
        yield return new WaitForSeconds(5.0f);
        int accessKeyY = MyOwnRandomizer.TwoNumberIntReturn(1.0f, macroGridSize - 1);

        Debug.Log(macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position);

        yield return new WaitForSeconds(5.0f);
        //Do the same thing for the end access points, but retrieve the Vector3 as well for later path completion verification.
        int endAccessKeyX = MyOwnRandomizer.TwoNumberIntReturn(accessKeyX + 1, macroGridSize);
        yield return new WaitForSeconds(5.0f);
        int endAccessKeyY = MyOwnRandomizer.TwoNumberIntReturn(accessKeyX + 1, macroGridSize);
        Vector3 endPos = macroGridStorage.macroGridPoints[endAccessKeyX, endAccessKeyY].transform.position;

        Debug.Log(endPos);

        Instantiate(modulePrefabs.roomPrefabs[0], macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position, Quaternion.identity);

        do
        {
            #region MoveChecking
            Vector3 closePos;

            Vector3[] surroundingWaypoints = new Vector3[4];
            for(int i = 0; i < 4; i++)
            {
                if(i == 0)
                    surroundingWaypoints[0] = macroGridStorage.macroGridPoints[(accessKeyX + 1), accessKeyY].transform.position;
                else if(i==1)
                    surroundingWaypoints[1] = macroGridStorage.macroGridPoints[accessKeyX, (accessKeyY + 1)].transform.position;
                else if(i==2)
                    surroundingWaypoints[2] = macroGridStorage.macroGridPoints[accessKeyX, (accessKeyY - 1)].transform.position;
                else
                    surroundingWaypoints[3] = macroGridStorage.macroGridPoints[(accessKeyX - 1), accessKeyY].transform.position;
            }

            closePos = surroundingWaypoints[0];

            for(int i = 1; i < 3; i++)
            {
                if(Vector3.Distance(closePos, endPos) > Vector3.Distance(surroundingWaypoints[i], endPos))
                {
                    closePos = surroundingWaypoints[i];
                }
            }

           if(closePos == surroundingWaypoints[0])
           {
               accessKeyX++;
               rotateHall = true;
           }  
           else if(closePos == surroundingWaypoints[1])
               accessKeyY++;
           else if(closePos == surroundingWaypoints[2])
               accessKeyY--;
           else
           {
               accessKeyX--;
               rotateHall = true;
           }
            #endregion

            if(roomOrHallModule % 2 == 0)
            {
                GameObject tempModule = Instantiate(modulePrefabs.roomPrefabs[0], macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position, Quaternion.identity);
                if(rotateHall)
                    rotateHall = false;

                macroGridStorage.moduleDictionary.Add(tempModule.transform.position, tempModule);
            }else
            {
                GameObject tempModule = Instantiate(modulePrefabs.HallPrefabs[0], macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position, Quaternion.identity);
                if(rotateHall)
                {
                    Vector3 rotationStation = new Vector3(0.0f, 0.0f, 90.0f);
                    tempModule.transform.Rotate(rotationStation, Space.Self);
                    rotateHall = false;
                }
                macroGridStorage.moduleDictionary.Add(tempModule.transform.position, tempModule);
            }

            roomOrHallModule++;
            
            bool endReached = macroGridStorage.moduleDictionary.ContainsKey(endPos);
            
            if(endReached)
            {
                continueBuild = false;
                if(macroGridStorage.moduleDictionary[endPos].gameObject.CompareTag("Hall"))
                {
                    Destroy(macroGridStorage.moduleDictionary[endPos].gameObject);
                    macroGridStorage.moduleDictionary.Remove(endPos);
                    GameObject tempModule = Instantiate(modulePrefabs.roomPrefabs[0], endPos, Quaternion.identity);
                }
            }
            
        }while(continueBuild);
        Debug.Log(roomOrHallModule.ToString());
    }
}