using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class WhompBoss : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float damage;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private float speed;

    [SerializeField] private Animator animator;
    [SerializeField] private Transform starPosition;
    [SerializeField] private Transform whompTransform;


    private bool atacking;
    private float timeToAtack;
    private bool CanSpeak;
    private bool talking;

    private int TextsPassed;
    [SerializeField] private AudioClip whompBattle;
    private bool falled;

    private Action<int> OnChangeText;
    private TMP_Text textTalk;
    private bool canMove=true;
    [SerializeField] private Rigidbody rigidBody;
    private bool receivingDamage;
    [SerializeField] private AudioClip whompSmashed;

    private void Start()
    {
        health = enemyData.health;
        damage = enemyData.damage;
        OnChangeText += ChangingText;   
        textTalk = GameManager.instance.textTalk;
        

    }

    private void Update()
    {
        if (talking == true)
        {

            RaycastToPlayer();
        }
        else if (talking==false)
        {
            Talking();
        }
        if (health <= 0)
        {
            canMove = false;
            GameManager.instance.PanelText.SetActive(true);
            GameManager.instance.PanelUi.SetActive(false);
            OnChangeText?.Invoke(2);
            health = 0;
            GameManager.instance.marioTransform.canMove = false;
            if (health == 0)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameManager.instance.marioTransform.canMove = true;
                    GameManager.instance.PanelText.SetActive(false);
                    GameManager.instance.PanelUi.SetActive(true);
                    Instantiate(GameManager.instance.Star, starPosition);
                    Destroy(whompTransform.gameObject);
                }

            }

        }


    }
    
    private void Move(Vector3 dir)
    {
        if (canMove == true)
        {

            rigidBody.AddForce(transform.up *-8f);
            atacking = false;
            animator.SetBool("Walk",true);
            transform.position += dir * (Time.deltaTime*speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir),5);

            
            if (timeToAtack <= Time.time)
            {
                animator.SetTrigger("Atack");
                atacking = true;
                canMove = false;
                falled = true;
                receivingDamage = false;
            }


        }
        else if (canMove == false)
        {
            
                
                
            
            if (Time.time>timeToAtack+8f)
            {
                receivingDamage = true;
                animator.SetTrigger("StandUp");
                falled = false;
                canMove = true;
                timeToAtack = 12 + Time.time;


            }


        }
    }
    private void RaycastToPlayer()
    {

        var vectorToChar = GameManager.instance.marioTransform.gameObject.transform.position - transform.position;
        
        var collided = Physics.Raycast(transform.position, vectorToChar, out RaycastHit raycastInfo, 12);
        if (collided && raycastInfo.collider.transform.tag == "Player")
        {

            textTalk.text = "Los humanos son muy malos. Nosotros formamos sus casas, castillos y caminos, y nos pagan caminando sobre nosotros!";
            CanSpeak = true;
        }
        vectorToChar = new Vector3(vectorToChar.x, 0, vectorToChar.z);
        vectorToChar.Normalize();
        if (talking == true)
        {
            Move(vectorToChar);
        }
    }

    private void ReceiveDamage(float damage)
    {
        
        if (health > 0)
        {
            health -= damage;
            GameManager.instance.audioSource.PlayOneShot(whompSmashed);
        }

        

        
        

    }
    private void Talking()
    {
        RaycastToPlayer();

        if (CanSpeak == true)
        {
            var vectorToChar = GameManager.instance.marioTransform.gameObject.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(vectorToChar), 5);

            GameManager.instance.marioTransform.canMove = false;
            animator.SetBool("Talking",true);
            GameManager.instance.PanelText.SetActive(true);
            GameManager.instance.PanelUi.SetActive(false);
            if (Input.GetKeyDown(KeyCode.F))
            {
                OnChangeText?.Invoke(1);
            }
        }
        
      
    }
    private void ChangingText(int c)
    {
        TextsPassed += c;

        if (TextsPassed == 1)
        {
            textTalk.text = "No pueden dar las gracias? No?.. Entonces, te aplastaré solo por diversion";
        }
        if (c == 2)
        {
            textTalk.text = "No! ME HAS VENCIDO! Me he dado cuenta que no soy nada en este mundo. Quédate con tu estrella";

        }

        else if (TextsPassed == 2)
        {

            GameManager.instance.marioTransform.canMove = true;
            GameManager.instance.PanelText.SetActive(false);
            GameManager.instance.PanelUi.SetActive(true);
            CanSpeak = false;
            GameManager.instance.audioSource.Stop();
            GameManager.instance.audioSource.PlayOneShot(whompBattle);
            talking = true;
            animator.SetBool("Talking", false);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(Time.time> timeToAtack + 10)
        {
            atacking = false;
            GameManager.instance.marioTransform.transform.localScale = new Vector3(2, 2, 2);
            
        }
        if (atacking==true && collision.collider.transform.tag == "Player" && GameManager.instance.marioTransform.jumpDown==false)
        {
            GameManager.instance.marioTransform.ReceiveDamage(damage);
            GameManager.instance.marioTransform.transform.localScale = new Vector3(2,.2f, 2);
            atacking = false;
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if(falled == true)
        {
            
            if (other.transform.tag == "Player" && GameManager.instance.marioTransform.jumpDown==true && receivingDamage==false)
            {
                atacking = false;
                
                ReceiveDamage(2);
                receivingDamage = true;

            }
        }
       

        
    }




}
