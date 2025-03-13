using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPongPaddle : MonoBehaviour
{
    public float speed;

    public float movementValue;


    float zBound = 8.34f;

    GameManager manager; //We need the manager to know if the game is active or not and which cam is active

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.isGameOver == false)
        {
            //We move using W and S when in 2D view and using A and D when in 3D view
            if (manager.isCam2DActive) //if we are in 2D view
            {
                movementValue = Input.GetAxis("Vertical");
            }
            else //meaning we are in 3D view
            {
                movementValue = Input.GetAxis("Horizontal");
            }

            //Move the player
            transform.Translate(0, 0, movementValue * speed * Time.deltaTime);

            //The following ifs make sure we stay in bounds
            if (transform.position.z > zBound)
            {
                transform.position = new Vector3(-15.0f, 0.241f, zBound);

            }
            if (transform.position.z <-zBound)
            {
                transform.position = new Vector3(-15.0f, 0.241f, -zBound);
            }

        }
    }
}
