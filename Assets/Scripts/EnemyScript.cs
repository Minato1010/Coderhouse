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


    public MarioController character;
    [SerializeField] private float speed;
    [SerializeField] private EnemyStates CurrentState;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float goombaDamage = 1f;
    public float goombaHealth = 2f;
    [SerializeField] private Animator goombaAnimator;
    [SerializeField] private Rigidbody goombaRigidbody;
    [SerializeField] private Transform goombaPosition;
    [SerializeField] private float MarioDamage = 1;
    [SerializeField] private bool IsWalking;
    [SerializeField] private Vector3 vectorZero= new Vector3(0,0,0);
    [SerializeField] private bool marioIsJumping;
    [SerializeField] private Transform[] goombaPositions;
    [SerializeField] private float ThresholdDistance = 2f;
    [SerializeField]private int i = 0;

    public AudioSource audioSource;
    [SerializeField] private AudioClip goombaDestroyed;
    [SerializeField] private AudioClip goombaFindMario;

    [SerializeField] private float enemyDistanceToSee;
    [SerializeField] private LayerMask layerToCollideWith;
    private bool GoToPlayer;
    private bool watchedMario;

    private bool wayToInit = true;
    private float goombaScore = 50f;
    private float timeToAtack = 2 ;
    private bool Atack;
    public bool beDestroyed = true;
   
    private float delayToDamage = 1f;
    
    void Start()
    {


        character = GameManager.instance.marioTransform;

        audioSource = GameManager.instance.audioSource;
        goombaDestroyed = GameManager.instance.goombaDestroyed;
        goombaFindMario = GameManager.instance.goombaFindMario;
        goombaPositions = GameManager.instance.goombaPositions;
        speed = 1;
        rotationSpeed = .5f;
        
        
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


    private void RaycastToPlayer()
    {
       
        var vectorToChar = character.gameObject.transform.position - goombaPosition.position;
        vectorToChar.Normalize();
        var collided = Physics.Raycast(goombaPosition.position , vectorToChar, out RaycastHit raycastHitInfo, enemyDistanceToSee, layerToCollideWith );
        if (collided && raycastHitInfo.collider.tag == "Player")
        {
            goombaAnimator.SetBool("Walking", true);
            GoToPlayer = true;

        }
        else
        {
            goombaAnimator.SetBool("Walking", false);
            goombaAnimator.SetBool("idle", true);
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
        if (GoToPlayer == false)
        {
            transform.position += direction * (speed * Time.deltaTime);
            goombaRigidbody.AddForce(transform.up * (-2f), ForceMode.Impulse);
            goombaAnimator.SetBool("Walking", true);
        }
        else
        {
            ExecuteGoombaPursuit();
        }
    }

    private void Patrol()
    {
        var wayToGo = Random.Range(0, 5);
        var currentWaypoint = goombaPositions[i];
        var currDifference = currentWaypoint.position - transform.position;
        var direction = currDifference.normalized;
        Move(direction);
        var currDistance = currDifference.magnitude;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currDifference), Time.deltaTime * rotationSpeed); ;
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
                beDestroyed = false;
                if (beDestroyed == false)
                {
                    GameManager.instance.CharacterScore(goombaScore);
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
