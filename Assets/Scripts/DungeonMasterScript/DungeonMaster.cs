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

        int numberOfTileRowsGenerated = 0;
        foreach(KeyValuePair<Vector3, GameObject> kvp in macroGridStorage.moduleDictionary)
        {
            if(kvp.Value.CompareTag("Room"))
            {
                for(int i = 0; i < 10; i++)
                {
                    for(int j = 0; j < 10; j++)
                    {
                        localGridStorage.AddToLocalGrid(numberOfTileRowsGenerated, j, kvp.Value.GetComponent<LocalGridSpawner_Room>().localGrid[i,j]);
                    }

                    numberOfTileRowsGenerated++;
                }
            }else
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
    }
}
