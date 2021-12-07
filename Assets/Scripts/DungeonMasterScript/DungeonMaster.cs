using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMaster : MonoBehaviour
{
    public SetMacroGrid setMacroGrid;
    public MacroGridStorage macroGridStorage;
    public LocalGridStorage localGridStorage;
    // Start is called before the first frame update
    void Start()
    {
        //Start the chain reaction. Call BuildMacroGrid which then calls the room spawner.
        setMacroGrid.BuildMacroGrid();
    }

    public void BuildMasterGrid()
    {
        int numberOfTileRowsGenerated = 0;
        Debug.Log("about to enterForeach");
        foreach(KeyValuePair<Vector3, GameObject> kvp in macroGridStorage.moduleDictionary)
        {
            Debug.Log("Hello am in foreach");
            Debug.Log(kvp.Key.ToString());
            Debug.Log(kvp.Value.ToString());
            if(kvp.Value.gameObject.CompareTag("Room"))
            {
                Debug.Log("In Local Grid Spawner");
                for(int i = 0; i < 10; i++)
                {
                    for(int j = 0; j < 10; j++)
                    {
                        localGridStorage.AddToLocalGrid(numberOfTileRowsGenerated, j, kvp.Value.GetComponent<LocalGridSpawner_Room>().localGrid[i,j]);
                    }

                    numberOfTileRowsGenerated++;
                }
            }else if(kvp.Value.gameObject.CompareTag("Hall"))
            {
                for(int i = 0; i < 10; i++)
                {
                    for(int j = 0; j < 10; j++)
                    {
                        localGridStorage.AddToLocalGrid(numberOfTileRowsGenerated, j, kvp.Value.GetComponent<LocalGridSpawner_Hall>().localGrid[i,j]);
                    }

                    numberOfTileRowsGenerated++;
                }
            }

        }

        foreach(KeyValuePair<Vector3, GameObject> kvp in macroGridStorage.moduleDictionary)
        {
            if(!(kvp.Value.CompareTag("Branch") || kvp.Value.CompareTag("Trunk")))
            {
                Destroy(kvp.Value.gameObject);
                macroGridStorage.moduleDictionary.Remove(kvp.Key);
            }
        }
        Debug.Log("Exiting foreach");
    }
}
