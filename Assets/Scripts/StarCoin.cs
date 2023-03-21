using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StarCoin : MonoBehaviour
{

    public UnityEvent<int> OnStarCoinCollected;
    public AudioClip StarCoinAudio;




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            OnStarCoinCollected?.Invoke(1);
            GameManager.instance.audioSource.PlayOneShot(StarCoinAudio);

            Destroy(gameObject);

        }



    }



}
