using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 enum EnemyStates
{
    Goomba,
    PiranhaPlant

}

public class EnemyScript : MonoBehaviour
{


    [SerializeField] private Transform character;
    [SerializeField] private float speed;
    [SerializeField] private EnemyStates CurrentState;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float goombaDamage = 1f;
    [SerializeField] private Animator goombaAnimator;
    [SerializeField] private Rigidbody goombaRigidbody;
    private bool neverBeTrue = false;


    private float timeToAtack = 2 ;
    private bool Atack;


    void Start()
    {
        speed = 1;
        rotationSpeed = .5f;

    }
    public void SetState()
    {
       
    }
    
    void Update()
    {
      
        switch (CurrentState)
        {
            case EnemyStates.Goomba:


                ExecutePursuit();
                break;
            case EnemyStates.PiranhaPlant:
                LookAtPlayer();
                break;
        }

    }

    private void ExecutePursuit()
    {
        var vectorToChar = character.position - transform.position;
        var distance = vectorToChar.magnitude;
        vectorToChar.Normalize();
        
        if (distance>2)
        {
            transform.position = Vector3.MoveTowards(transform.position, character.position, Time.deltaTime * speed);
            goombaAnimator.SetBool("Walking", true);
            
            transform.LookAt(character.position);

            if (neverBeTrue == true)
            {
                goombaRigidbody.AddForce(transform.up * 1, ForceMode.Impulse);
            }
            
        }
        else
        {
            goombaAnimator.SetBool("idle", true);
        }

    }
    private void LookAtPlayer()
    {
        var vectorToChar = character.position - transform.position;
        
        var newRotation = Quaternion.LookRotation(vectorToChar);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);

        if (vectorToChar.x<=2 || vectorToChar.z<=2)
        {
            
            speed = 1;
        }


               
    }


   
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            
            
           switch (CurrentState)
           {
                case EnemyStates.Goomba:
                    
                    var MarioController = collision.collider.gameObject.GetComponent<MarioController>();

                     



                    if (timeToAtack <= Time.time && Atack==false)
                    {
                        goombaAnimator.SetBool("Atacking", true);
                        goombaAnimator.SetBool("idle", false);
                        goombaAnimator.SetBool("Walking", false);
                        MarioController.ReceiveDamage(goombaDamage);
                        Atack = true;
                    }
                    else if(Atack==true)
                    {
                        timeToAtack = 2 + Time.time;
                        Atack = false;
                    }

                    break;

                case EnemyStates.PiranhaPlant:

                    break;

            }

        }
    }
   


}
