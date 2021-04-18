using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{

    public float maxThrust; //max Thrust
    public float maxTorque; //max torque
    public Rigidbody2D rb;
    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;
    public int asteroidSize; // 3 = Large, 2 = Medium, 1 = Small
    public GameObject asteroidMedium;
    public GameObject asteroidSmall;
    public int points;
    public GameObject player;
    public GameObject explosion;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //add a random mount of torque and trust to the asteroid
        Vector2 thrust = new Vector2(Random.Range(-maxThrust, maxThrust), Random.Range(-maxThrust, maxThrust));
        float torque = Random.Range(-maxTorque, maxTorque);

        rb.AddForce(thrust);
        rb.AddTorque(torque);

        //Fiend the player
        player = GameObject.FindWithTag("Player");
        //Find game manager
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Screen Wraping

        Vector2 newPos = transform.position;
        if (transform.position.y > screenTop)
        {
            newPos.y = screenBottom;
        }
        
        if (transform.position.y < screenBottom)
        {
            newPos.y = screenTop;
        }

        if (transform.position.x > screenRight)
        {
            newPos.x = screenLeft;
        }
        
        if (transform.position.x < screenLeft)
        {
            newPos.x = screenRight;
        }

        transform.position = newPos;
    }
    
    //when a bullet collides with an asteroid, the object of the field is destroyed 
     void OnTriggerEnter2D(Collider2D other)
    {
        //Check to see if its a bullet
        if (other.CompareTag("bullet"))
        {
            //destroy the bullet 
            Destroy(other.gameObject);
            //Check size of asteroid and spawn in next smaller size
            if (asteroidSize == 3)
            { 
                //Spawn two medium asteroids
                Instantiate(asteroidMedium, transform.position, transform.rotation); 
                Instantiate(asteroidMedium, transform.position, transform.rotation);

                gameManager.UpdateNumberOfAsteroids(1);
            }
            else if (asteroidSize == 2)
            {
                //Spawn two small asteroids
                Instantiate(asteroidSmall, transform.position, transform.rotation); 
                Instantiate(asteroidSmall, transform.position, transform.rotation);

                gameManager.UpdateNumberOfAsteroids(1);
            }
            else if (asteroidSize == 1)
            {
                //Remove the asteroid
                gameManager.UpdateNumberOfAsteroids(-1);
                
            }
            //tell the player to score some point by shuting
            player.SendMessage("ScorePoints", points); 

            //Make an explosion
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 3f);

            //Remove the current asteroid
            Destroy(gameObject);
        }
    }

}
