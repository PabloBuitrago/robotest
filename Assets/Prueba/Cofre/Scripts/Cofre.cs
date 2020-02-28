using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Cofre : MonoBehaviour {

    public GameObject itemFis;

    public bool[,] inv = new bool[2, 6];
    public GameObject[,] obj = new GameObject[2, 6];

    Animator anim;

    Controlador controlador;

    Inventario scriptInv;

    public bool abierto = false;

    bool entrar = false;

    bool esperado = true;

    GameObject player;
    GameObject salida;

    public Text texto;

    public bool debug = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scriptInv = player.GetComponent<Inventario>();

        salida = GameObject.FindGameObjectWithTag("Salida");

        controlador = GameObject.FindGameObjectWithTag("Controlador").GetComponent<Controlador>();

        anim = GetComponent<Animator>();

        NotificationCenter.DefaultCenter().AddObserver(this, "Cerrar");
    }

	void Start () {
        
	}

    void Update()
    {
        if (Input.GetButtonDown("E") && entrar && esperado)
        {
            esperado = false;

            Invoke("Esperar", 0.5f);

            abierto = !abierto;

            anim.SetBool("abrir", abierto);

            if (!abierto)
                Cerrar();                   

            controlador.abiertoCofre = abierto;
        }

        //Debug cofre.

        if (debug)
        {
            texto.text = "";

            for (int j = 0; j < controlador.posicionesy.Length; j++)
            {
                for (int i = 0; i < controlador.posicionesx.Length; i++)
                {
                    texto.text += "Gameobject[" + j + "," + i + "] = " + obj[j, i] + "\n";
                }
            }

            texto.text += "----------------------------------\n";

            for (int j = 0; j < controlador.posicionesy.Length; j++)
            {
                for (int i = 0; i < controlador.posicionesx.Length; i++)
                {
                    texto.text += "Inventario[" + j + "," + i + "] = " + inv[j, i] + "\n";
                }
            }
        }       
        
    }

    void Esperar()
    {
        esperado = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            entrar = true;        
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            entrar = false;
        }

    }

    public void Abrir()
    {
        OcultarMostrar(true);
    }

    public void Cerrar()
    {
        OcultarMostrar(false);      
    }

    void OcultarMostrar(bool activo)
    {
        controlador.ActivarInvCofre(activo);

        for (int i = 0; i < controlador.posicionesx.Length; i++)
        {
            for (int j = 0; j < controlador.posicionesy.Length; j++)
            {
                if (obj[j, i] != null)
                    obj[j, i].SetActive(activo);
            }
        }
    }

    public void Posiciar(int num11, int num12, int num21, int num22)
    {
        obj[num11, num12].transform.position = new Vector3(controlador.posicionesx[num22], controlador.posicionesy[num21], 0);
        obj[num21, num22].transform.position = new Vector3(controlador.posicionesx[num12], controlador.posicionesy[num11], 0);

        GameObject auxGame;

        auxGame = obj[num11, num12];
        obj[num11, num12] = obj[num21, num22];
        obj[num21, num22] = auxGame;
    }

    public void Lanzar(int num1, int num2)
    {
        Sprite sprite = obj[num1, num2].GetComponent<Image>().sprite;

        Destroy(obj[num1, num2]);

        GameObject aux = Instantiate(itemFis);

        aux.transform.position = salida.transform.position;

        aux.GetComponent<Rigidbody>().AddForce(player.transform.forward * scriptInv.fuerza);

        aux.GetComponent<SpriteRenderer>().sprite = sprite;

        inv[num1, num2] = false;
    }
}
