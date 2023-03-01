using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }
    

    public void CharacterScore(float ScoreAmount)
    {
        score += ScoreAmount;
        Debug.Log("Your Score: " + score);

    }
    public void CoinsCollected(int Coin)
    {

        Coin += Coin;
        Debug.Log("Coins collected: " + Coin);


    }
    public void KingBobOmbDefeated()
    {
        
           var mario= marioTransform.GetComponent<MarioController>();
            mario.KingBobOmbDefeated();
            Debug.Log("Victory");
            KingBobOmbDied = true;
        
    }
   

}
