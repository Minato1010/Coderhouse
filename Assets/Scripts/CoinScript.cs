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
    public AudioClip coinAudio;
    private void Awake()
    {
        Debug.Log("Publisher OnCoinCollected");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.audioSource.PlayOneShot(coinAudio);

            if (Healing == true)
            {
                OnCoinCollected?.Invoke();
                
                Healing = false;
                Destroy(gameObject);
            }
            
        }

    }


}
