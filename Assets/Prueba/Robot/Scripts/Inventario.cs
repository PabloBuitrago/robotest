using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Inventario : MonoBehaviour {

    public GameObject itemCanvas;
    public GameObject itemFis;
    public GameObject itemCons;

    public GameObject mochila;

    public GameObject inven;
    
    public float fuerza = 300f;

    public bool[,] inv = new bool[3,5];
    public GameObject[,] obj = new GameObject[3,5];

    GameObject[] ranx = new GameObject[5];
    GameObject[] rany = new GameObject[3];

    public float[] posicionesx = new float[5];
    public float[] posicionesy = new float[3];

    public Image[] casillas = new Image[5];

    int j = 0;

    public int num = 0;   

    public Transform salida;

    public Animator alerta;

    public Color gris;
    public Color blanco;

    public RigidbodyFirstPersonController movCamara;

    bool entrar = true;

    public Cofre cofre;

    bool esperado = true;

    bool enCofre = false;

    public Text texto;
    public Text texto2;

    Quaternion guardar;

    public bool debug;

    public Construccion cons;

    public Sprite imagen;

    void Start ()
    {
        ranx = GameObject.FindGameObjectsWithTag("Inventario");
        rany = GameObject.FindGameObjectsWithTag("Mochila");

        Ordenacionx(new float[] { ranx[0].transform.position.x, ranx[1].transform.position.x, ranx[2].transform.position.x, ranx[3].transform.position.x,
            ranx[4].transform.position.x });

        Ordenaciony(new float[] { rany[0].transform.position.y, rany[1].transform.position.y, rany[2].transform.position.y});
    }

    void Ordenacionx(float[] numeros)
    {
        float auxili;
        int j;
        for (int i = 0; i < numeros.Length; i++)
        {
            auxili = numeros[i];
            j = i - 1;
            while (j >= 0 && numeros[j] > auxili)
            {
                numeros[j + 1] = numeros[j];
                j--;
            }
            numeros[j + 1] = auxili;
        }

        for (int i = 0; i < ranx.Length; i++)
        {
            posicionesx[i] = numeros[i];
        }
    }

    void Ordenaciony(float[] numeros)
    {
        float auxili;
        int j;
        for (int i = 0; i < numeros.Length; i++)
        {
            auxili = numeros[i];
            j = i - 1;
            while (j >= 0 && numeros[j] > auxili)
            {
                numeros[j + 1] = numeros[j];
                j--;
            }
            numeros[j + 1] = auxili;
        }

        for (int i = 0; i < rany.Length; i++)
        {
            posicionesy[i] = numeros[i];
        }
    }

    void Update () {

        if (Input.GetButtonDown("Q") && inv[0,num] == true)
        {
            Lanzar(0, num);

            cons.Falso();
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                Avanzar(true);
            }

            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                Avanzar(false);
            }

            cons.Comprobar(num);
        }

        if (Input.GetButtonDown("E") && esperado)
        {
            if (enCofre)
            {
                esperado = false;

                Invoke("Esperar", 0.5f);
            }           

            CambioCursor();
        }

        //Debug inventario

        if (debug)
        {
            texto.text = "";

            for (int j = 0; j < posicionesy.Length; j++)
            {
                for (int i = 0; i < posicionesx.Length; i++)
                {
                    texto.text += "Gameobject[" + j + "," + i + "] = " + obj[j, i] + "\n";
                }
            }

            texto2.text = "";

            for (int j = 0; j < posicionesy.Length; j++)
            {
                for (int i = 0; i < posicionesx.Length; i++)
                {
                    texto2.text += "Inventario[" + j + "," + i + "] = " + inv[j, i] + "\n";
                }
            }
        }      
    }

    void Esperar()
    {
        esperado = true;
    }

    public void Lanzar(int num1, int num2)
    {
        Sprite sprite = obj[num1,num2].GetComponent<Image>().sprite;

        Destroy(obj[num1,num2]);

        if (obj[num1, num2].name.Contains("Construccion"))
        {
            GameObject aux = Instantiate(itemCons);

            aux.transform.position = salida.transform.position;

            aux.GetComponent<Rigidbody>().AddForce(transform.forward * fuerza);
        }
        else
        {
            GameObject aux = Instantiate(itemFis);

            aux.transform.position = salida.transform.position;

            aux.GetComponent<Rigidbody>().AddForce(transform.forward * fuerza);

            aux.GetComponent<SpriteRenderer>().sprite = sprite;
        }        

        inv[num1,num2] = false;
    }

    public void CambioCursor()
    {
        if (entrar)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            movCamara.enabled = false;
            OcultarMostrar(true);
        }
        else
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                movCamara.enabled = true;
                OcultarMostrar(false);
            }
        }

        entrar = !entrar;
    }

    public void Morir()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        movCamara.enabled = false;
    }

    void OcultarMostrar(bool activo)
    {
        mochila.SetActive(activo);

        for (int i = 0; i < posicionesx.Length; i++)
        {
            for (int j = 1; j < posicionesy.Length; j++)
            {
                if(obj[j, i] != null)
                    obj[j, i].SetActive(activo);
            }
        }
     }

    void Avanzar(bool direccion)
    {
        casillas[num].color = blanco;

        if (direccion)
        {
            num++;
        }
        else
        {
            num--;
        }

        if (num > 4)
            num = 0;
        if (num < 0)
            num = 4;

        casillas[num].color = gris;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;

        if (col.tag == "Item")
        {

            j = 0;

            while (j < 5 && inv[0,j] == true)
            {

                j++;
            }

            if(j == 5)
            {
                j = 0;

                while (j < 5 && inv[2, j] == true)
                {

                    j++;
                }

                if(j == 5)
                {
                    j = 0;

                    while (j < 5 && inv[1, j] == true)
                    {

                        j++;
                    }

                    if(j == 5)
                    {
                        alerta.SetTrigger("entrar");
                    }
                    else
                    {
                        Aparecer(1, j, other, col, false);
                    }
                }
                else
                {
                    Aparecer(2, j, other, col, false);
                }                
            }
            else
            {
                Aparecer(0, j, other, col, true);
            }

            if (!cons.entrar)
            {
                cons.Comprobar(num);
            }              
        }

        if (col.tag == "Cofre")
        {
            cofre = col.GetComponent<Cofre>();

            enCofre = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject col = other.gameObject;

        if (col.tag == "Cofre")
        {
            enCofre = false;
        }
    }

    void Aparecer(int num1, int num2, Collider other, GameObject col, bool activar)
    {
        inv[num1, num2] = true;

        obj[num1, num2] = Instantiate(itemCanvas);

        obj[num1, num2].name = col.name;

        if (col.name.Contains("Construccion"))
            obj[num1, num2].GetComponent<Image>().sprite = imagen;
        else
            obj[num1, num2].GetComponent<Image>().sprite = other.GetComponent<SpriteRenderer>().sprite;

        obj[num1, num2].transform.SetParent(inven.transform);

        obj[num1, num2].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

        obj[num1, num2].transform.position = new Vector3(posicionesx[num2], posicionesy[num1], 0);

        obj[num1, num2].SetActive(activar);

        Destroy(col);
    }
}
