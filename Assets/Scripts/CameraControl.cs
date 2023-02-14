using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
   [SerializeField] private CinemachineVirtualCamera camera1;
   [SerializeField] private CinemachineVirtualCamera camera2;
    private float cameraChanger = .9f;
    private int cameraChanges = 0;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cameraClick;



    private void Start()
    {
        cameraChanger += Time.time;
    }
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
            if (cameraChanger <= Time.time)
            {
                if(cameraChanges == 0)
                {
                    TurnOnCamera(camera2, camera1);

                    cameraChanges = 1;
                    audioSource.PlayOneShot(cameraClick);
                    cameraChanger = Time.time +.9f;
                }
                else if (cameraChanges == 1)
                {
                    TurnOnCamera(camera1, camera2);
                    cameraChanges = 0;
                    audioSource.PlayOneShot(cameraClick);
                    cameraChanger = .9f + Time.time;
                }
               
            }

        }
        



    }


    private void TurnOnCamera(CinemachineVirtualCamera camTurnOn, CinemachineVirtualCamera camTurnOff)
    {
        camTurnOn.gameObject.SetActive(true);
        camTurnOff.gameObject.SetActive(false);

    }
}
