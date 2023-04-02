using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text coins;
    [SerializeField] private TMP_Text stars;
    private int starsCollected;
    [SerializeField] private Image GameOver;
    private int coinsCollected;
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

    public bool CanRespawn;

    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject normalMenu;



    
    private void Start()
    {
        currentLife = life8;
        characterController.OnHealthChange += Life;
        StarScript.OnStarCollected+=AddStars;
       
        Debug.Log("Suscriber, OnStarCollected, playerDied");
    }
    
    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            normalMenu.SetActive(false);
            pause.SetActive(true);
            GameManager.instance.audioSource.Pause();
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackGame()
    {
        GameManager.instance.audioSource.Play();
        Time.timeScale = 1;
        normalMenu.SetActive(true);
        pause.SetActive(false);
        GameManager.instance.marioTransform.animator.SetTrigger("Alive");
    }

    private void Update()
    {
              
        Pause();
    }
    public void PlayerDied()
    {        
        GameOver.gameObject.SetActive(true);
        
        CanRespawn = true;
        normalMenu.SetActive(false);

    }
    public void Revived()
    {
        GameManager.instance.marioTransform.animator.SetTrigger("Alive");
        Time.timeScale = 1;
        GameManager.instance.marioTransform.Heal(10);
        
        GameManager.instance.marioTransform.transform.position = GameManager.instance.worldToGo.position;
        GameManager.instance.marioTransform.audioSource.PlayOneShot(GameManager.instance.peachCastle);
        GameOver.gameObject.SetActive(false);
        normalMenu.SetActive(true);

    }
    public void AddCoins()
    {
        coinsCollected = GameManager.instance.coins;
       
        coins.text = "x " + coinsCollected;
    }
    private void AddStars()     
    {
        starsCollected += 1;

        stars.text = "x " + starsCollected; 
    }
    private void Life(float marioLife)
    {
        
        if (marioLife == 8)
        {
            currentLife = life8;

        }
        else if(marioLife>=7 && marioLife < 8)
        {
            currentLife = life7;

        }
        else if (marioLife >= 6 && marioLife < 7)
        {
            currentLife = life6;

        }
        else if (marioLife >= 5 && marioLife < 6)
        {
            currentLife = life5;

        }
        else if (marioLife >= 4 && marioLife < 5)
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
        else if (marioLife < 0)
        {
            currentLife = life0;
        }

        LIFE.sprite = currentLife;
    }



}
