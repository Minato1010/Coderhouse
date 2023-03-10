using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StarScript : MonoBehaviour
{
    public static Action OnStarCollected;


    private void Awake()
    {
        Debug.Log("Publisher OnStarCollected");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnStarCollected?.Invoke();
            
            Destroy(gameObject);
        }
    }

}
