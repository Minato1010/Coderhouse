using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public Vector3 myPosition;
    [SerializeField] private float speed;
    [SerializeField] private float health = 10;
    [SerializeField] private float rotationSpeed;







    void Start()

    {
        speed = 1;
        rotationSpeed = 1f;
        myPosition = transform.position;
    }

    void Update()
    {

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (health > 0)
        {

            Move(new Vector3(horizontal, 0, vertical));


        }
    }
    private void Move(Vector3 MoveDirection)
    {
        
        
        myPosition += MoveDirection * speed * Time.deltaTime;
        transform.position = myPosition;
       
        var newRotation = Quaternion.LookRotation(MoveDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);

    }
    




}
