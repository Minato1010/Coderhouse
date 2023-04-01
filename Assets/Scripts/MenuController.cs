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
    public string stereoPanNamePrefs;
    public string volumeNamePrefs;
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
        PlayerPrefs.SetInt(stereoPanNamePrefs, Panstereo);
        PlayerPrefs.SetFloat(volumeNamePrefs, volumeMusic);
    }

    public void SoundVolume()
    {
        audioSource.volume=volume.value;
        PlayerPrefs.SetFloat(volumeNamePrefs,volumeMusic);
    }
    public void StereoPan()
    {
        audioSource.panStereo = stereoPan.value*-1;
        PlayerPrefs.SetInt(stereoPanNamePrefs, Panstereo);
    }


}
