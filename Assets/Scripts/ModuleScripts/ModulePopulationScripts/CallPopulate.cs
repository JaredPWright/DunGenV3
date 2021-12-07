using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallPopulate : MonoBehaviour
{
    private PopulationMaster popMaster;
    private MacroGridStorage macroGridStorage;
    private DungeonMaster dungeonMaster;
    public int roomsWithObstacles = 0;
    public int roomsWithItems = 0;
    public int roomsWithEnemies = 0;

    public void PopulateDungeon()
    {
        popMaster = GetComponent<PopulationMaster>();
        macroGridStorage = GetComponent<MacroGridStorage>();
        dungeonMaster = GetComponent<DungeonMaster>();

        Debug.Log("About to create Obstacles");
        for(int i = 0; i < popMaster.howManyRoomsWithObstacles; i++)
        {
            macroGridStorage.roomModules[MyOwnRandomizer.TwoNumberIntReturn(0.0f, macroGridStorage.roomModules.Count)].GetComponent<Item_ObstaclePopulation>().SpawnObstacles();
            roomsWithObstacles++;
        }

        Debug.Log("About to create Items");
        for(int i = 0; i < popMaster.howManyRoomsWithTreasure; i++)
        {
            macroGridStorage.roomModules[MyOwnRandomizer.TwoNumberIntReturn(0.0f, macroGridStorage.roomModules.Count)].GetComponent<Item_ObstaclePopulation>().SpawnItems();
            roomsWithItems++;
        }

        for(int i = 0; i < macroGridStorage.roomModules.Count / popMaster.howManyRoomsWithEnemies; i++)
        {
            macroGridStorage.roomModules[MyOwnRandomizer.TwoNumberIntReturn(0.0f, macroGridStorage.roomModules.Count)].GetComponent<EnemyPopulator>().SpawnEnemies();
        }

        dungeonMaster.BuildMasterGrid();
    }
}
