using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    protected bool Healing = true;
    private int CoinCollected = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var marioControler = other.GetComponent<MarioController>();

            if (Healing == true)
            {
                marioControler.Heal(1);
                GameManager.instance.CoinsCollected(CoinCollected);
                Healing = false;
                Destroy(gameObject);
            }
            
        }

    }


}
