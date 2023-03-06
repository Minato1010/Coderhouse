using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip FileSelect;
    public Texture2D hand;
    public Vector2 normalHand;
    [SerializeField] private Texture2D cursor;
    [SerializeField] private LuigiController Luigi;
    [SerializeField] private MarioController Mario;
    private void Start()
    {
        
          audioSource.PlayOneShot(FileSelect);
    }
    private void Update()
    { 
        
            NormalCursor();
        
    }

    public void NormalCursor()
    {
        Cursor.SetCursor(hand,normalHand,CursorMode.Auto);
    }
    

    public void LoadScene(string name)
    {
        Cursor.SetCursor(null, Vector3.zero, CursorMode.ForceSoftware);

        SceneManager.LoadScene(name);
      
       audioSource.Stop();
    }
    
}
