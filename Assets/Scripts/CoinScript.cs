using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
public class CoinScript : MonoBehaviour
{
    protected bool Healing = true;
    protected int CoinCollected = 1;
    public UnityEvent OnCoinCollected;
    private void Awake()
    {
        Debug.Log("Publisher OnCoinCollected");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var marioControler = other.GetComponent<MarioController>();

            if (Healing == true)
            {
                OnCoinCollected?.Invoke();
                
                Healing = false;
                Destroy(gameObject);
            }
            
        }

    }


}
