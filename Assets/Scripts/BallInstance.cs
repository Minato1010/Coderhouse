using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInstance : MonoBehaviour
{
    [SerializeField] private Transform position1;
    [SerializeField] private Transform position2;
    [SerializeField] private Transform ball;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private float timeToAppear;
    [SerializeField] private Rigidbody rigidbodyBall;
    private bool appear;

    private void Update()
    {
       AppearBall();

    }
    private void AppearBall()
    {

        if (timeToAppear <= Time.time && appear==true)
        {
            Instantiate(ball, position1.position, Quaternion.identity);
            Instantiate(ball, position2.position,Quaternion.identity);
            appear = false;
            
        }
        else if (appear == false)
        {
          
            timeToAppear = 15+ Time.time;
            appear = true;
        }
        
    }
   

}
