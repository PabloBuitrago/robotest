using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombate : MonoBehaviour {

    Animator anim;

    public PlayerMovement playerMov;

    public GameObject espada;

    public Slider slider;
    public GameObject background;

    public float vida = 100f;

    GameObject enemy = null;
    EnemyCombate enemyComb;

    public float distanciaMin = 2;
    float distancia;

    float fuerza = 0;

    Inventario inv;
    PlayerCombate playerComb;

    bool muerto = true;

    public Construccion cons;

    void Start () {
        anim = GetComponent<Animator>();
        playerComb = GetComponent<PlayerCombate>();
        inv = GetComponent<Inventario>();
        
        slider.maxValue = vida;
        slider.value = vida;
    }
		
	void Update () {
        if (Cursor.visible == false)
        {
            if (Input.GetMouseButtonDown(0) && playerMov.pegar && playerMov.salto && !cons.entrar)
            {
                espada.SetActive(true);
                anim.SetTrigger("atack");

                playerMov.pegar = false;

                Invoke("Espada", 1.08f);

                fuerza = 30f;
            }

            if (Input.GetMouseButtonDown(1) && playerMov.pegar && playerMov.salto)
            {
                anim.SetTrigger("atack2");

                playerMov.pegar = false;

                Invoke("Punetazo", 0.7f);

                fuerza = 10f;
            }
        }

        if (enemy != null)
        {
            distancia = (Mathf.Sqrt(Mathf.Pow(enemy.transform.position.x - transform.position.x, 2))
            + (Mathf.Pow(enemy.transform.position.z - transform.position.z, 2)));
        }       
    }

    public void QuitarVida(float numero)
    {
        if (muerto)
        {
            vida -= numero;

            slider.value = vida;

            if (vida <= 0)
            {
                muerto = false;

                background.SetActive(false);
                anim.SetTrigger("dead");

                playerMov.enabled = false;
                playerComb.enabled = false;

                inv.Morir();
                inv.enabled = false;

                Controlador.muerto = true;
            }
            else
            {
                anim.SetTrigger("hit");
            }
        }       
        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;

        if (col.tag == "Enemy")
        {
            enemy = col;
            enemyComb = col.GetComponent<EnemyCombate>();
        }
    }

    public void Atacar()
    {
        if(enemyComb != null && distancia < distanciaMin)
        {
            enemyComb.QuitarVida(fuerza);
        }        
    }

    public void Espada()
    {
        espada.SetActive(false);
        playerMov.pegar = true;
    }

    public void Punetazo()
    {
        playerMov.pegar = true;
    }
}
