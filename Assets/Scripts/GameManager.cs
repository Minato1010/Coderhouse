using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float score;
    public Transform marioTransform;
    public AudioSource audioSource;
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

}
