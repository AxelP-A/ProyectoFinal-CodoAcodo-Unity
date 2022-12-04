using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomb : MonoBehaviour
{
    public void SelfDestroy() // X animator destruye el objeto al terminar.
    {
        Destroy(gameObject);        
    }
}
