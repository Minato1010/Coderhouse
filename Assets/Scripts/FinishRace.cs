using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishRace : MonoBehaviour
{

    public static string raceWiner;

    private bool winner;



    private void OnTriggerEnter(Collider other)
    {
        if (KoopaBossController.playMusic == true)
        {
            if (other.gameObject.tag == "Player" && winner==false)
            {
                raceWiner = "Mario";
                winner = true;
            }
            else if (other.gameObject.tag == "BossKoopa" && winner==false)
            {

                raceWiner="Koopa";
                winner = true;
            }



        }



    }
}
