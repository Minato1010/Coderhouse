using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class KoopaBossController : MonoBehaviour
{
    [SerializeField] private List<Transform> KoopaPositions;
    private bool talk;
    private int i=0;
    [SerializeField] private GameObject PanelUi;
    [SerializeField] private GameObject PanelText;
    [SerializeField] private TMP_Text textTalk;
    private int TextsPassed;
    private Action<int> ChangeText;
    [SerializeField] Rigidbody koopaRigidbody;
    [SerializeField] Animator koopaAnimator;
    private float speed = 16;
    private bool playMusic;
    private bool notMove;

    private void Start()
    {
        KoopaPositions = GameManager.instance.KoopaBossPositions;
        ChangeText += ChangingText;
        PanelUi = GameManager.instance.PanelUi;
        PanelText = GameManager.instance.PanelText;
        textTalk = GameManager.instance.textTalk;

    }




    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag=="Player" && talk == false)
        {
            transform.LookAt(other.transform);
            

                GameManager.instance.marioTransform.canMove = false;
            
            Text();
            GameManager.instance.marioTransform.animator.SetBool("IsWalking",false);
            GameManager.instance.marioTransform.animator.SetBool("idle", true);
        }
        else if (talk == true && other.gameObject.tag=="Player")
        {
            
            PanelText.SetActive(false);
            PanelUi.SetActive(true);
            koopaAnimator.SetBool("Run",true);
            Destroy(gameObject.GetComponent<BoxCollider>());
            GameManager.instance.marioTransform.canMove = true;
        }

        
    }
    private void ChangingText(int c)
    {
        TextsPassed += c;
        if (TextsPassed==1)
        {
            textTalk.text = "Sabes?, pareces rapido. Pero que tan rapido eres? Podras ganarme a mi?";
        }
        else if (TextsPassed == 2)
        {
            textTalk.text = "No lo creo pues soy Koopa el rapido. Quieres intentarlo? ";
        }
        else if (TextsPassed == 3)
        {
            textTalk.text = "La carrera sera hasta la cima, donde estaba el rey. Que me dices Mario?";
        }
        if (TextsPassed==4)
        {
            talk = true;

        }

    }
    private void Update()
    {
        if (talk == true)
        {
            RunRace();
        }
        

    }

    private void RunRace()
    {
        if (playMusic == false)
        {
            GameManager.instance.audioSource.Stop();
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.raceMusic);
            playMusic = true;
        }




        var currentWaypoint = KoopaPositions[i];
        var currDifference = currentWaypoint.position - transform.position;
        var direction = currDifference.normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currDifference), 1f);

        Move(direction);
        

        if (i <= KoopaPositions.Count)
        {
            
            if (i == 16)
            {
                koopaAnimator.SetBool("RaceFinish", true);
                koopaAnimator.SetBool("Run", false);
                notMove = true;
            }

            if (currDifference.magnitude <= 1f && i<16)
            {

                NextWaypoint();

            }

        }

    }
    private void Move(Vector3 direction)
    {
        if (notMove != true)
        {
            transform.position += direction * (speed * Time.deltaTime);

        }
    }
    private void NextWaypoint()
    {
        koopaRigidbody.AddForce(Vector3.down * (.8f), ForceMode.Impulse);

        i++;


    }



    private void Text()
    {
        PanelUi.SetActive(false);
        PanelText.SetActive(true);
        if (TextsPassed == 0)
        {
            textTalk.text = "Oye, Mario! Es cierto que le ganaste a el Rey? Genial, Eres muy fuerte";
        }
        if (Input.GetKeyDown(KeyCode.F) && talk==false)
        {
            ChangeText?.Invoke(1);
        }        
    }






}
