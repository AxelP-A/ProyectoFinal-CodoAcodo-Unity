using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public CanvasGroup myCG;
    public GameObject startGameBlackScreen;
    public GameObject player;
    CharacterController2D characterController2d;
    public GameObject pauseMenuUI;
    public GameObject playerProtection;
    public GameObject mainCamera;
    AudioListener audioListener;
    private bool blackScreen = false;

    public static int deathCount = 0;
    public static int simpleAttacksDone = 0;
    public static int rangedAttacksDone = 0;
    public static int timesHealed = 0;
    public static int quantityOfDashes = 0;
    public static int quantityOfJumps = 0;
    public static int quantityOfDoubleJumps = 0;
    public static int hitsGivenToYou = 0;
    public static int timesPlayed = 0;
    public static int spacebarTimesPressed = 0;
    public static int mouseLeftTimesPressed = 0;
    public static int mouseRightTimesPressed = 0;
    public static int alpha1TimesPressed = 0;
    public static int alpha2TimesPressed = 0;
    public static int killCount = 0;

    public static GameManager Instance = null;

    public static bool gameIsPaused = false;

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
		//DontDestroyOnLoad (gameObject);
       audioListener = mainCamera.GetComponent<AudioListener>();
    }

    void Start()
    {
        FMODAudioPlayer.Instance.PlayAmbienceSound();
    }
     
     void FixedUpdate ()
     {
        if (blackScreen)
        {
            myCG.alpha = myCG.alpha - Time.deltaTime;
            if (myCG.alpha <= 0)
            {
                myCG.alpha = 0;
                blackScreen = false;
                characterController2d.SetIfCanMove(true);
                characterController2d.SetIfCanAttack(true);
                playerProtection.SetActive(false);
                //audioListener.enabled = true;
                //FMODAudioPlayer.Instance.PlayAmbienceSound();
            }
        }


     }

     void Update()
     {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
     }

     public void Resume()
     {
        FMODAudioPlayer.Instance.PauseSound();
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        characterController2d.SetIfCanMove(true);
        characterController2d.SetIfCanAttack(true);
        gameIsPaused = false;
     }

     void Pause()
     {
        FMODAudioPlayer.Instance.PauseSound();
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        characterController2d.SetIfCanMove(false);
        characterController2d.SetIfCanAttack(false);
        gameIsPaused = true;
     }
     
    void StartGame()
    {
        if(deathCount == 0)
        {
            playerProtection.SetActive(true);
            //audioListener.enabled = false;
            //FMODAudioPlayer.Instance.PlayIntroductoryNarrativeSound();
            startGameBlackScreen.SetActive(true);
            myCG.alpha = 1;
            characterController2d.SetIfCanMove(false);
            characterController2d.SetIfCanAttack(false);
            blackScreen = true;

        }  
    }

    public void ToMenu()
    {
        Time.timeScale = 1f;
        FMODAudioPlayer.Instance.StopAmbienceSound();
        SceneManager.LoadScene(0);
    }

   /* void ContinueGame()
    {
        blackScreen = true;
        myCG.alpha = 0;    
    }*/


    public void DestroyAndSaveData()
    {
        PlayerPrefs.SetInt("deathCount", deathCount);
        PlayerPrefs.SetInt("simpleAttacksDone", simpleAttacksDone);
        PlayerPrefs.SetInt("rangedAttacksDone", rangedAttacksDone);
        PlayerPrefs.SetInt("timesHealed", timesHealed);
        PlayerPrefs.SetInt("quantityOfDashes", quantityOfDashes);
        PlayerPrefs.SetInt("quantityOfJumps", quantityOfJumps);
        PlayerPrefs.SetInt("quantityOfDoubleJumps", quantityOfDoubleJumps);
        PlayerPrefs.SetInt("hitsGivenToYou", hitsGivenToYou);
        PlayerPrefs.SetInt("timesPlayed", timesPlayed);
        PlayerPrefs.SetInt("spacebarTimesPressed", spacebarTimesPressed);
        PlayerPrefs.SetInt("mouseLeftTimesPressed", mouseLeftTimesPressed);
        PlayerPrefs.SetInt("mouseRightTimesPressed", mouseRightTimesPressed);
        PlayerPrefs.SetInt("alpha1TimesPressed", alpha1TimesPressed);
        PlayerPrefs.SetInt("alpha2TimesPressed", alpha2TimesPressed);
        PlayerPrefs.SetInt("killedEnemies", killCount);
        Destroy(gameObject);
    }


        
}