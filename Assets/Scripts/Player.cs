using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    public Camera cam;
    public int life = 3;
    [SerializeField] int heartsQuantity;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    
    float width;
    float height;
    public float offset;

    // Para disparar
    public GameObject playerBullet;
    public GameObject bulletPos1;
    public GameObject bulletPos2;
    bool allowFire = true;

    public float fireRate = 0.3f; // Si el num baja dispara mas rapido.


    void Awake()
	{
     HeartsController();
    }



    void Start(){
        // Agarramos la altura del player basado en su transform scale.
        width = transform.localScale.x + offset;
        height = transform.localScale.y + offset;
    }

    void Move(Vector2 direction){
        // Limitamos el movimiento maximo del jugador. Sacando el tama;o maximo de la pantalla.
        Vector2 min = cam.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = cam.ViewportToWorldPoint(new Vector2(1, 1));
        // Le quitamos a min y max los valores de width y height del sprite del player
        max.x = max.x - width;
        min.x = min.x + width;
        max.y = max.y - height;
        min.y = min.y + height;
        // Calculamos la posicion del player
        Vector2 pos = transform.position;
        pos += direction * speed * Time.deltaTime;
        // Verificamos que este dentro de la pantalla y sino, lo arreglamos
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        // Actualizamos la posicion del player a la nueva
        transform.position = pos;
    }

    IEnumerator Shoot(){
        allowFire = false;
        // Creamos la bala
        GameObject bullet1 = Instantiate(playerBullet);
        // Movemos el objeto a donde tendria que salir.
        bullet1.transform.position = bulletPos1.transform.position;
        // Lo mismo para la segunda bala.
        GameObject bullet2 = Instantiate(playerBullet);
        bullet2.transform.position = bulletPos2.transform.position;
        yield return new WaitForSeconds(fireRate);
        allowFire = true;
    }


    // Update is called once per frame
    void Update()
    {
        // Agarramos el Input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x, y).normalized;
        // Movemos en la direccion.
        Move(direction);

        if(Input.GetKey(KeyCode.Space) && allowFire){
            StartCoroutine(Shoot());
        }
    }

    // Por si lo golpean.
    void OnTriggerEnter2D(Collider2D col){
        // Nos fijamos si choco contra otra nave o una bala enemiga.
        if( col.tag.Equals("EBullet") || col.tag.Equals("Enemy")){
            // Mostramos la explosion

            //col.gameObject.SendMessage("ApplyDamage", 1); Testeando
            life--;
            HeartsController();
            if(life <= 0)
            {   
            GameManager.instance.PlayExplotion(transform.position, new Color(255, 0, 0, 255));
            Destroy(gameObject);
            Debug.Log("Perdiste");
            }
            // Destuimos la bala.
            Destroy(col.gameObject);
           
        }
    }

    public void HeartsController()
    {
        if (life > heartsQuantity)
        {
            life = heartsQuantity;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < life)
            {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if(i < heartsQuantity)
            {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }
}
