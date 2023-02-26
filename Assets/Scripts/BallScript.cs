using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private float timeToDestroy;
    private bool appear;
    private void DestroyBall()
    {

        if (timeToDestroy <= Time.time && appear == true)
        {
            Destroy(gameObject);

        }
        else if (appear == false)
        {
            timeToDestroy = 30 + Time.time;
            appear = true;
        }
    }
    private void Update()
    {
        DestroyBall();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            var marioController = collision.collider.gameObject.GetComponent<MarioController>();
            marioController.ReceiveDamage(1);
            marioController.marioRigidbody.AddForce(marioController.transform.TransformDirection(Vector3.back) * 10, ForceMode.Impulse);
        }

        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "InvisibleWall")
        {
            Destroy(gameObject);
        }
    }
}
