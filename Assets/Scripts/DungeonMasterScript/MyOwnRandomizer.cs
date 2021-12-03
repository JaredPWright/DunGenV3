using System;
using UnityEngine;

public class MyOwnRandomizer : MonoBehaviour
{
    public bool TwoNumberRandomizer(float x, float y)
    {
        int randomNumber = (int)UnityEngine.Random.Range(x, y);
        if(randomNumber <= 50)
        {
            return false;
        }else
            return true;
    }

    public static Vector3 TwoNumberVector3Return(float xMin, float xMax, float zMin, float zMax, float yMin = 0, float yMax = 0)
    {
        int randomX = (int)UnityEngine.Random.Range(xMin, xMax);
        int randomY = (int)UnityEngine.Random.Range(yMin, yMax);
        int randomZ = (int)UnityEngine.Random.Range(zMin, zMax);

        return new Vector3(randomX, randomY, randomZ);
    }

    public static int TwoNumberIntReturn(float val1, float val2)
    {
        int randomSeed = System.DateTime.Now.Millisecond * 10;
        //Debug.Log(randomSeed.ToString());
        UnityEngine.Random.InitState(randomSeed);
        int randomInt = (int)UnityEngine.Random.Range(val1, val2);
        return randomInt;
    }
}