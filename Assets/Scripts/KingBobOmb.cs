using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingBobOmb : MonoBehaviour
{
    [SerializeField] private Animator KingAnimator;
    [SerializeField] private float currentHealth;
    private float maxHealth=10;
    private float timeToHeal;
    private bool healing;
    [SerializeField] private float speed;
    private bool talking;
    private MarioController character;
    private float timeToAtack;
    private bool atacking;
    private void Start()
    {
        character = GameManager.instance.marioTransform;
        currentHealth = maxHealth;
    }
    private void Update()
    {
        Heal();
        TalkWithThePlayer();
    }

    private void Move(Vector3 dir)
    {
        var vectorToChar = dir - transform.position;
        vectorToChar.Normalize();
        transform.position += vectorToChar*(speed*Time.deltaTime);

        KingAnimator.SetBool("IsAtacked", false);
        KingAnimator.SetBool("IsWalking", true);
        KingAnimator.SetBool("IsAtacking", false);
        KingAnimator.SetBool("Talking", false);
       
    }
    private void TalkWithThePlayer()
    {
        if (talking == false)
        {
            RaycastToPlayer();
        }
        else
        {
            KingAnimator.SetBool("Talking", true);
        }

    }
    private void RaycastToPlayer()
    {
        var vectorToChar = character.gameObject.transform.position - transform.position;
        vectorToChar.Normalize();
        var collided = Physics.Raycast(transform.position, vectorToChar, out RaycastHit raycastInfo, 20);
        if (collided)
        {
            Move(character.gameObject.transform.position);
        }
        else
        {
            KingAnimator.SetBool("IsWalking", false);
        }



    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            var marioControl = collision.collider.GetComponent<MarioController>();
            var vectorToChar = character.transform.position - transform.position;
            if (vectorToChar.magnitude <= .5f &&  timeToAtack <= Time.time && atacking==true)
            {
                marioControl.ReceiveDamage(1);
                atacking = false;
                KingAnimator.SetBool("IsAtacked", false);
                KingAnimator.SetBool("IsWalking", false);
                KingAnimator.SetBool("IsAtacking", true);
                KingAnimator.SetTrigger("Atacking");
                KingAnimator.SetBool("Talking", false);

            }
            else if(atacking==false)
            {
                atacking = true;
                timeToAtack = 2+ Time.time;
                if (marioControl.IsKicking == false)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(vectorToChar.normalized), Time.deltaTime * .6f); ;

                }
            }
            marioControl.marioRigidbody.AddForce(marioControl.transform.TransformDirection(Vector3.back) * 5, ForceMode.Impulse);




        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var marioControl = other.GetComponent<MarioController>();
            if (marioControl.IsKicking == true)
            {
                ReceiveDamage(3); 
                KingAnimator.SetBool("IsAtacked", true);
                KingAnimator.SetBool("IsWalking", false);
                KingAnimator.SetBool("IsAtacking", false);
                KingAnimator.SetBool("Talking", false);
            }

        }
    }

    private void ReceiveDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0){
            GameManager.instance.KingBobOmbDefeated();
            Debug.Log("HOLA");
            Destroy(gameObject);
        }
    }
    private void Heal()
    {
        if (timeToHeal <= Time.time && healing==true)
        {
            currentHealth += 1;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            healing = false;
        }
        else if (healing == false)
        {
            timeToHeal = 10f + Time.time;
            healing = true;
        }
    }




}
