using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerMovement : MonoBehaviour {

    public float speed = 6f;
    public float speedMedium = 4.5f;
    public float speedBack = 3f;
    public float speedUp = 7f;

    public float speedCam = 10f;

    public float speedJump = 6f;

    Rigidbody rig;
    Animator anim;
    Vector3 movement;

    float v;
    float l;

    public bool pegar = true;
    public bool salto = true;

    float contadorx = 0;
    float contadorz = 0;
    float auxx = 0;
    float auxz = 0;

    float sumax = 0.1f;
    float sumaz = 0.1f;

    public GameObject camara;
    public GameObject camaraAtras;

    public RigidbodyFirstPersonController movCamara;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (Cursor.visible == false)
        {
            v = Input.GetAxisRaw("Vertical");

            l = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            v = 0;

            l = 0;
        }

        Move(v, l);

    }

    void Move(float v, float l)
    {
        movement.Set(0, 0, v);

        Animacion(v, contadorx, sumax, auxx, "velocityx", "frente");
        Animacion(l, contadorz, sumaz, auxz, "velocityz", "lado");

        if (v == 0 && l == 0 && Cursor.visible == false)
            anim.SetFloat("turn", movCamara.giro);
        else
            anim.SetFloat("turn", 0f);

        if (pegar)
        {
            if (v >= 0)
            {
                if (v == 1)
                {
                    if (l == 1)
                        movement = transform.forward * v * speedBack * Time.deltaTime;
                    else
                        movement = transform.forward * v * speedUp * Time.deltaTime;


                    movement += transform.right * l * speedMedium * Time.deltaTime;
                }
                else
                {
                    movement = transform.forward * v * speed * Time.deltaTime;
                    movement += transform.right * l * speedMedium * Time.deltaTime;
                }

            }
            else
            {
                movement = transform.forward * v * speedBack * Time.deltaTime;
                movement += transform.right * l * speedBack * Time.deltaTime;
            }

            rig.MovePosition(rig.position + movement);
        }

    }

    void Animacion(float var, float contador, float suma, float aux, string nombre, string pos)
    {
        if (var == 1)
        {
            contador += suma;
            if (contador > 1)
                contador = 1;
        }
        if (var == -1)
        {
            contador -= suma;
            if (contador < -1)
                contador = -1;
        }
        if (var == 0)
        {
            if (aux > 0)
            {
                contador -= suma;
                if (contador < 0)
                    contador = 0;
            }
            else
            {
                contador += suma;
                if (contador > 0)
                    contador = 0;
            }
        }
        else
        {
            aux = contador;
        }

        //Guardado

        if (pos == "frente")
        {
            contadorx = contador;
            sumax = suma;
            auxx = aux;
        }

        if (pos == "lado")
        {
            contadorz = contador;
            sumaz = suma;
            auxz = aux;
        }

        anim.SetFloat(nombre, contador);
    }

    void Update()
    {
        if (Cursor.visible == false)
        {
            if (Input.GetButtonDown("Jump") && salto && pegar)
            {
                salto = false;
                anim.SetTrigger("jump");
                rig.AddForce(new Vector3(0, speedJump, 0));
            }

            if (Input.GetButtonDown("F5"))
            {
                camara.SetActive(false);
                camaraAtras.SetActive(true);
            }

            if (Input.GetButtonUp("F5"))
            {
                camara.SetActive(true);
                camaraAtras.SetActive(false);
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
        #if UNITY_EDITOR

        #else
                Application.Quit();
        #endif
        }

    }

    public void Salto()
    {
        salto = true;
    }

    
}
