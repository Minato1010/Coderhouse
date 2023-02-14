using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float maxHealth = 10;
    [SerializeField] private float currentHealth;
    [SerializeField] private Animator animator;

    [SerializeField] private Rigidbody marioRigidbody;
    [SerializeField] private float forceAmount;
    [SerializeField] private float speedLimit;

    [SerializeField] private Vector3 movingMario;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip marioYahoo;
    [SerializeField] private AudioClip marioYipee;

    [SerializeField] private AudioClip cameraMoving;

    
    [SerializeField] private AudioClip marioHurt;
    [SerializeField] private AudioClip GameOver;
    
    private bool Died = true;
    private float timeToDestroy;
    private bool cameraIsMoving = true;
    private float cameraSoundDelay;

    protected Vector3 idlePosition = new Vector3(0, 0, 0);
    void Start()

    {
        speed = 10f;
        currentHealth = maxHealth;
    }
   

    void Update()
    {
        if (Time.time>5.5f)
        {
            
            Move(GetMoveVector());
            

        }  

        

        
    }


    private void Move(Vector3 MoveDir)
    {
       

        var transform1 = transform;
        transform1.position += (MoveDir.x * transform1.right + MoveDir.z * transform1.forward) * (speed * Time.deltaTime);        

        RotateCharacter(GetRotationAmount());
        
        var MarioIsMoving = transform1.position - movingMario;

        if(MarioIsMoving!= idlePosition)
        {
            animator.SetBool("IsWalking",true);
            animator.SetBool("idle", false);
            Jumping();
            if (Input.GetKeyDown(KeyCode.F))
            {
                Kicking(true);
            }
        }
       
        else 
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("idle", true);
            Jumping();
            if (Input.GetKeyDown(KeyCode.F))
            {
                Kicking(true);
            }
        }
        

    }

    private float GetRotationAmount()
    {
       
        return Input.GetAxis("Mouse X");
    }
    private void RotateCharacter(float rotateAmount)
    {
       
        

        if (rotateAmount != 0 && cameraSoundDelay<=Time.time)
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

        transform.Rotate(Vector3.up, rotateAmount * rotateSpeed, Space.Self);

    }

    private Vector3 GetMoveVector()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        movingMario = transform.position;       
        return new Vector3(horizontal, 0, vertical).normalized;

       

    }
    

    public void Heal(float healing)
    {

        if (currentHealth < 10)
        {
            currentHealth += healing;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }

    }
    public void ReceiveDamage(float damage)
    {

       
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            audioSource.PlayOneShot(marioHurt);
        }
        if (currentHealth <= 0)
        {
           
            audioSource.PlayOneShot(GameOver);
            animator.SetTrigger("Died");
            if (timeToDestroy <=Time.time && Died==false)
            {
                
                Destroy(gameObject);
            }
            else if(Died==true)
            {
                timeToDestroy = Time.time+4.5f;
                Died = false;
            }
            
                
           
        }

    }
    private bool IsWithinSpeedLimit()
    {
        var l_currentYVelocity = Mathf.Abs(marioRigidbody.velocity.y);
        return l_currentYVelocity < speedLimit;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            ReceiveDamage(.7f);
            
        }
    }

    private void Jumping()
    {
        if (IsWithinSpeedLimit())
        {
            
            var jumpToFront = Input.GetKeyDown(KeyCode.F);
            if (Input.GetKeyDown(KeyCode.Space) && jumpToFront==false)
            {
                animator.SetTrigger("jumping");
                animator.SetBool("IsWalking", false);
                animator.SetBool("idle", false);
                marioRigidbody.AddForce(transform.up * forceAmount, ForceMode.Impulse);
               
                audioSource.PlayOneShot(marioYahoo);

            } 
            else if (Input.GetKeyDown(KeyCode.Space) && jumpToFront)
            {
                audioSource.PlayOneShot(marioYipee);
                animator.SetTrigger("jumpingFront");
                Debug.Log("Saltando");
                animator.SetBool("IsWalking", false);
                animator.SetBool("idle", false);
                marioRigidbody.AddForce(transform.up * forceAmount, ForceMode.Impulse);
                marioRigidbody.AddForce(transform.forward * forceAmount, ForceMode.Impulse);
                Kicking(false);
            }
        }
    }

    private void Kicking(bool Kick)
    {
        if (Input.GetKeyDown(KeyCode.F) && Kick==true)
        {

            
            animator.SetTrigger("Kicking");

        }
    }

    private void Run()
    {

        speed = 15f;

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            ReceiveDamage(10f);
        }
    }

    
}

