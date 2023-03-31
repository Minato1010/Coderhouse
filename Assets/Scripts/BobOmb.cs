using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobOmb : EnemyScript
{
    private float Health;
    private float Speed;
    private float explosionDamage;
    private bool receiveDamage;
    [SerializeField] private Transform[] patrolPositions;
    [SerializeField] private EnemyData enemy;
    private float timeToExplode;
    
    [SerializeField] private Animator BobOmbAnimator;
   [SerializeField] private Rigidbody bobOmbRigid;
    private bool explode;
    private bool idle;
    private int index=0;
    [SerializeField] private AudioClip explosionSound;

    private void Start()
    {
        Speed = 1.3f;
        goombaDestroyed = null;
        goombaFindMario = null;
        character = GameManager.instance.marioTransform;
        audioSource = GameManager.instance.audioSource;
        Health = enemy.health;
        explosionDamage = enemy.damage;
        goombaAnimator = null;
        index = Random.Range(0, patrolPositions.Length);

    }
   




    private void Update()
    {
        RaycastToPlayer();
        if (GoToPlayer == true)
        {
            var vectorToChar= character.transform.position - transform.position;
            vectorToChar.Normalize();
            Move(vectorToChar);
        }
        else if(GoToPlayer == false)
        {
            Patrol();
        }
        
    }

    private void Patrol()
    {
        var currentWaypoint = patrolPositions[index];
        var currDifference = currentWaypoint.position - transform.position;
        var direction = currDifference.normalized;

        Move(direction);
        
        transform.rotation = Quaternion.LookRotation(currDifference);
        if (currDifference.magnitude <= 1)
        {
            index = Random.Range(0, patrolPositions.Length);
        }
    }
   
    private void Move(Vector3 dir)
    {
        bobOmbRigid.AddForce(transform.up * (-1.6f), ForceMode.Impulse);
        if (GoToPlayer == false)
        {

            transform.position +=  dir*( Time.deltaTime * Speed);
            
        }
        if (GoToPlayer == true)
        {
            Speed = 2f;
            if (idle != true)
            {
                BobOmbAnimator.SetBool("PlayerDetected", true);
                transform.position += dir * (Time.deltaTime * Speed);
                transform.rotation = Quaternion.LookRotation(dir);
            }

            if (explode == true && timeToExplode<=Time.time)
            {
                bobOmbRigid.AddExplosionForce(explosionDamage, transform.position, 3);
                GameManager.instance.audioSource.PlayOneShot(explosionSound);
                if (dir.magnitude <=3)
                {
                    character.ReceiveDamage(explosionDamage);
                    Destroy(gameObject);
                }

            }
            
            else if (explode == false)
            {
                timeToExplode = 4 + Time.time;
                
                explode = true;
                BobOmbAnimator.SetBool("InPosition", true);
                BobOmbAnimator.SetBool("PlayerDetected", false);

            }
        }

    }
    private void ReceiveDamage(float damage)
    {
        Health -= damage;

    }
   

    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.tag == "Player")
        {
            var MarioController = collision.collider.gameObject.GetComponent<MarioController>();
            

            if (MarioController.IsKicking == true)
            {



                if (receiveDamage == true)
                {
                    ReceiveDamage(1);
                    receiveDamage = false;
                }
                else if (receiveDamage == false)
                {
                    receiveDamage = true;
                }

                    
                
            }
        }

    }
}
