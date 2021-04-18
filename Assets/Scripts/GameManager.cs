using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int numberOfAsteroids; //This is the current number of asteroids in the scene
    public int levelNumber = 1; 
    public GameObject asteroid;
   

    public void UpdateNumberOfAsteroids(int change)
    {
        numberOfAsteroids += change;

        //Check to see if we have any asteroids left
        if(numberOfAsteroids <= 0)
        {
            //Start new level
            Invoke("StartNewLevel", 3f);
        }
    } 

    void StartNewLevel()
    {
        levelNumber++;

        //Spawn new asteroids
        for(int i = 0; i < levelNumber*2; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-18.5f, 18.5f), 12f);
            Instantiate(asteroid, spawnPosition, Quaternion.identity);
            numberOfAsteroids++;
        }
    }
}
