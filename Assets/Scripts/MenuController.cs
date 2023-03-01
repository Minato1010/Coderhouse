using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{


    private int coins;

    private void Update()
    {
        string  hola =""+ coins;
    }


    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }


}
