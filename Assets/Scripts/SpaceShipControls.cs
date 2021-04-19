using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceShipControls : MonoBehaviour
{
    public Rigidbody2D rd;
    public SpriteRenderer spriteRenderer;
    public Collider2D colider;
    public float thrust;
    public float turnThrust;
    private float thrustInput;
    private float turnInput;
    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;
    public float bulletForce;
    public float deathForce;
    private bool hyperspace; //true = currently hyperspacing

    public int score;
    public int lives;

    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;

    public AudioSource audio;

    public GameObject explosion;
    public GameObject bullet;

    public Color inColor; 
    public Color normalColor; 
     

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        hyperspace  = false;
        scoreText.text = "Score " + score;
        livesText.text = "Lives " + lives;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for input from the keyboard
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        //Check for input  from the fire key and make bullets

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletForce);
            Destroy(newBullet, 5.0f);//bullet lifetime to free memory
        }

        //Check for hyperspace
        if(Input.GetButtonDown ("Hyperspace") && !hyperspace)
        {
            hyperspace = true;
            //Turn of colliders and spriteRender
            spriteRenderer.enabled = false;
            colider.enabled = false;
            Invoke("Hyperspace", 1f);
        }

        //Rotate the ship
        //not very natural, but more friendly control, we fully control the turn of the ship
        transform.Rotate(Vector3.forward * turnInput * Time.deltaTime * -turnThrust);  


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

    void  FixedUpdate()
    {
        rd.AddRelativeForce(Vector2.up * thrustInput);
        //rd.AddTorque(-turnInput);
    }

    void ScorePoints(int pointsToAdd) //score points
    {
        score += pointsToAdd;
        scoreText.text = "Score " + score;
    }


    void Respawn()
    {

        rd.velocity = Vector2.zero;
        transform.position = Vector2.zero;

       
        spriteRenderer.enabled = true;
        spriteRenderer.color = inColor;
        Invoke("Invulnerable", 3f);
    }

    void Invulnerable()
    {
         colider.enabled = true;
         spriteRenderer.color = normalColor;
    }

    void Hyperspace()
    {
        //Move to new render position
        Vector2 newPosition = new Vector2(Random.Range(-14f, 14f), Random.Range(-9f, 9f));
        transform.position = newPosition;
        //Turn on colliders and spriteRender
        spriteRenderer.enabled = true;
        colider.enabled = true;

        hyperspace = false;
    }

    //fixing collision with asteroid
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.relativeVelocity.magnitude);
        audio.Play();
        if(col.relativeVelocity.magnitude > deathForce)
        {
            lives--;
             //Make explosion
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 3f);
            livesText.text = "Lives " + lives;
            //Respawn - New Live
            spriteRenderer.enabled = false;
            colider.enabled = false;
            Invoke("Respawn", 3f);

            if (lives <= 0)
            {
                //GameOver
                GameOver();
            }
        } 
        else
        {
            audio.Play();
        }
    }

    void GameOver()
    {
        CancelInvoke();
        gameOverPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
