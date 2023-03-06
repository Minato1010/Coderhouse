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
    }
   

    public void CharacterScore(float ScoreAmount)
    {
        score += ScoreAmount;
        Debug.Log("Your Score: " + score);

    }
    public void CoinsCollected(int Coin)
    {
        coins += Coin;


    }
    public void KingBobOmbDefeated()
    {
        
           var mario= marioTransform.GetComponent<MarioController>();
          
            Debug.Log("Victory");
            Instantiate(Star, StarPosition);

            KingBobOmbDied = true;
        
        
    }
    public void StarCollected(int stars)
    {
        var mario = marioTransform.GetComponent<MarioController>();

        starsCollected += stars;
        audioSource.Stop();
        mario.transform.position = insideCastle.position;

        audioSource.PlayOneShot(peachCastle);

    }


}
