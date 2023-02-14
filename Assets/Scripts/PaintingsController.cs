using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingsController : MonoBehaviour
{
    [SerializeField] private Transform[] newPosition;
    [SerializeField] private int worldToGo = 0;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bobOmb;


    private void Start()
    {
        
    }
    void OnTriggerStay(Collider other)
    {
        Dictionary<int, Transform> newPositions = new Dictionary<int,Transform>();
        newPositions.Add(0, newPosition[0]);
       // newPositions.Add(1, newPosition[1]);
        if (other.gameObject.tag == "Player")
        {

            if (newPositions[worldToGo] == newPosition[worldToGo] )
            {
                if (worldToGo == newPosition.Length)
                {
                    worldToGo = 0;
                    other.gameObject.transform.position = newPosition[worldToGo].position;

                }
                other.gameObject.transform.position = newPosition[worldToGo].position;


            }




        }



    }

    


}
