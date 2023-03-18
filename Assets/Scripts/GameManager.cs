using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float score;
    public MarioController marioTransform;
    public AudioSource audioSource;
    public AudioClip goombaDestroyed;
    public AudioClip goombaFindMario;
    public EnemyScript goombaPrefab;
    public Transform[] goombaPositions;
    public bool KingBobOmbDied;
    [SerializeField] private Transform insideCastle;
    public int coins;
    public int starsCollected;
    [SerializeField] private StarScript Star;
    [SerializeField] private Transform StarPosition;
    public AudioClip FileSelect;
    public AudioClip peachCastle;

    private void Awake()
    {
        

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        StarScript.OnStarCollected += StarCollected;

        Debug.Log("Suscriber EnemyDied, Star,King");
    }

    private void Start()
    {
        EnemyScript.OnEnemyDied += CharacterScore;
     
    }
    public void CharacterScore()
    {
        score += 50;
        Debug.Log("Your Score: " + score);

    }
   public void AddCoins()
    {
        
        coins +=1;
    }

    public void KingBobOmbDefeated()
    {
        
           var mario= marioTransform.GetComponent<MarioController>();
          
            Debug.Log("Victory");
            Instantiate(Star, StarPosition);

            KingBobOmbDied = true;
        
        
    }
    public void StarCollected()
    {
        var mario = marioTransform.GetComponent<MarioController>();

        starsCollected += 1;
        audioSource.Stop();
        mario.transform.position = insideCastle.position;

        audioSource.PlayOneShot(peachCastle);

    }


}
