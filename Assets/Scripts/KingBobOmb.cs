using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using TMPro;

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
    private bool canMove=true;
    private bool CanSpeak;
    private int TextsPassed;
    private Action<int> OnChangeText;
    [SerializeField] AudioClip BobOmbFight;

    [SerializeField] private GameObject PanelUi;
    [SerializeField] private GameObject PanelText;
    [SerializeField] private TMP_Text textTalk;
    private bool atack;
    private float timeAtacking;

    private void Start()
    {
        
        character = GameManager.instance.marioTransform;
        maxHealth = enemyData.health;
        currentHealth = maxHealth;
        damage = enemyData.damage;
        OnChangeText += ChangingText;
        PanelUi = GameManager.instance.PanelUi;
        PanelText = GameManager.instance.PanelText;
        textTalk = GameManager.instance.textTalk;
        textTalk.text = "Soy el Rey Bob-omb, líder de todo lo explotable, Rey de las bombas de todo el mundo!";


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
            Atack();
        }
        else if (atacking == false)
        {
            canMove = true;
            atacking = true;
            timeToAtack = 3f + Time.time;
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
        if (talking == true)
        {
            Move(character.transform.position);
        }
        else
        {
            
            KingAnimator.SetBool("Talking", true);
            TextSpeak();
        }

    }
    private void TextSpeak()
    {
        RaycastToPlayer();
        if (CanSpeak == true)
        {
            GameManager.instance.audioSource.Stop();
            PanelUi.SetActive(false);
            PanelText.SetActive(true);
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
            textTalk.text = "Te ha costado llegar hasta aquí? Te atreviste a poner un pie en la cima Imperial";
        }
        else if (TextsPassed == 2)
        {
            textTalk.text = "Venciste a mis guardias, pero no podrás escapar de mí..";
        }
        
        else if (TextsPassed == 3)
        {

            GameManager.instance.marioTransform.canMove = true;
            PanelText.SetActive(false);
            PanelUi.SetActive(true);
            CanSpeak = false;
            GameManager.instance.audioSource.PlayOneShot(BobOmbFight);
            talking = true;
        }
        

    }
    private void RaycastToPlayer()
    {
        var vectorToChar = character.gameObject.transform.position - transform.position;
        vectorToChar.Normalize();
        var collided = Physics.Raycast(transform.position, vectorToChar, out RaycastHit raycastInfo, 14);
        if (collided && raycastInfo.collider.transform.tag=="Player")
        {
            GameManager.instance.marioTransform.canMove = false;
            transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(transform.position), Quaternion.LookRotation(vectorToChar), 5);

            CanSpeak = true;
        }



    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            if (timeAtacking <= Time.time)
            {
                atack = true;
            }
            
            var marioControl = GameManager.instance.marioTransform;
            if (atack == true)
            {
                marioControl.ReceiveDamage(damage);
                atack = false;
                timeAtacking = 2 + Time.time;
            }




            marioControl.marioRigidbody.AddForce(marioControl.transform.TransformDirection(Vector3.back) * .6f, ForceMode.Impulse);
            marioControl.marioRigidbody.AddForce(marioControl.transform.TransformDirection(Vector3.up) * 1.1f, ForceMode.Impulse);

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && GameManager.instance.marioTransform.IsKicking == true)
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
            GameManager.instance.audioSource.Stop();
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
