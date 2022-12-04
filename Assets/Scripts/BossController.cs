using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public int maxHp;
    int currentHp;
    BlinkEffect blinkScript;


    [SerializeField] BoxCollider2D[] hitboxes;
    [SerializeField] Transform[] bulletSpawners;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float timeBetweenBarrages;

    [SerializeField] Transform[] bombSpawners;
    [SerializeField] Transform[] explosionIndicators;

    // Start is called before the first frame update
    void Start()
    {
        currentState = BOSS_STATE.INITIAL;
        currentHp = maxHp;
        blinkScript = GetComponent<BlinkEffect>();
        FillTheBombSpawners();
    }

    void FillTheBombSpawners(){
        GameObject parent = GameObject.Find("BossMarkers");
        for(int i=0; i<12; i++){
            bombSpawners[i] = parent.transform.GetChild(i).transform;
        }
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
                // Disparamos
                if(isShooting == null){
                    isShooting = StartCoroutine(IsShooting(timeBetweenBarrages));
                }
                // Switch to phase 2
                if(currentHp <= 2500){
                    currentState = BOSS_STATE.PHASE2;
                }
                break;
            case BOSS_STATE.PHASE2:
                transform.Translate(dir* zigZagSpeed *Time.deltaTime);

                if(transform.position.x <= maxLeft){
                    dir = Vector3.right;
                }else if(transform.position.x >= maxRigh){
                    dir = Vector3.left;
                }
                // Disparos fase 2
                if(isBombarding == null){
                    isBombarding = StartCoroutine(IsBombarding(2f));
                }
                if(currentHp <= 0){
                    currentState = BOSS_STATE.BOOM;
                }
                break;
            case BOSS_STATE.BOOM:
                //Debug.Log("Se entro a Boom");
                if(isExploting == null){
                    isExploting = StartCoroutine(IsExploting());
                }
                break;
            default:
                // code block
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if((currentState == BOSS_STATE.PHASE1 || currentState == BOSS_STATE.PHASE2) && col.tag == "PBullet"){
            currentHp = GameManager.instance.DealDamageToBoss(currentHp, 15);
            if(blinkDamage == null){
                blinkDamage = StartCoroutine(BlinkDamage(0.3f));
            }
            // We destroy the bullet
            Destroy(col.gameObject);
        }
    }

    Coroutine blinkDamage = null;
    IEnumerator BlinkDamage(float blinkTime){
        // Activamos el blinking
        blinkScript.enabled = true;
        yield return new WaitForSeconds(blinkTime);
        // Lo apagamos
        blinkScript.enabled = false;
        blinkDamage = null;
    }

    Coroutine isShooting = null;
    IEnumerator IsShooting(float timeBetweenBarrages){
        for(int i=0; i<4; i++){
            ShootBullets();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(timeBetweenBarrages);// Delay antes de disparar de nuevo.        
        isShooting = null;
    }

    void ShootBullets(){
        for(int i=0; i< bulletSpawners.Length; i++){
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawners[i].position, bulletSpawners[i].rotation);
            //bullet.transform.parent = transform; // Ponerle otro item de padre q no se mueva ?
            // Dirijo la bala hacia abajo.
            bullet.GetComponent<EnemyBullet>().SetDirection(Vector2.down);
        }
    }

    Coroutine isBombarding = null;
    IEnumerator IsBombarding(float timeBetweenBarrages){
        for(int i=0; i<6; i++){
            int j = Random.Range(0, 12);
            LaunchBombs(j);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(timeBetweenBarrages);// Delay antes de disparar de nuevo.        
        isBombarding = null;
    }

    void LaunchBombs(int nIndex){
        bombSpawners[nIndex].GetChild(0).GetComponent<Animator>().Play("reticulaSpin");
    }

    Coroutine isExploting = null;
    IEnumerator IsExploting(){
        for(int i=0; i<14; i++){
            GameManager.instance.PlayExplotion(explosionIndicators[i].position, Color.blue);
            VFXController.instance.PlayVFX(VFXController.VFXName.EXPLOSION);
            yield return new WaitForSeconds(0.4f);
        }
        // Triggereamos el win despues de las explosiones.
        GameManager.instance.TriggerVictory();
        yield return null;    
    }

}
