using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuigiController : MarioController
{



    private void Update()
    {
        GetMoveVector();
        Move(GetMoveVector());
        RotateCharacter(GetRotationAmount());
    }
    private float GetRotationAmount()
    {

        return Input.GetAxis("Mouse X");
    }
    private void RotateCharacter(float rotateAmount)
    {

        transform.Rotate(Vector3.up, rotateAmount * rotateSpeed, Space.World);


        if (rotateAmount != 0 && cameraSoundDelay <= Time.time)
        {
            cameraIsMoving = true;
            if (cameraIsMoving == true)
            {
                audioSource.PlayOneShot(cameraMoving);
                cameraSoundDelay = 1f + Time.time;
            }


        }
        else if (rotateAmount != 0 && cameraIsMoving == true)
        {
            cameraIsMoving = false;
            cameraSoundDelay = 1f + Time.time;
        }



    }

}
