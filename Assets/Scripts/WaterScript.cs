using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {


        if (other.gameObject.tag == "Player")
        {
            Physics.gravity *= -1;



        }



    }



}
