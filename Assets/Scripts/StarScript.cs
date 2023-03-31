using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class StarScript : MonoBehaviour
{
    public static Action OnStarCollected;
    public float timeToDestroy;
    public bool destroy;
    [SerializeField] private AudioClip StarAppears;

    private void Awake()
    {
        Debug.Log("Publisher OnStarCollected");
    }

    private void Start()
    {
        GameManager.instance.audioSource.Stop();
        GameManager.instance.audioSource.PlayOneShot(StarAppears);
    }
    private void Update()
    {
        if (destroy == true && timeToDestroy <= Time.time)
        {
            OnStarCollected?.Invoke();
            Destroy(gameObject);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (destroy == false)
            {
                GameManager.instance.marioTransform.Heal(8);
                GameManager.instance.audioSource.Stop();
                GameManager.instance.audioSource.PlayOneShot(GameManager.instance.marioTransform.StarCollected);
                destroy = true;
                timeToDestroy = 3.4f + Time.time;
            }           

            
        }
    }

}
