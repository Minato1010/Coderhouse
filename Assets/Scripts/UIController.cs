using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.UI;
using UnityEngine.UI;


public class UIController : CoinScript
{
    [SerializeField] private TMP_Text coins;
    [SerializeField] private TMP_Text stars;
     private int coinsCollected;
    private int starsCollected;
    [SerializeField] private Image GameOver;
   
    [SerializeField] private Sprite life8;
    [SerializeField] private Sprite life7;
    [SerializeField] private Sprite life6;
    [SerializeField] private Sprite life5;
    [SerializeField] private Sprite life4;
    [SerializeField] private Sprite life3;
    [SerializeField] private Sprite life2;
    [SerializeField] private Sprite life1;
    [SerializeField] private Sprite life0;
    [SerializeField] private Canvas UI;
    [SerializeField] private Image LIFE;
    private Sprite currentLife;
    [SerializeField] private MarioController characterController;
    




    
    private void Start()
    {
        currentLife = life8;
        characterController.OnHealthChange += Life;
        StarScript.OnStarCollected += AddStars;
        OnCoinCollected.AddListener(AddCoins);
        Debug.Log("Suscriber, OnStarCollected, playerDied");
    }
   

    public void PlayerDied()
    {
        GameOver.gameObject.SetActive(true);
    }
    private void AddCoins(int coin)
    {
        coin = GameManager.instance.coins;
       
        coins.text = "x " + coin;
    }
    private void AddStars()
    {
        starsCollected = GameManager.instance.starsCollected;

        stars.text = "x " + starsCollected; 
    }
    private void Life(float marioLife)
    {
        
        if (marioLife >= 7)
        {
            currentLife = life8;

        }
        else if(marioLife>=6 && marioLife < 7)
        {
            currentLife = life7;

        }
        else if (marioLife >= 5 && marioLife < 6)
        {
            currentLife = life6;

        }
        else if (marioLife >= 4 && marioLife < 5)
        {
            currentLife = life5;

        }
        else if (marioLife >= 4 && marioLife < 4)
        {
            currentLife = life4;

        }
        else if (marioLife >= 3 && marioLife < 4)
        {
            currentLife = life3;

        }
        else if (marioLife >= 2 && marioLife < 3)
        {
            currentLife = life2;

        }
        else if (marioLife >= 1 && marioLife < 2)
        {
            currentLife = life1;

        }
        else if (marioLife < 1)
        {
            currentLife = life0;
        }

        LIFE.sprite = currentLife;
    }



}
