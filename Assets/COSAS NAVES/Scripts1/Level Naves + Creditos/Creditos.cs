using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Creditos : MonoBehaviour
{
    // Para las estadisticas
    public TextMeshProUGUI killsT;
    public TextMeshProUGUI meleeT;
    public TextMeshProUGUI rangeT;
    public TextMeshProUGUI receivedT;
    public TextMeshProUGUI defeatsT;
    public TextMeshProUGUI dashesT;
    public TextMeshProUGUI jumpsT;
    public TextMeshProUGUI doubleJumpsT;
    public TextMeshProUGUI healingT;


    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
       // Debug.Log(PlayerPrefs.GetInt("killedEnemies") + " CANTIDAD DE ENEMIGOS ASESINADOS EN EL AWAKE.");
    }

    void Start()
    {
        anim = transform.GetComponent<Animator>();
        LoadEndGameData();
        DisplayStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) 
        {
            SceneManager.LoadScene("Menu_Scene");
        }
    }

    public void WaitToEnd()
    {
        SceneManager.LoadScene("Menu_Scene");
    }

    int deathCount;
    int simpleAttacksDone;
    int rangedAttacksDone;
    int timesHealed;
    int quantityOfDashes;
    int quantityOfJumps;
    int quantityOfDoubleJumps;
    int hitsGivenToYou;
    int killedEnemies;

    public void LoadEndGameData()
    {
        deathCount = PlayerPrefs.GetInt("deathCount", 0);
        simpleAttacksDone = PlayerPrefs.GetInt("simpleAttacksDone", 0);
        rangedAttacksDone = PlayerPrefs.GetInt("rangedAttacksDone", 0);
        timesHealed = PlayerPrefs.GetInt("timesHealed", 0);
        quantityOfDashes = PlayerPrefs.GetInt("quantityOfDashes", 0);
        quantityOfJumps = PlayerPrefs.GetInt("quantityOfJumps", 0);
        quantityOfDoubleJumps = PlayerPrefs.GetInt("quantityOfDoubleJumps", 0);
        hitsGivenToYou = PlayerPrefs.GetInt("hitsGivenToYou", 0);
        killedEnemies = PlayerPrefs.GetInt("killedEnemies", 0);
    }

    void DisplayStats(){
        killsT.text = killedEnemies.ToString();
        meleeT.text = simpleAttacksDone.ToString();
        rangeT.text = rangedAttacksDone.ToString();
        receivedT.text = hitsGivenToYou.ToString();
        defeatsT.text = deathCount.ToString();
        dashesT.text = quantityOfDashes.ToString();
        jumpsT.text = quantityOfJumps.ToString();
        doubleJumpsT.text = quantityOfDoubleJumps.ToString();
        healingT.text = timesHealed.ToString();
    }

    public void StartRollingCredits(){
        anim.enabled = true;
        Invoke("WaitToEnd", 8);
    }
}
