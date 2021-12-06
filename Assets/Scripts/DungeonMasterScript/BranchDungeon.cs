using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchDungeon : MonoBehaviour
{
    #region ScriptReferences
    private BuildPath buildPath;
    private MacroGridStorage macroGridStorage;
    private ModulePrefabs modulePrefabs;
    private SetMacroGrid setMacroGrid;
    #endregion

    #region PrivateInts_AccessKeys_BranchesGenerated
    private int branchesGenerated = 0;
    private int accessKeyX;
    private int accessKeyY;
    #endregion

    #region PrivateBools_RotateHall_ContinueBuild
    private bool rotateHall = false;
    private bool continueBuild = true;
    #endregion
    
    private Dictionary<Vector3, GameObject> localModuleDictionary = new Dictionary<Vector3, GameObject>();

    private Vector3 endOfBranchPosition;

    public int numberOfBranches = 2;
    public float lengthOfBranchesMin = 2.0f;
    public float lengthOfBranchesMax = 3.0f;

    public void Branch()
    {
        #region GetScripts
        buildPath = GetComponent<BuildPath>();
        macroGridStorage = GetComponent<MacroGridStorage>();
        modulePrefabs = GetComponent<ModulePrefabs>();
        setMacroGrid = GetComponent<SetMacroGrid>();
        #endregion

        Debug.Log("In Branch- about to enter generation phase");

        //Make sure that the user wants branches
        if(numberOfBranches > 0)
        {
            do
            {
                foreach(KeyValuePair<Vector3, GameObject> kvp in macroGridStorage.moduleDictionary)
                {
                    if(kvp.Value.CompareTag("Room"))
                    {
                        //This should make the selection of which rooms are branched off non-sequential, hopefully. 
                        if(MyOwnRandomizer.TwoNumberRandomizer(0.0f, 100.0f))
                        {
                            accessKeyX = kvp.Value.GetComponent<AccessKeyHolder>().xAccessKey;
                            accessKeyY = kvp.Value.GetComponent<AccessKeyHolder>().yAccessKey;

                            endOfBranchPosition = kvp.Value.transform.position;

                            if(buildPath.buildVertical)
                            {
                                if(MyOwnRandomizer.TwoNumberRandomizer(0.0f, 100.0f))
                                {
                                    endOfBranchPosition.x += MyOwnRandomizer.TwoNumberIntReturn(lengthOfBranchesMin, lengthOfBranchesMax) * setMacroGrid.gridIntervals;
                                }else
                                    endOfBranchPosition.x -= MyOwnRandomizer.TwoNumberIntReturn(lengthOfBranchesMin, lengthOfBranchesMax) * setMacroGrid.gridIntervals;
                                
                                rotateHall = true;
                            }else
                            {
                                if(MyOwnRandomizer.TwoNumberRandomizer(0.0f, 100.0f))
                                    endOfBranchPosition.y += MyOwnRandomizer.TwoNumberIntReturn(lengthOfBranchesMin, lengthOfBranchesMax) * setMacroGrid.gridIntervals;
                                else
                                    endOfBranchPosition.y -= MyOwnRandomizer.TwoNumberIntReturn(lengthOfBranchesMin, lengthOfBranchesMax) * setMacroGrid.gridIntervals;
                                
                                rotateHall = false;
                            }

                            Debug.Log(endOfBranchPosition);

                            BuildBranch(endOfBranchPosition);
                            branchesGenerated++;
                        }
                    }
                }
            }while(branchesGenerated < numberOfBranches);

            foreach(KeyValuePair<Vector3, GameObject> keyValuePair in localModuleDictionary)
            {
                Vector3 tempKey = keyValuePair.Key;
                GameObject tempValue = keyValuePair.Value;
                if(macroGridStorage.moduleDictionary.ContainsKey(tempKey))
                {
                    macroGridStorage.moduleDictionary[tempKey] = tempValue;
                }else
                    macroGridStorage.moduleDictionary.Add(tempKey, tempValue);
            }
        }
    }

    void BuildBranch(Vector3 endOfBranch)
    {
        int roomOrHallModule = 1;
        do
        {
            if(!buildPath.buildVertical)
            {
                if(!buildPath.buildDirection)
                {
                    accessKeyY--;
                    if(accessKeyY < 0)
                    {
                        break;
                    }
                }
                else
                {
                    accessKeyY++;
                    if(accessKeyY > setMacroGrid.lengthWidth / 10)
                    {
                        break;
                    }
                }
            }
            else
            {
                if(!buildPath.buildDirection)
                {
                    accessKeyX--;
                    if(accessKeyX < 0)
                    {
                        break;
                    }
                }else
                {
                    accessKeyX++;
                    if(accessKeyX > setMacroGrid.lengthWidth / 10)
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
                GameObject tempModule;
                tempModule = Instantiate(modulePrefabs.roomPrefabs[0], macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position, Quaternion.identity);
                Debug.Log("Access Keys: (" + accessKeyX.ToString() + ", " + accessKeyY.ToString() + "), Position: " + tempModule.transform.position);
                localModuleDictionary.Add(tempModule.transform.position, tempModule);
                tempModule.GetComponent<AccessKeyHolder>().xAccessKey = accessKeyX;
                tempModule.GetComponent<AccessKeyHolder>().yAccessKey = accessKeyY;
                tempModule.GetComponent<AccessKeyHolder>().phaseDesignation = "Branch";
            }else
            {
                GameObject tempModule;
                tempModule = Instantiate(modulePrefabs.HallPrefabs[0], macroGridStorage.macroGridPoints[accessKeyX, accessKeyY].transform.position, Quaternion.identity);
                Debug.Log("Access Keys: (" + accessKeyX.ToString() + ", " + accessKeyY.ToString() + "), Position: " + tempModule.transform.position);
                if(rotateHall)
                    tempModule.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
                localModuleDictionary.Add(tempModule.transform.position, tempModule);
                tempModule.GetComponent<AccessKeyHolder>().xAccessKey = accessKeyX;
                tempModule.GetComponent<AccessKeyHolder>().yAccessKey = accessKeyY;
                tempModule.GetComponent<AccessKeyHolder>().phaseDesignation = "Branch";
            }

            roomOrHallModule++;
            
            bool endReached = localModuleDictionary.ContainsKey(endOfBranch);
            
            if(endReached)
            {
                continueBuild = false;
                if(localModuleDictionary[endOfBranch].gameObject.CompareTag("Hall"))
                {
                   if(buildPath.buildVertical)
                    {
                        endOfBranch += new Vector3(0.0f, setMacroGrid.gridIntervals, 0.0f);
                        if(!buildPath.buildDirection)
                            accessKeyY--;
                        else
                            accessKeyY++;
                    }else
                    {
                        endOfBranch += new Vector3(setMacroGrid.gridIntervals, 0.0f, 0.0f);
                        if(!buildPath.buildDirection)
                            accessKeyX--;
                        else
                            accessKeyX++;
                    }
                    GameObject tempModule = Instantiate(modulePrefabs.roomPrefabs[0], endOfBranch, Quaternion.identity);
                    macroGridStorage.moduleDictionary.Add(tempModule.transform.position, tempModule);
                    tempModule.GetComponent<AccessKeyHolder>().xAccessKey = accessKeyX;
                    tempModule.GetComponent<AccessKeyHolder>().yAccessKey = accessKeyY;
                    tempModule.GetComponent<AccessKeyHolder>().phaseDesignation = "Branch";
                }
            }
            
        }while(continueBuild);
    }
}
