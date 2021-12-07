using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPopulator : MonoBehaviour
{
    private LocalGridSpawner_Room localGridSpawner;
    private PopulationMaster popMaster;
    public GameObject[] enemyPrefabs;
    
    public void SpawnEnemies()
    {
        localGridSpawner = GetComponent<LocalGridSpawner_Room>();
        popMaster = GameObject.Find("DungeonMaster").GetComponent<PopulationMaster>();

        int maximumEnemies = MyOwnRandomizer.TwoNumberIntReturn((float)popMaster.minEnemies, (float)popMaster.maxEnemies);
        int numberOfEnemies = 0;

        do
        {
            bool continueSpawning = true;

            do
            {
                int accessKeyX = MyOwnRandomizer.TwoNumberIntReturn(0.0f, localGridSpawner.lengthWidth);
                int accessKeyY = MyOwnRandomizer.TwoNumberIntReturn(0.0f, localGridSpawner.lengthWidth);

                if(localGridSpawner.localGrid[accessKeyX, accessKeyY].Traversable)
                {
                    GameObject tempItem;
                    tempItem = Instantiate(enemyPrefabs[0], localGridSpawner.localGrid[accessKeyX, accessKeyY].WaypointObject.transform.position, Quaternion.identity);
                    localGridSpawner.localGrid[accessKeyX, accessKeyY].Traversable = false;
                    continueSpawning = false;
                } 
            }while(continueSpawning);

            numberOfEnemies++;
        }while(numberOfEnemies < maximumEnemies);
    }
}
