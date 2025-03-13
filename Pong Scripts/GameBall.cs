using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameBall : MonoBehaviour
{
    public float speedIncreaseValue = 2f;

    GameManager manager;

    //When the ball's position on the x-axis in WORLD COORDINATES 
    // is greater than this or less than (-this) then a goal has happened
    float scoreThreshold = 18f;


    void Awake() //Note we use Awake as we want this to run whenever we spawn a new Ball instance
    {
        manager = FindObjectOfType<GameManager>();

        //Note that in order for this function to run we had to add initial velocity
        //to the ball when we spawned it-- otherwise the ball will remain asleep forever!
       
    }

    // Update is called once per frame
    void Update()
    {
        if ( gameObject.transform.position.x >= scoreThreshold) // meaning the player scored
        {
            manager.RoundEnd("player");
        }

        if (gameObject.transform.position.x <= (-scoreThreshold) ) // meaning the AI scored
        {
            manager.RoundEnd("AI");
        }
        
    }
    
    //We used to have score-bars that the ball was meant to collide with and that is how we knew who scored.
    //The problem was when the ball got too fast it would just go through the entire collider 
    // and Unity would not recognize that the ball collided with a score-bar. 
    // We now instead use the update function and check the ball's position every frame.
    


    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "Paddle")  //The ball speeds up when it collides with the Player or Computer paddle
        {

            //increase the magnitute of the ball velocity by speedIncreaseValue. 
            // Note we do this on OnCollisionExit and not on OnCollisionEnter.
            Vector3 currentDirection = gameObject.GetComponent<Rigidbody>().velocity.normalized;// velocity is in world space
            gameObject.GetComponent<Rigidbody>().AddForce(currentDirection * speedIncreaseValue, ForceMode.VelocityChange);
            //AddForce the vector is in global coordinates

        }

    }

}
