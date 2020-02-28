using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

    Transform perseguirObjectivo;

    NavMeshAgent nav;
    Animator anim;

    EnemyCombate enemyComb;

    public float distanciaMin = 2;
    public float distancia;

    public bool entrar = false;

    Vector3 guardado;

    public bool ataque = false;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        enemyComb = GetComponent<EnemyCombate>();
    }

    void Start()
    {
        guardado = transform.position;
    }

    void Update() {

        if (!ataque)
        {
            if (entrar && Controlador.muerto == false)
            {
                Actualizar(perseguirObjectivo.position);
            }
            else
            {
                Actualizar(guardado);
            }
        }

        anim.SetFloat("velocity", (Mathf.Sqrt(Mathf.Pow(nav.velocity.x, 2) + Mathf.Pow(nav.velocity.z, 2))) / nav.speed);

        if(perseguirObjectivo != null)
            distancia = (Mathf.Sqrt(Mathf.Pow(perseguirObjectivo.position.x - transform.position.x, 2)) 
            + (Mathf.Pow(perseguirObjectivo.position.z - transform.position.z, 2)));
    }

    public void Actualizar(Vector3 puntoDestino)
    {
        nav.destination = puntoDestino;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;

        if (col.tag == "Player")
        {
            entrar = true;

            perseguirObjectivo = col.transform;

            distancia = (Mathf.Sqrt(Mathf.Pow(perseguirObjectivo.position.x - transform.position.x, 2))
            + (Mathf.Pow(perseguirObjectivo.position.z - transform.position.z, 2)));

            enemyComb.EmpezarComb();
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject col = other.gameObject;

        if (col.tag == "Finish")
        {
            entrar = false;
        }
    }

}
