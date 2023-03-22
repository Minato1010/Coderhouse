using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingsController : MonoBehaviour
{
    [SerializeField] private Transform[] newPosition;
    public int worldToGo = 0;
    public AudioSource audioSource;
    public AudioClip audioPlace;
    public static PaintingsController paintingController;
    public Transform[] currentPosition;


    
    void OnTriggerStay(Collider other)
    {
       
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        Dictionary<int, Transform> newPositions = new Dictionary<int,Transform>();
        newPositions.Add(0, newPosition[0]);
        newPositions.Add(1, newPosition[1]);
        if (other.gameObject.tag == "Player")
        {
            
            if (newPositions[worldToGo] == newPosition[worldToGo] )
            {
                GameManager.instance.worldToGo = currentPosition[worldToGo];   
                    audioSource.PlayOneShot(audioPlace);               

                other.gameObject.transform.position = newPosition[worldToGo].position;
               
                
            }




        }



    }

    


}
