using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPongPaddle : MonoBehaviour
{
    GameObject currentBall;
    public float AISpeed;

    float zBound = 8.34f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    //The choice for the AI was between making:
    // * A slower movement speed AI that updates how it moves every frame.
    // * A higher speed AI that only updates how it moves every 0.2 seconds or something 
    // (which results in a "dummer" AI that can overshoot or react with lag).

    //Ultimately I think a slower movement speed AI 
    // that updates how it moves every frame is more interesting and fun to play against.



    // Update is called once per frame -- We use LateUpdate here to make sure the AI moves after the ball
    // and that the ball won't be destroyed immediately after this function 
    // (which would make it seem in the next frame as if the AI moved for no reason as
    // there would no longer be a ball).
    void LateUpdate()
    {
        if (currentBall != null) //This checks the current ball has not yet been destroyed
        {
            //If the current ball is above the AIPaddle, move the AI paddle up.
            //We added the requirement that the difference must be bigger than 0.5 as it reduced WebGL jitter.
            if (currentBall.transform.position.z - transform.position.z >  0.5) 
            {
                transform.Translate(0, 0, 1 * AISpeed * Time.deltaTime);
            }
            else if (0.5 < transform.position.z - currentBall.transform.position.z)
            {
                transform.Translate(0, 0, -1 * AISpeed * Time.deltaTime);
            }

            //The following ifs make sure we stay in bounds
            if (transform.position.z > zBound)
            {
                transform.position = new Vector3(15.0f, 0.241f, zBound);

            }
            if (transform.position.z < -zBound)
            {
                transform.position = new Vector3(15.0f, 0.241f, -zBound);
            }


        }

    }

    public void SetCurrentBall(GameObject Ball)
    {
        currentBall = Ball;
    }
}
