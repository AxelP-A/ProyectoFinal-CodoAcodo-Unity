using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    // Para la win screen.
    public AudioSource mainSong;
    // Todos los clips de audio
    public AudioClip victorySong;
    public AudioClip bossSong;
    // El objeto que se va a generar cada vez que se pide un VFX
    public GameObject parlante;

    [Header("Clips de VFX")]
    public AudioClip shootSound;
    public AudioClip explosionSound;
    public AudioClip pickupSound;
    public AudioClip gameOverSound;
    public AudioClip hitSound;
    public AudioClip hoverSound;
    public AudioClip sirenSound;

    //Trigger para bajar el volumen.
    bool lowerVolume = false;
    public float velocidadFade;
    public float maxVolume;

    // El Enum que se usa de Key para llamar a la funcion PlayVFX.
    public enum VFXName {
        SHOOT,
        EXPLOSION,
        PICKUP,
        GAME_OVER,
        HIT,
        HOVER,
        SIREN
    }
    // Un diccionario que va a contener todos los clips de audio con su respectivos volumenes.
    Dictionary<VFXName, Tuple<AudioClip, float>> clipLibrary = new Dictionary<VFXName, Tuple<AudioClip, float>>();

    public static VFXController instance = null;

    void Awake()
    {
        //If this script does not exit already, use this current instance
        if (instance == null)
            instance = this;

        //If this script already exit, DESTROY this current instance
        else if (instance != this)
            Destroy(gameObject);
        
        //LLenamos la libreria de Sonidos
        FillSoundLibrary();
    }

    // Funcion que reproduce cualquier VFX que queramos.
    public void PlayVFX(VFXName sonido){
        // Creamos un parlante
        GameObject efectoSonoro = Instantiate(parlante);
        // Lo colocamos como hijo del VFX Manager, para no spamear la jerarquia.
        efectoSonoro.transform.parent = transform;
        // Le ponemos un nombre para identificarlo
        efectoSonoro.name = "VFX SOUND - " + sonido.ToString();
        // Accedemos a su audioSource y seteamos el clip, el soonido
        AudioSource audioS = efectoSonoro.GetComponent<AudioSource>();
        audioS.clip = clipLibrary[sonido].Item1;
        audioS.volume = clipLibrary[sonido].Item2;
        // Lo reproducimos, y le decimos que se destruya cuando termine.
        audioS.Play();
        Destroy(efectoSonoro, audioS.clip.length); // Destruye el objeto una vez que termine de reproducirse.

        // Caso especifico para que se puedan destruir los hovers, porque la escala de tiempo es 0.
        if(sonido == VFXName.HOVER){
            efectoSonoro.AddComponent<DestroySound>();
        }        
    }

    void FillSoundLibrary(){
        // Usando las keys de los enum, puedo obtener el sonido y el volumen que corresponde.
        clipLibrary.Add(VFXName.SHOOT, Tuple.Create(shootSound, 1f));
        clipLibrary.Add(VFXName.EXPLOSION, Tuple.Create(explosionSound, 0.6f));
        clipLibrary.Add(VFXName.PICKUP, Tuple.Create(pickupSound, 0.28f));
        clipLibrary.Add(VFXName.GAME_OVER, Tuple.Create(gameOverSound, 1f));
        clipLibrary.Add(VFXName.HIT, Tuple.Create( hitSound, 0.57f));
        clipLibrary.Add(VFXName.HOVER, Tuple.Create(hoverSound, 1f));
        clipLibrary.Add(VFXName.SIREN, Tuple.Create(sirenSound, 0.05f));
    }

    // Esta funcion utiliza el Music Player Original.
    public void PlayVictoySound(){
        mainSong.Stop();
        mainSong.volume = 0.25f;
        mainSong.clip = victorySong; 
        mainSong.Play();
    }
    // Esta funcion es para que hagan ruido los botones al poner el mouse sobre ellos.
    // Dice que no tiene referencias pero si se esta usando en los botones.
    public void PlayButtonHoverSound(){
        PlayVFX(VFXName.HOVER);  
    }

    public void SwapToBossMusic(){
        lowerVolume = true;
        //Debug.Log("Se cambio Lower Vol");
    }

    void Update(){
        if(lowerVolume && mainSong.volume <= maxVolume){
            mainSong.volume -= velocidadFade * Time.deltaTime;
            if(mainSong.volume <=0){
                lowerVolume = false;
                mainSong.Stop();
                mainSong.clip = bossSong;
                mainSong.Play();                
            }
        }
        if(!lowerVolume && mainSong.volume < maxVolume){
            mainSong.volume += velocidadFade * Time.deltaTime;
        }
    }
}
