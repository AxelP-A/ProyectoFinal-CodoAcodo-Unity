using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BOSS_STATE {
        INITIAL,
        PHASE1,
        PHASE2,
        BOOM
    }
    
    BOSS_STATE currentState;
    public float appearSpeed;
    public float zigZagSpeed;

    public float maxLeft;
    public float maxRigh;
    private Vector3 dir = Vector3.left;

    [SerializeField] BoxCollider2D[] hitboxes;
    // Start is called before the first frame update
    void Start()
    {
        currentState = BOSS_STATE.INITIAL;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            // Cuando aparece y es invul
            case BOSS_STATE.INITIAL:
                transform.Translate(new Vector3(0f, -0.5f * Time.deltaTime * appearSpeed, 0f));
                if(transform.position.y <= 2.6f ){
                    // Si se llega a la posicion deseada, activamos los hitboxes y cambiamos de fase.
                    for(int i=0; i <hitboxes.Length; i++){
                        hitboxes[i].enabled = true;
                    }
                    currentState = BOSS_STATE.PHASE1;
                }
                break;
            case BOSS_STATE.PHASE1:
                //Debug.Log("Se paso a fase 1"); 
                transform.Translate(dir* zigZagSpeed *Time.deltaTime);

                if(transform.position.x <= maxLeft){
                    dir = Vector3.right;
                }else if(transform.position.x >= maxRigh){
                    dir = Vector3.left;
                }    
                // code block */
                break;
            case BOSS_STATE.PHASE2:
                // code block
                break;
            case BOSS_STATE.BOOM:
                // code block
                break;
            default:
                // code block
                break;
        }
    }
}
