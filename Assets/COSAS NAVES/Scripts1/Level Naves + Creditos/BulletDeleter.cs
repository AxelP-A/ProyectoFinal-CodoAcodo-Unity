using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDeleter : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag.Equals("PBullet") || col.tag.Equals("EBullet") )
        {
            Destroy(col.gameObject);
            //Debug.Log("Destruido");
        }
    }
}
