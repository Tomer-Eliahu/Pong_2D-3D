using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static System.Net.WebRequestMethods;

//Special Imports
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public int numberOfGoalsToWin = 3;

    int roundNumber = 1;
    int playerGoals = 0;
    int computerGoals = 0;

    public GameObject ball;

    GameObject currentBall;

    float initialBallSpeed = 15.0f;


    //AI stuff
    AIPongPaddle AI;

    //UI stuff
    public GameObject titleScreen;
    public GameObject gameOverlayScreen;
    public GameObject gameOverWinScreen;
    public GameObject gameOverLoseScreen;
    //UI text we update during the game
    public TextMeshProUGUI scoreboardText;
    public TextMeshProUGUI getReadyText;

    public bool isGameOver; //We set the initial value to True in the Editor


    //Camera switching stuff
    public bool isCam2DActive;//We set the initial value to True in the Editor

    public GameObject Cam2D; //the main camera in the editor
    public GameObject Cam3D; //the secondary camera in the editor

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) //User pressed the 'C' key to switch views
        {
            SwitchCams();
        }

    }

    private IEnumerator ReadySetGo()
    {
        //Waits 1 second in real-time-- We do this to make sure people have enough time to read the score
        yield return new WaitForSecondsRealtime(1); 
        getReadyText.text = "Get Ready!";
        yield return new WaitForSecondsRealtime(1);

        for (int i = 3; i>0; i--)
        {
            getReadyText.text = $"{i}";
            yield return new WaitForSecondsRealtime(1);
        }
        getReadyText.text = "Go!";

    }

    void RoundStart()
    {
        //Initialize round

        int rotationAngle = Random.Range(-75, 76);

        if (Random.Range(0, 2) == 1) //decides randomly whether the ball will be first shot to the Player or the computer.
        {
            rotationAngle = rotationAngle + 180;
        }


        //We rotate by an additional 90 degrees so that in the
        //  local coordinates of the ball the z-axis will be direction of movement.
        currentBall = Instantiate(ball, ball.transform.position, UnityEngine.Quaternion.Euler(0, 90 + rotationAngle, 0));

        //Critical-- We must add a force on the ball here (not in the ball awake function-- that would not work). 
        // If we don't the ball will be asleep by default
        //and remain asleep (The awake function for the ball in ball.cs will NOT run ever).
        currentBall.GetComponent<Rigidbody>().AddForce(currentBall.transform.forward * initialBallSpeed, ForceMode.VelocityChange);
        

        //Once the ball is launched we clear the "Go!" message from the screen
        getReadyText.text = "";

        
        AI.SetCurrentBall(currentBall);

    }


    public void RoundEnd(string whoScored)
    {
        Destroy(currentBall);
        if (whoScored == "player")
        {
            playerGoals++;
            UpdateScore();
            if (playerGoals == numberOfGoalsToWin)
            {
                Debug.Log("Player Wins!");
                gameOverWinScreen.SetActive(true);
                isGameOver = true;
                return;
            }
        }
        if (whoScored == "AI")
        {
            computerGoals++;
            UpdateScore();
            if (computerGoals == numberOfGoalsToWin)
            {
                Debug.Log("AI Wins!");
                gameOverLoseScreen.SetActive(true);
                isGameOver = true;
                return;
            }
        }

        //If we got here that means no one won yet
        roundNumber++;
        StartCoroutine(ReadySetGo()); // This prints the get ready messages
        Invoke("RoundStart", 5.2f); //initialize the next round in 5.2 seconds
    }

    //reload the current scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        titleScreen.SetActive(false);

        isGameOver = false;

        //initialize score
        gameOverlayScreen.SetActive(true);
        UpdateScore(); //The initial scoreboardText will be Player: 0 AI: 0

        //start the game
        AI = FindObjectOfType<AIPongPaddle>(); // Find the AI paddle GameObject by type.

        StartCoroutine(ReadySetGo()); // This prints the get ready messages

        //initialize round 1 in 5.2 seconds (we add .2 seconds to make sure it happens after ReadySetGo is done).
        Invoke("RoundStart", 5.2f);

    }
    
    void UpdateScore()
    {
        scoreboardText.text = $"Player: {playerGoals}     AI: {computerGoals}";
    }

    void SwitchCams()
    {
        //I am allowing switching cameras even if the game has not started yet or has ended.

        //switching from 2D to 3D
        if (isCam2DActive)
        {
            Cam2D.SetActive(false);
            Cam3D.SetActive(true);
            isCam2DActive = false;
        }
        else //switching from 3D to 2D
        {
            Cam3D.SetActive(false);
            Cam2D.SetActive(true);
            isCam2DActive = true;
        }
    }
}
