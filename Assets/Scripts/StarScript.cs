using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour
{

    public int StarCollected=1;
   

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.StarCollected(StarCollected);
            Destroy(gameObject);
        }
    }

}
