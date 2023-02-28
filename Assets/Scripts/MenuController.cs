using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : CoinScript
{
    [SerializeField] private Scene MainScene;

    private void Start()
    {
        MainScene = SceneManager.GetSceneByName("MainScene");
    }

    public void teleportScene()
    {
        MainScene = SceneManager.GetSceneByName("MainScene");

        SceneManager.LoadScene("MainScene");

    }
   


}
