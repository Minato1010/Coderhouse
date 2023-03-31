using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
using TMPro;

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
    public List<Transform> KoopaBossPositions;
    public GameObject PanelUi;
    public GameObject PanelText;
    public TMP_Text textTalk;

    public bool KingBobOmbDied;
    [SerializeField] private Transform insideCastle;
    public int coins;
    public static int starsCollected=1;
   public StarScript Star;
    public Transform StarPosition;
    public AudioClip FileSelect;
    public AudioClip peachCastle;
    public int StarCoinsCollected;
    public Transform StarPositionCoins;
    public KoopaBossController KoopaBoss;
    public Transform koopaPosition;
    public AudioClip raceMusic;
    public Transform worldToGo;
    

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
        Instantiate(KoopaBoss);
        KoopaBoss.transform.position = koopaPosition.position;

        Debug.Log("Victory");
            Instantiate(Star, StarPosition);
        

            KingBobOmbDied = true;
        
        
    }
    public void StarCollected()
    {

        
        audioSource.Stop();
        audioSource.PlayOneShot(marioTransform.StarCollected);

        audioSource.Stop();
        
        marioTransform.gameObject.transform.position= worldToGo.position;

        audioSource.PlayOneShot(peachCastle);

    }
    public void StarCoinCollected(int Coins)
    {
        

        StarCoinsCollected += Coins;

        if (StarCoinsCollected == 8)
        {
            Debug.Log("Star Appeared");
            Instantiate(Star, StarPositionCoins);
            StarCoinsCollected = 0;

        }




    }


}
