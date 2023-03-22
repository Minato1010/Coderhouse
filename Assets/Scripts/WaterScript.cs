using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            GameManager.instance.marioTransform.animator.SetBool("Swimming", true);



        }


    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            GameManager.instance.marioTransform.animator.SetBool("Swimming", false);


        }



    }



}
