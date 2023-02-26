using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceGoombas : MonoBehaviour
{
    [SerializeField] private List<Transform> goombaWaypoints;
    [SerializeField] private EnemyScript GoombaPrefab;
    private int posToAppear;
    [SerializeField] private int maxGoombas;
    private int goombasAppearing;
     private float timeToAppear=0;
    private bool appear;
    private int i;

    private void Start()
    {
        GoombaPrefab = GameManager.instance.goombaPrefab;
        
    }


    private void Update()
    {
        InstantiateGoombas();


    }
    

    public void InstantiateGoombas()
    {

       

        for (i=0;i<=maxGoombas;i++)
        {

            if (timeToAppear <= Time.time && appear == true && goombasAppearing<=maxGoombas)
            {
                posToAppear = Random.Range(0, goombaWaypoints.Count);
                Instantiate(GoombaPrefab, goombaWaypoints[posToAppear].position, Quaternion.identity);
                appear = false;
                goombasAppearing++;
                GoombaPrefab.character = GameManager.instance.marioTransform;
                GoombaPrefab.audioSource = GameManager.instance.audioSource;

            }
            else if (appear == false)
            {
                timeToAppear = 10f + Time.time;
                appear = true;

            }      
        }
       
       
        
    }
    


    




}
