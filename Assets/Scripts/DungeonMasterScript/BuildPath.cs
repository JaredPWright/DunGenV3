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
    public bool buildDirection = false;


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
            rotateHall = false;
            buildDirection = accessKeyY < (macroGridSize / 2);
            if(buildDirection)
                endAccessKeyY = MyOwnRandomizer.TwoNumberIntReturn(accessKeyY + 1, macroGridSize);
            else
                endAccessKeyY = MyOwnRandomizer.TwoNumberIntReturn(1.0f, accessKeyY);
        }
        else
        {
            rotateHall = true;
            buildDirection = accessKeyX < (macroGridSize / 2);
            if(buildDirection)
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
            if(buildVertical)
            {
                if(buildDirection)
                    {
                        accessKeyY++;
                        if(accessKeyY > macroGridSize)
                        {
                            break;
                        }
                    }
                else
                    {
                        accessKeyY--;
                        if(accessKeyY < 0)
                        {
                            break;
                        }
                    }
            }
            else
            {
                if(buildDirection)
                    {
                        accessKeyX++;
                        if(accessKeyX > macroGridSize)
                        {
                            break;
                        }
                    }
                else
                    {
                        accessKeyX--;
                        if(accessKeyX < 0)
                        {
                            break;
                        }
                    }
            }

            if(macroGridStorage.moduleDictionary.ContainsKey(macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position))
            {
                continue;
            }else if(roomOrHallModule % 2 == 0)
            {
                //Create a module, save its access key data for potential branching later
                GameObject tempModule = Instantiate(modulePrefabs.roomPrefabs[0], macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position, Quaternion.identity);
                tempModule.GetComponent<AccessKeyHolder>().xAccessKey = accessKeyX;
                tempModule.GetComponent<AccessKeyHolder>().yAccessKey = accessKeyY;
                tempModule.GetComponent<AccessKeyHolder>().phaseDesignation = "Trunk";

                macroGridStorage.moduleDictionary.Add(tempModule.transform.position, tempModule);
            }else
            {
                GameObject tempModule = Instantiate(modulePrefabs.HallPrefabs[0], macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position, Quaternion.identity);
                tempModule.GetComponent<AccessKeyHolder>().xAccessKey = accessKeyX;
                tempModule.GetComponent<AccessKeyHolder>().yAccessKey = accessKeyY;
                tempModule.GetComponent<AccessKeyHolder>().phaseDesignation = "Trunk";
                if(rotateHall)
                {
                    Vector3 rotationStation = new Vector3(0.0f, 0.0f, 90.0f);
                    tempModule.transform.Rotate(rotationStation, Space.Self);
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
                    if(buildVertical)
                    {
                        endPos += new Vector3(0.0f, setMacroGrid.gridIntervals, 0.0f);
                        accessKeyY++;
                    }else
                    {
                        endPos += new Vector3(setMacroGrid.gridIntervals, 0.0f, 0.0f);
                        accessKeyX++;
                    }
                    GameObject tempModule = Instantiate(modulePrefabs.roomPrefabs[0], endPos, Quaternion.identity);
                    macroGridStorage.moduleDictionary.Add(tempModule.transform.position, tempModule);
                    tempModule.GetComponent<AccessKeyHolder>().xAccessKey = accessKeyX;
                    tempModule.GetComponent<AccessKeyHolder>().yAccessKey = accessKeyY;
                    tempModule.GetComponent<AccessKeyHolder>().phaseDesignation = "Trunk";
                }
            }
            
        }while(continueBuild);
        
        Debug.Log("Calling Branch");
        branchDungeon.Branch();
    }
}