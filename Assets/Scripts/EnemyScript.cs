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
    [SerializeField] private float goombaHealth = 2f;
    [SerializeField] private Animator goombaAnimator;
    [SerializeField] private Rigidbody goombaRigidbody;
    [SerializeField] private Transform goombaPosition;
    [SerializeField] private float MarioDamage = 1;
    [SerializeField] private bool IsWalking;
    [SerializeField] private Vector3 vectorZero= new Vector3(0,0,0);
    [SerializeField] private bool marioIsJumping;


    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip goombaDestroyed;
    [SerializeField] private AudioClip goombaFindMario;


    [SerializeField] private float enemyDistanceToSee;
    [SerializeField] private LayerMask layerToCollideWith;
    private bool GoToPlayer;
    private bool watchedMario;


    private float goombaScore = 50;
    private float timeToAtack = 2 ;
    private bool Atack;
    private bool beDestroyed = true;

    private float delayToDamage = 1f;
    

    void Start()
    {
        speed = 1;
        rotationSpeed = .5f;
        character = GameManager.instance.marioTransform;

        audioSource = GameManager.instance.audioSource;
        if (enemyDistanceToSee <= 0)
        {
            enemyDistanceToSee = 30f;
        }
    }
    
    
    void Update()
    {
        

        switch (CurrentState)
        {
            case EnemyStates.Goomba:


                ExecuteGoombaPursuit();
                
                break;
            case EnemyStates.PiranhaPlant:
                LookAtPlayer();
                break;
        }

    }


    private void RaycastToPlayer()
    {
        var vectorToChar = character.position - goombaPosition.position;
        vectorToChar.Normalize();
        var collided = Physics.Raycast(goombaPosition.position , vectorToChar, out RaycastHit raycastHitInfo, enemyDistanceToSee, layerToCollideWith );
        if (collided && raycastHitInfo.collider.tag == "Player")
        {

            GoToPlayer = true;

        }
        else
        {
            GoToPlayer = false;
        }


    }

    private void ExecuteGoombaPursuit()
    {


        var vectorToChar = character.position - transform.position;
        vectorToChar.Normalize();

        if (vectorToChar != vectorZero)
        {
            IsWalking = true;
            goombaAnimator.SetBool("Walking", true);


            watchedMario = false;
        }
        else
        {
            IsWalking = false;
            watchedMario = true;
        }

        RaycastToPlayer();
        if (GoToPlayer == true)
        {
        
        if (IsWalking == true && marioIsJumping == false)
        {
            if (watchedMario == true)
            {
                audioSource.PlayOneShot(goombaFindMario);

            }
            goombaRigidbody.AddForce(transform.up * (-2f), ForceMode.Impulse);
            transform.position = Vector3.MoveTowards(transform.position, character.position, Time.deltaTime * speed);

            transform.LookAt(character.position);

            IsWalking = true;

        }
            else
            {
                Patroling();
            }
    }


    }
    private void Patroling()
    {

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
    private void goombaReceiveDamage(float damage )
    {
        
        goombaHealth -= damage;
        
        if (goombaHealth <= 0)
        {
            if (beDestroyed == true)
            {

                audioSource.PlayOneShot(goombaDestroyed);
                Destroy(gameObject);

                GameManager.instance.CharacterScore(goombaScore);
            }
        }
    }

   
     void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            
            
           switch (CurrentState)
           {
                case EnemyStates.Goomba:
                    
                    var marioController = collision.collider.gameObject.GetComponent<MarioController>();


                    if (timeToAtack <= Time.time && Atack==false)
                    {
                        goombaAnimator.SetBool("Atacking", true);
                        goombaAnimator.SetBool("idle", false);
                        goombaAnimator.SetBool("Walking", false);
                        marioController.ReceiveDamage(goombaDamage);
                        Atack = true;
                    }
                    else if(Atack==true)
                    {
                        timeToAtack = 2 + Time.time;
                        Atack = false;
                    }
                    if (marioController.marioIsJumping ==true)
                    {
                        marioIsJumping=true;
                    }
                    else
                    {
                        marioIsJumping = false;
                    }

                    break;

                case EnemyStates.PiranhaPlant:

                    break;

            }

        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            var MarioController = collision.collider.gameObject.GetComponent<MarioController>();
            

            if (MarioController.IsKicking == true)
            {
                if (delayToDamage <= Time.time)
                {


                    goombaReceiveDamage(MarioDamage);
                }
            }
        }
        
    }

    

}
