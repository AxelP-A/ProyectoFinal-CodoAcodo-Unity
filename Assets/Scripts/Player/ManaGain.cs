using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaGain : MonoBehaviour
{
    public GameObject manaBottle;
    //GameObject manaImage;
    public static ManaGain instance = null;
    public GameObject player;
    //public Transform enemyTransform;
    CharacterController2D characterController;
    GameObject mana;
    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    void Start()
    {
        characterController = player.GetComponent<CharacterController2D>();
       /// manaImage = player.transform.GetChild(1).gameObject; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void manaDrop(Transform enemyTransform)
    {

        Vector2 position = enemyTransform.position;
        mana = Instantiate(manaBottle, position, Quaternion.identity);
        mana.SetActive(true);
        Destroy(mana, 10f);
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Player"))
       {    
        if(characterController.mana < characterController.manaQuantity)
        {
            characterController.mana++;
            Destroy(mana);  
        } 
        else 
        {
            Destroy(mana);  
        }
       }
    }


}
