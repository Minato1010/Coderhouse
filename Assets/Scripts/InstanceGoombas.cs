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
    [SerializeField] private float timeToAppear = 15f;
    private bool appear;

    private void Start()
    {
        GoombaPrefab = GameManager.instance.goombaPrefab;
        
    }


    private void Update()
    {
        InstantiateGoombas();
    }
    

    private void InstantiateGoombas()
    {

        for (int i=0;i<=maxGoombas;i++)
        {

            if (timeToAppear <= Time.time && appear == true && goombasAppearing < maxGoombas)
            {
                posToAppear = Random.Range(0, goombaWaypoints.Count);
                Instantiate(GoombaPrefab, goombaWaypoints[posToAppear].position, Quaternion.identity);
                Debug.Log("Aparecio goombaa");
                appear = false;
                goombasAppearing++;


            }
            else if (appear == false)
            {
                timeToAppear = 10f + Time.time;
                appear = true;

            }
            
            

        }
       
        
    }
    




}
