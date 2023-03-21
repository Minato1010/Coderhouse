using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class MarioController : MonoBehaviour
{

    [SerializeField] protected float speed;
    [SerializeField] protected float rotateSpeed;

    [SerializeField] protected float maxHealth = 8;
    public float currentHealth;
    [SerializeField] protected Animator animator;

    public Rigidbody marioRigidbody;
    [SerializeField] protected float forceAmount;
    [SerializeField] protected float speedLimit;

    [SerializeField] protected Vector3 movingMario;

    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip marioYahoo;
    [SerializeField] protected AudioClip marioYipee;

    [SerializeField] protected AudioClip cameraMoving;

    
    [SerializeField] protected AudioClip marioHurt;
    [SerializeField] protected AudioClip GameOver;
    [SerializeField] protected VolumeProfile VolProf;
    public Transform afterDefeatBobOmb;


    public bool marioIsJumping;

    public bool IsKicking;
    
    protected bool Died = true;
    protected float timeToDestroy;
    protected bool cameraIsMoving = true;
    protected float cameraSoundDelay;

    protected Vector3 idlePosition = new Vector3(0, 0, 0);
    public event Action<float> OnHealthChange;
     public UnityEvent OnDeath;
    protected bool VolumeChanged;
    protected float timeToChangePost;
    protected bool PostChange = true;

    void Start()

    {
        speed = 10f;
        currentHealth = maxHealth;
        Debug.Log("Publisher OnDeath Player, OnHealthChange");
    }
   

    void Update()
    {                  
            Move(GetMoveVector());        
    }
    public float GetHealth()
    {
        return currentHealth;
    }
    
   

        protected void Move(Vector3 MoveDir)
    {
        if (timeToChangePost <= Time.time && PostChange == false)
        {
            if (VolProf.TryGet(out Vignette vignetteComponent))
            {
                vignetteComponent.color.value = Color.black;
                VolumeChanged = false;

            }
            PostChange = true;
        }
        else if (PostChange == true)
        {
            timeToChangePost = 1 + Time.time;
            PostChange = false;
        }
        

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
                IsKicking = true;
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
                IsKicking = true;

            }
            else
            {
                IsKicking = false;
            }
        }
        

    }
   


    private float GetRotationAmount()
    {
       
        return Input.GetAxis("Mouse X");
    }
    private void RotateCharacter(float rotateAmount)
    {

        transform.Rotate(Vector3.up, rotateAmount * rotateSpeed, Space.Self);
        

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

        

    }

    protected Vector3 GetMoveVector()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        movingMario = transform.position;       
        return new Vector3(horizontal, 0, vertical).normalized;

       

    }
    

    public void Heal(int healing)
    {
        healing = 1;
        if (currentHealth < 10)
        {
            currentHealth += healing;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        OnHealthChange?.Invoke(currentHealth);

    }
    public void ReceiveDamage(float damage)
    {

        if (damage != 0)
        {
            if (VolProf.TryGet(out Vignette vignetteComponent))
            {
                if (VolumeChanged != true)
                {
                    vignetteComponent.color.value = Color.red;
                    vignetteComponent.intensity.value = .45f;
                    vignetteComponent.smoothness.value = .14f;
                    VolumeChanged = true;
                }
                
            }

        }
        if (currentHealth > 0)
        {
            currentHealth -= damage;
           
            audioSource.PlayOneShot(marioHurt);
        }
        if (currentHealth <= 0)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(GameOver);
            animator.SetTrigger("Died");
            if (timeToDestroy <=Time.time && Died==false)
            {
                OnDeath?.Invoke();
                Destroy(gameObject);
            }
            else if(Died==true)
            {
                timeToDestroy = Time.time+2.5f;
                Died = false;
            }
            
                
           
        }
        OnHealthChange?.Invoke(currentHealth);

    }



    public void Jumping()
    {
            
            var jumpToFront = Input.GetKeyDown(KeyCode.F);
        
    var collided = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down),  out RaycastHit raycastHitInfo, .8f);

        var jumping = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit raycastInfo, 20f);

        if (jumping && Input.GetKeyDown(KeyCode.G))
        {
            animator.SetTrigger("JumpSmashIn");

        }

        if (collided)
        {
            if (marioIsJumping == true && Input.GetKeyDown(KeyCode.Space) && jumpToFront == false)
            {
                animator.SetTrigger("jumpingHigh");
                marioRigidbody.AddForce(transform.up * 11, ForceMode.Impulse);
                audioSource.PlayOneShot(marioYahoo);
                marioIsJumping = false;

            }

            else if (Input.GetKeyDown(KeyCode.Space) && jumpToFront == false && marioIsJumping==false)
            {
                animator.SetTrigger("jumping");
                animator.SetBool("IsWalking", false);
                animator.SetBool("idle", false);
                marioRigidbody.AddForce(transform.up * forceAmount, ForceMode.Impulse);

                audioSource.PlayOneShot(marioYahoo);
                marioIsJumping = true;
               
            }
            else if (Input.GetKeyDown(KeyCode.Space) && jumpToFront == true)
            {
                marioIsJumping = true;
                audioSource.PlayOneShot(marioYipee);
                animator.SetTrigger("jumpingFront");

                animator.SetBool("IsWalking", false);
                animator.SetBool("idle", false);
                Kicking(false);
                marioRigidbody.AddForce(transform.up * (forceAmount * 1.5f), ForceMode.Impulse);
                marioRigidbody.AddForce(transform.forward * forceAmount, ForceMode.Impulse);
                Kicking(false);
            }
            
        }
        
       
    }

    protected void Kicking(bool Kick)
    {
        if (Input.GetKeyDown(KeyCode.F) && Kick==true)
        {

            
            animator.SetTrigger("Kicking");

        }
    }
    
   
    
   

    
}

