using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPath : MonoBehaviour
{
    #region ScriptReferences
    private ModulePrefabs modulePrefabs;
    private MacroGridStorage macroGridStorage;
    private SetMacroGrid setMacroGrid;
    private BranchDungeon branchDungeon;
    #endregion

    #region PrivateBools_ContinueBuild_RotateHall
    private bool continueBuild = true;
    private bool rotateHall = false;
    #endregion

    private int roomOrHallModule = 1;

    public bool buildVertical = false;


    public IEnumerator Build()
    {
        #region GetScripts
        modulePrefabs = GetComponent<ModulePrefabs>();
        macroGridStorage = GetComponent<MacroGridStorage>();
        setMacroGrid = GetComponent<SetMacroGrid>();
        branchDungeon = GetComponent<BranchDungeon>();
        #endregion

        float macroGridSize = setMacroGrid.lengthWidth;

        //Declare the random start access points externally so that I can acess and count from them going forward.
        int accessKeyX = MyOwnRandomizer.TwoNumberIntReturn(1.0f, macroGridSize - 1);
        int accessKeyY = MyOwnRandomizer.TwoNumberIntReturn(1.0f, macroGridSize - 1);
        int endAccessKeyX;
        int endAccessKeyY;

        Debug.Log(macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position);

        yield return new WaitForSeconds(1.0f);

        buildVertical = MyOwnRandomizer.TwoNumberRandomizer(0.0f, 100.0f);

        if(buildVertical)
        {
            endAccessKeyX = accessKeyX;
            if(accessKeyY < (macroGridSize / 2))
                endAccessKeyY = MyOwnRandomizer.TwoNumberIntReturn(accessKeyY + 1, macroGridSize);
            else
                endAccessKeyY = MyOwnRandomizer.TwoNumberIntReturn(1.0f, accessKeyY);
        }
        else
        {
            if(accessKeyX < (macroGridSize / 2))
                endAccessKeyX = MyOwnRandomizer.TwoNumberIntReturn(accessKeyX + 1, macroGridSize);
            else
                endAccessKeyX = MyOwnRandomizer.TwoNumberIntReturn(1.0f, accessKeyX);
            endAccessKeyY = accessKeyY;
        }

        Vector3 endPos = macroGridStorage.macroGridPoints[endAccessKeyX, endAccessKeyY].transform.position;

        Debug.Log(endPos);

        GameObject startRoom = Instantiate(modulePrefabs.roomPrefabs[0], macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position, Quaternion.identity);
        //Get and store access keys
        startRoom.GetComponent<AccessKeyHolder>().xAccessKey = accessKeyX;
        startRoom.GetComponent<AccessKeyHolder>().yAccessKey = accessKeyY;

        //Begin build operations
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
                //Create a module, save its access key data for potential branching later
                GameObject tempModule = Instantiate(modulePrefabs.roomPrefabs[0], macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position, Quaternion.identity);
                tempModule.GetComponent<AccessKeyHolder>().xAccessKey = accessKeyX;
                tempModule.GetComponent<AccessKeyHolder>().yAccessKey = accessKeyY;
                if(rotateHall)
                    rotateHall = false;

                macroGridStorage.moduleDictionary.Add(tempModule.transform.position, tempModule);
            }else
            {
                GameObject tempModule = Instantiate(modulePrefabs.HallPrefabs[0], macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position, Quaternion.identity);
                tempModule.GetComponent<AccessKeyHolder>().xAccessKey = accessKeyX;
                tempModule.GetComponent<AccessKeyHolder>().yAccessKey = accessKeyY;
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
        
        Debug.Log("Calling Branch");
        branchDungeon.Branch();
    }
}