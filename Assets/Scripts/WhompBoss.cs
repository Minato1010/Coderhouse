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


    private bool atacking;
    private float timeToAtack;
    private bool CanSpeak;
    private bool talking;
    private bool LastText;

    private int TextsPassed;
    [SerializeField] private AudioClip whompBattle;

    private Action<int> OnChangeText;
    private TMP_Text textTalk;
    private bool canMove=true;
    private void Start()
    {
        health = enemyData.health;
        damage = enemyData.damage;
        OnChangeText += ChangingText;   
        textTalk = GameManager.instance.textTalk;
        textTalk.text = "Los humanos son muy malos. Nosotros formamos sus casas, castillos y caminos, y nos pagan caminando sobre nosotros!";


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
        

    }
    private void Move(Vector3 dir)
    {
        if (canMove == true)
        {

//            rigidBody.AddForce(Vector3.down *8f);
            atacking = false;
            animator.SetBool("Walk",true);
            transform.position += dir * (Time.deltaTime*speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir),5);

            
            if (timeToAtack <= Time.time)
            {
                animator.SetTrigger("Atack");

                canMove = false;
            }


        }
        else if (canMove == false)
        {
            
                
                atacking = true;
            
            if (Time.time>timeToAtack+8f)
            {
                
                canMove = true;
                timeToAtack = 12 + Time.time;


            }


        }
    }
    private void RaycastToPlayer()
    {

        var vectorToChar = GameManager.instance.marioTransform.gameObject.transform.position - transform.position;
        vectorToChar = new Vector3(vectorToChar.x, 0, vectorToChar.z);
        vectorToChar.Normalize();
        var collided = Physics.Raycast(transform.position, vectorToChar, out RaycastHit raycastInfo, 12);
        if (collided && raycastInfo.collider.transform.tag == "Player")
        {
           
            transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(transform.position), Quaternion.LookRotation(vectorToChar), 5);

            CanSpeak = true;
        }
        if (talking == true)
        {
            Move(vectorToChar);
        }
    }

    private void ReceiveDamage(float damage)
    {
        health -= damage;

        if (health <0)
        {
            GameManager.instance.PanelText.SetActive(true);
            GameManager.instance.PanelUi.SetActive(false);
            OnChangeText?.Invoke(2);

            if (Input.GetKeyDown(KeyCode.F))
            {
                GameManager.instance.PanelText.SetActive(false);
                GameManager.instance.PanelUi.SetActive(true);
                Instantiate(GameManager.instance.Star, starPosition);
                Destroy(gameObject);
            }

        }
       
    }
    private void Talking()
    {
        RaycastToPlayer();

        if (CanSpeak == true)
        {
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
        if(Time.time> timeToAtack + 8)
        {
            atacking = false;
            GameManager.instance.marioTransform.transform.localScale = new Vector3(2, 2, 2);
            
        }
        if (atacking==true && collision.collider.transform.tag == "Player")
        {
            GameManager.instance.marioTransform.ReceiveDamage(damage);
            GameManager.instance.marioTransform.transform.localScale = new Vector3(2,.2f, 2);
            atacking = false;
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.collider.transform.tag=="Player" && atacking==true && GameManager.instance.marioTransform.jumpDown==true)
        {
            ReceiveDamage(2);

        }

        
    }




}
