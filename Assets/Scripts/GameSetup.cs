using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    int redBallRemaining = 7;
    int blueBallRemaining = 7;
    float ballRadius;
    float ballDiameter;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform cueBallPosition;
    [SerializeField] Transform headBallPosition;

    /**
     * Awake is called before Start,
     * to make sure the cue ball already exists before the camera control
     */
    private void Awake()
    {
        ballRadius = ballPrefab.GetComponent<SphereCollider>().radius * 100f;
        ballDiameter = ballRadius * 2f;
        PlaceAllBalls();
    }

    void PlaceAllBalls()
    {
        PlaceCubeBall();
        PlaceRandomBalls();
    }

    void PlaceCubeBall()
    {
        GameObject ball = Instantiate(ballPrefab, cueBallPosition.position, Quaternion.identity);
        ball.GetComponent<Ball>().MakeCueBall();
    }

    void PlaceEightBall(Vector3 position)
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        ball.GetComponent <Ball>().MakeEightBall();
    }

    void PlaceRandomBalls()
    {
        int NumInThisRow = 1;
        int rand;
        Vector3 firstInRowPosition = headBallPosition.position;
        Vector3 currentPosition = firstInRowPosition;

        void PlaceRedBall(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallSetup(true);
            redBallRemaining--;
        }

        void PlaceBlueBall(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallSetup(false);
            blueBallRemaining--;
        }

        //Outer loop is the 5 rows
        for(int i = 0; i < 5; i++)
        {
            //Inner loop is the balls in each row
            for(int j=0;j<NumInThisRow;j++)
            {
                //check to see if it's the miiddle where the eight ball goes
                if (i == 2 && j == 1)
                {
                    PlaceEightBall(currentPosition);
                }
                //random between red and blue
                else if (redBallRemaining > 0 && blueBallRemaining > 0)
                {
                    rand = Random.Range(0, 2);
                    if(rand == 0)
                    {
                        PlaceRedBall(currentPosition);
                    }
                    else
                    {
                        PlaceBlueBall(currentPosition);
                    }
                }
                //place red
                else if (redBallRemaining > 0)
                {
                    PlaceRedBall(currentPosition);
                }
                //place blue
                else
                {
                    PlaceBlueBall(currentPosition);
                }
                //move the current postion for the next ball in this row to the right
                currentPosition += new Vector3(1, 0, 0).normalized * ballDiameter;
            }
            //once all the balls in the row have been placed, move to the next row
            firstInRowPosition +=Vector3.back * (Mathf.Sqrt(3)* ballRadius) + Vector3.left*ballRadius;
            currentPosition = firstInRowPosition;
            NumInThisRow++;
        }
    }
}
