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
    private Action<int> OnChangeText;
    [SerializeField] Rigidbody koopaRigidbody;
    [SerializeField] Animator koopaAnimator;
    private float speed = 13;
    public static bool playMusic;
    private bool notMove;

    private bool textFinishRace;

    private void Start()
    {
        KoopaPositions = GameManager.instance.KoopaBossPositions;
        OnChangeText += ChangingText;
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
        else if (TextsPassed==4)
        {
            GameManager.instance.audioSource.Stop();
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.raceMusic);
            talk = true;


        }
        else if(c==5)
        {
            PanelUi.SetActive(false);
            PanelText.SetActive(true);
            textTalk.text = "Puedes volver a intentarlo";
            if (Input.GetKeyDown(KeyCode.F))
            {
                OnChangeText?.Invoke(7);
            }

            

        }
        else if (c == 7)
        {
            PanelUi.SetActive(true);
            PanelText.SetActive(false);

            Instantiate(GameManager.instance.KoopaBoss);
            GameManager.instance.KoopaBoss.transform.position = GameManager.instance.koopaPosition.position;
            FinishRace.winner = false;
            GameManager.instance.marioTransform.transform.position =GameManager.instance.worldToGo.position;
            GameManager.instance.audioSource.Stop();
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.peachCastle);
            Destroy(this.gameObject);
        }
        else if (c == 6)
        {
            PanelUi.SetActive(false);
            PanelText.SetActive(true);
            textTalk.text = "Vaya que eres rapido, aquí tienes una recompensa";
            if (Input.GetKeyDown(KeyCode.F))
            {
                OnChangeText?.Invoke(8);
            }

        }
        else if (c == 8)
        {
            Instantiate(GameManager.instance.Star, GameManager.instance.StarPosition);
            PanelUi.SetActive(true);
            PanelText.SetActive(false);
            Destroy(this.gameObject);
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
            
            

            if (currDifference.magnitude <= 1f && i<=16)
            {
                if (i == 16)
                {
                    notMove = true;
                    koopaAnimator.SetBool("RaceFinish", true);
                    koopaAnimator.SetBool("Run", false);
                    if (FinishRace.raceWiner == "Koopa")
                    {
                        KoopaWin(true);
                    }
                    else if (FinishRace.raceWiner == "Mario")
                    {
                        KoopaWin(false);
                    }
                }
                if (i != 16)
                {
                    NextWaypoint();
                }

            }

        }

    }
    private void KoopaWin(bool winner)
    {
        if (winner == true)
        {
            OnChangeText?.Invoke(5);
        }
        else if (winner == false)
        {
            OnChangeText?.Invoke(6);
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
            OnChangeText?.Invoke(1);
        }        
    }






}
