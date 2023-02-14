using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorChangeLocation : MonoBehaviour
{
    public enum door
    {
        door1,
        door2,
        door3,
        door4
    }

    [SerializeField] private Transform door1Position;
    [SerializeField] private Transform door2Position;
    [SerializeField] private door currentDoor;




    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.F))
        {
            switch (currentDoor)
            {
                case door.door1:
                    
                    other.gameObject.transform.position = door2Position.position + new Vector3(0, 0, -2);
                     


                    break;
                case door.door2:
                    // if (other.gameObject.tag == "Player")
                    //{
                    other.gameObject.transform.position = door1Position.position;
                    // }
                    break;

            }
        }
    }
}
