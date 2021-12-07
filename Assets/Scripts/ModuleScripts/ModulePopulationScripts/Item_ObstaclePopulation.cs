using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DunGenDataTypes;

public class Item_ObstaclePopulation : MonoBehaviour
{
    public LocalGridSpawner_Room localGridSystem;
    public PopulationMaster popMaster;
    public GameObject[] itemPrefabs;
    public GameObject[] obstaclePrefabs;

    public void SpawnItems()
    {
        localGridSystem = GetComponent<LocalGridSpawner_Room>();
        popMaster = GameObject.Find("DungeonMaster").GetComponent<PopulationMaster>();
        
        bool continueSearching = true;

        do
        {
            int accessKeyX = MyOwnRandomizer.TwoNumberIntReturn(0.0f, 10.0f);
            int accessKeyY = MyOwnRandomizer.TwoNumberIntReturn(0.0f, 10.0f);
            
            Debug.Log("Loading Item Info: " + accessKeyX + accessKeyY);
            LocalGridDataType dataTypeObject = localGridSystem.localGrid[accessKeyX, accessKeyY];
            bool placeObject = dataTypeObject.Traversable;
            if(placeObject)
            {
                GameObject tempItem;
                tempItem = Instantiate(itemPrefabs[0], localGridSystem.localGrid[accessKeyX, accessKeyY].WaypointObject.transform.position, Quaternion.identity);
                localGridSystem.localGrid[accessKeyX, accessKeyY].Traversable = false;
                continueSearching = false;
            }
        }
        while(continueSearching);
    }

    public void SpawnObstacles()
    {
        localGridSystem = GetComponent<LocalGridSpawner_Room>();
        popMaster = GameObject.Find("DungeonMaster").GetComponent<PopulationMaster>();

        int numberOfObstaclesToSpawn = MyOwnRandomizer.TwoNumberIntReturn((float)popMaster.minObstacles, (float)popMaster.maxObstacles);
        int numberOfObstacles = 0;

        do
        {
            bool continueSearching = true;

            do
            {
                int accessKeyX = MyOwnRandomizer.TwoNumberIntReturn(0.0f, localGridSystem.lengthWidth);
                int accessKeyY = MyOwnRandomizer.TwoNumberIntReturn(0.0f, localGridSystem.lengthWidth);

                LocalGridDataType localGridType = localGridSystem.localGrid[accessKeyX, accessKeyY];
                Debug.Log(localGridSystem.localGrid[accessKeyX, accessKeyY]);
                if(localGridType.ReturnTraversable())
                {
                    Debug.Log("inside check");
                    GameObject tempItem;
                    tempItem = Instantiate(obstaclePrefabs[0], localGridSystem.localGrid[accessKeyX, accessKeyY].WaypointObject.transform.position, Quaternion.identity);
                    localGridSystem.localGrid[accessKeyX, accessKeyY].Traversable = false;
                    continueSearching = false;
                }
            }
            while(continueSearching);

            numberOfObstacles++;

        }while(numberOfObstacles < numberOfObstaclesToSpawn);
    }
}
