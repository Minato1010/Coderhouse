using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
 enum EnemyStates
{
    Goomba,
    PiranhaPlant,
    BobOmb

}

public class EnemyScript : MonoBehaviour
{


    public MarioController character;
    [SerializeField] private float speed;
    [SerializeField] private EnemyStates CurrentState;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float Damage = 1f;
    private float health = 2f;
    [SerializeField] protected Animator goombaAnimator;
    [SerializeField] private Animator piranhaAnimator;
    [SerializeField] private Rigidbody goombaRigidbody;
    [SerializeField] private float MarioDamage = 1;
    [SerializeField] private bool IsWalking;
    [SerializeField] private Vector3 vectorZero= new Vector3(0,0,0);
    [SerializeField] private bool marioIsJumping;
    [SerializeField] private Transform[] goombaPositions;
    [SerializeField] private float ThresholdDistance = 2f;
     private int i = 0;
    [SerializeField] private EnemyData enemyData;

    public AudioSource audioSource;
    [SerializeField] protected AudioClip goombaDestroyed;
    [SerializeField] protected AudioClip goombaFindMario;

    [SerializeField] protected float enemyDistanceToSee;
    [SerializeField] protected LayerMask layerToCollideWith;
    public bool GoToPlayer;
    private bool watchedMario;

    private bool wayToInit = true;
    private float timeToAtack = 2 ;
    private bool Atack;
    public bool beDestroyed = true;
   
    private float delayToDamage = 1f;
    public static event Action OnEnemyDied;
    
    void Start()
    {
        health = enemyData.health;
        Damage = enemyData.damage;

        character = GameManager.instance.marioTransform;

        audioSource = GameManager.instance.audioSource;
        goombaDestroyed = GameManager.instance.goombaDestroyed;
        goombaFindMario = GameManager.instance.goombaFindMario;
        goombaPositions = GameManager.instance.goombaPositions;
        speed = 1;
        rotationSpeed = .5f;
        switch (CurrentState)
        {
            case EnemyStates.PiranhaPlant:
                goombaAnimator = null;
                goombaPositions = null;
                goombaRigidbody = null;
                goombaDestroyed = null;
                goombaFindMario = null;

                break;
            
        }
        Debug.Log("OnEnemyDied Publisher");
        
    }

    
    void Update()
    {

       




        switch (CurrentState)
        {
            case EnemyStates.Goomba:

                RaycastToPlayer();
                if (GoToPlayer == true)
                {
                    ExecuteGoombaPursuit();
                }
                else
                {
                    Patrol();
                }
                
                break;
            case EnemyStates.PiranhaPlant:
                LookAtPlayer();
                break;
        }

    }


    protected void RaycastToPlayer()
    {
       
        var vectorToChar = character.gameObject.transform.position - transform.position;
        vectorToChar.Normalize();
        var collided = Physics.Raycast(transform.position , vectorToChar, out RaycastHit raycastHitInfo, enemyDistanceToSee, layerToCollideWith );
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


        var vectorToChar = character.gameObject.transform.position - transform.position;
        vectorToChar.Normalize();

        if (vectorToChar != vectorZero)
        {
            IsWalking = true;
            


            watchedMario = false;
        }
        else
        {
            IsWalking = false;
            watchedMario = true;
        }

        

        
        if (IsWalking == true && marioIsJumping == false)
        {
            if (watchedMario == true)
            {
                audioSource.PlayOneShot(goombaFindMario);

            }
            goombaRigidbody.AddForce(transform.up * (-2f), ForceMode.Impulse);
            transform.position = Vector3.MoveTowards(transform.position, character.gameObject.transform.position, Time.deltaTime * speed);
            
            transform.LookAt(character.gameObject.transform.position);

            IsWalking = true;

        }
            
    }
       

    
    

    private void Move(Vector3 direction)
    {
        goombaAnimator.SetBool("Walking", true);
        if (GoToPlayer == false)
        {
            transform.position += direction * (speed * Time.deltaTime);
            goombaRigidbody.AddForce(transform.up * (-2f), ForceMode.Impulse);
            
        }
        else
        {
            ExecuteGoombaPursuit();
        }
    }

    private void Patrol()
    {
        var wayToGo = UnityEngine.Random.Range(0, 5);
        var currentWaypoint = goombaPositions[i];
        var currDifference = currentWaypoint.position - transform.position;
        var direction = currDifference.normalized;
        Move(direction);
        var currDistance = currDifference.magnitude;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currDifference), Time.deltaTime * rotationSpeed); 
        if (currDistance <= ThresholdDistance)
        {
            if (wayToGo <= 2)
            {
                NextWaypoint();
            }
            else
            {
                LessWaypoint();
            }
        }
    }

    private void NextWaypoint()
    {
        i++;
        if (i >4)
        {
            i = 0;
        }
    }
    private void LessWaypoint()
    {
        if (wayToInit==true)
        {
            i = 4;
            wayToInit = false;
        }
        i--;
        if (i < 0)
        {
            i = 4;
        }
    }

private void LookAtPlayer()
    {
        var vectorToChar = character.gameObject.transform.position - transform.position;
        
        var newRotation = Quaternion.LookRotation(vectorToChar);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);

        if (vectorToChar.magnitude <= 3)
        {
            piranhaAnimator.SetTrigger("Atacking");

        }
        


               
    }
    private void ReceiveDamage(float damage )
    {
        switch (CurrentState)
        {
            case EnemyStates.PiranhaPlant:
                piranhaAnimator.SetTrigger("Atacked");
                break;
        }
        health -= damage;
       
        if (health <= 0)
        {
            if (beDestroyed == true)
            {
                OnEnemyDied?.Invoke();

                switch (CurrentState)
                {
                    case EnemyStates.PiranhaPlant:
                        piranhaAnimator.SetTrigger("Died");
                        break;
                    case EnemyStates.Goomba:
                audioSource.PlayOneShot(goombaDestroyed);
                        break;
                }
                



            }
            Destroy(gameObject);
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
                        marioController.ReceiveDamage(Damage);
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

               

            }

        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.gameObject.tag == "Player")
        {
            var MarioController = collision.collider.gameObject.GetComponent<MarioController>();
            switch (CurrentState)
            {
                case EnemyStates.PiranhaPlant:

                    MarioController.ReceiveDamage(Damage);

                    break;
            }

            if (MarioController.IsKicking == true)
            {


                if (delayToDamage <= Time.time)
                {


                    ReceiveDamage(MarioDamage);
                }
            }
        }
        
    }

    

}
