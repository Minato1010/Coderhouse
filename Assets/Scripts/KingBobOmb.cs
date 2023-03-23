using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
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
   [SerializeField] private bool atacking;
    private float timeToAtack;
    [SerializeField] private EnemyData enemyData;
    private float damage;
    public UnityEvent OnKingDefeated;
    private bool damageAtacking;
    private bool canMove=true;

    private void Start()
    {
        character = GameManager.instance.marioTransform;
        maxHealth = enemyData.health;
        currentHealth = maxHealth;
        damage = enemyData.damage;

    }
    private void Update()
    {
        Heal();
        TalkWithThePlayer();
    }

    private void Move(Vector3 dir)
    {
        if (canMove != false)
        {
            var vectorToChar = dir - transform.position;
            vectorToChar.Normalize();
            transform.position += vectorToChar * (speed * Time.deltaTime);



            KingAnimator.SetBool("IsWalking", true);
            KingAnimator.SetBool("Talking", false);
           
            transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(transform.position), Quaternion.LookRotation(vectorToChar), 4);
               


            
        }
        if (timeToAtack <= Time.time && atacking == true)
        {
            damageAtacking = true;
            Atack();
        }
        else if (atacking == false)
        {
            canMove = true;
            damageAtacking = false;
            atacking = true;
            timeToAtack = 1.5f + Time.time;
            KingAnimator.SetBool("IsWalking", true);

        }

    }
    private void Atack()
    {
        canMove = false;
        KingAnimator.SetBool("IsWalking", false);

        KingAnimator.SetTrigger("Atacking");
        atacking = false;

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
            Move(character.transform.position);
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
            if (damageAtacking == true)
            {
                marioControl.ReceiveDamage(damage);
                damageAtacking = false;
            }
               
            

            marioControl.marioRigidbody.AddForce(marioControl.transform.TransformDirection(Vector3.back) * .6f, ForceMode.Impulse);
            marioControl.marioRigidbody.AddForce(marioControl.transform.TransformDirection(Vector3.up) * 1.1f, ForceMode.Impulse);

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.GetComponent<MarioController>().IsKicking == true)
        {
            
                ReceiveDamage(2);
                KingAnimator.SetTrigger("Atacked");
                KingAnimator.SetBool("IsWalking", false);
                KingAnimator.SetBool("Talking", false);
            

        }
    }

    private void ReceiveDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0){
            OnKingDefeated?.Invoke();
            Debug.Log("Publisher KingDefeated");
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
