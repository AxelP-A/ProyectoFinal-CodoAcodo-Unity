using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
   // Script that triggers after the animation to destroy the object containing the animation.
   void DestroyAnimation(){
      Destroy(gameObject);
   }
}
