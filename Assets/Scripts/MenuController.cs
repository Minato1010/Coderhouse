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
    public GameObject fileSelecting;
    public GameObject changeSound;
    public GameObject changeControls;
    public Slider volume;
    public Slider stereoPan;
    public int Panstereo;
    public float volumeMusic;

    private void Start()
    {
        
          audioSource.PlayOneShot(FileSelect);
    }
    public void ChangeVolume()
    {
        fileSelecting.SetActive(false);
        changeSound.SetActive(true);
    }
    public void ChangeControls()
    {
        fileSelecting.SetActive(false);
        changeControls.SetActive(true);
    }
    public void BackMenu()
    {
        fileSelecting.SetActive(true);
        changeControls.SetActive(false);
        changeSound.SetActive(false);
        
    }

    

    public void LoadScene(string name)
    {
        Cursor.SetCursor(null, Vector3.zero, CursorMode.ForceSoftware);

        SceneManager.LoadScene(name);
      
       audioSource.Stop();
    }

    public void SoundVolume()
    {
        audioSource.volume=volume.value;
        PlayerPrefs.SetFloat("Volume",volumeMusic);
    }
    public void StereoPan()
    {
        audioSource.panStereo = stereoPan.value*-1;
        PlayerPrefs.SetInt("PanStereo", Panstereo);
    }


}
