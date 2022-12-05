using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public CanvasGroup myCG;
    public GameObject player;
    CharacterController2D characterController2d;
    private bool blackScreen = false;

    public static int deathCount = 0;
    public static GameManager Instance = null;

    void Awake()
    {
        if (Instance == null)
		{
			Instance = this;
            characterController2d = player.GetComponent<CharacterController2D>();
            StartGame();
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad (gameObject);
    }
     
     void FixedUpdate ()
     {
       /* if(deathCount > 0)
        {
            myCG.alpha = 0;
        }*/
         if (blackScreen)
         {
             myCG.alpha = myCG.alpha - Time.deltaTime;
             if (myCG.alpha <= 0)
             {
                 myCG.alpha = 0;
                 blackScreen = false;
                characterController2d.SetIfCanMove(true);
                characterController2d.SetIfCanAttack(true);
             }
         }
     }
     
    void StartGame()
    {
        myCG.alpha = 1;
        characterController2d.SetIfCanMove(false);
        characterController2d.SetIfCanAttack(false);
        blackScreen = true;
        
    }

   /* void ContinueGame()
    {
        blackScreen = true;
        myCG.alpha = 0;    
    }*/



        
}