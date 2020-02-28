using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyCombate : MonoBehaviour {

    Animator anim;

    public Slider slider;
    public GameObject background;

    public float vida = 300f;

    PlayerCombate playerComb;

    public float fuerza = 10;

    public CapsuleCollider col;
    EnemyCombate enemyComb;
    EnemyMovement enemyMov;
    NavMeshAgent nav;

    void Start () {
        anim = GetComponent<Animator>();
        enemyComb = GetComponent<EnemyCombate>();
        enemyMov = GetComponent<EnemyMovement>();
        nav = GetComponent<NavMeshAgent>();

        slider.maxValue = vida;
        slider.value = vida;

        playerComb = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombate>();
    }
	
	void Update () {
		
	}

    private IEnumerator Combate()
    {
        if (enemyMov.distancia < enemyMov.distanciaMin && Controlador.muerto == false)
        {
            anim.SetTrigger("atack");

            enemyMov.ataque = true;
        }

        yield return new WaitForSeconds(2.5f);

        enemyMov.ataque = false;

        StartCoroutine("Combate");
    }

    public void QuitarVida(float numero)
    {
        vida -= numero;

        slider.value = vida;

        if (vida <= 0)
        {
            background.SetActive(false);
            anim.SetTrigger("dead");

            Destroy(col);
            Destroy(enemyMov);           
            Destroy(nav);
            Destroy(enemyComb);
        }
        else
        {
            anim.SetTrigger("hit");
        }        
    }

    public void EmpezarComb()
    {
        StartCoroutine("Combate");
    }
    
    public void Atacar()
    {
        if (enemyMov.distancia < enemyMov.distanciaMin && Controlador.muerto == false)
        {
            playerComb.QuitarVida(fuerza);
        }       
    }
}
