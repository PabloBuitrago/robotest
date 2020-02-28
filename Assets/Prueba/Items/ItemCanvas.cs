using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCanvas : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    bool pulsado = false;

    Vector3 guardado;

    Vector3 hueco;

    public GameObject itemFis;

    Inventario scriptInv;

    Construccion scriptCons;

    int anteriorx;
    int numerox;

    int anteriory;
    int numeroy;

    GameObject sel;
    GameObject obj;

    Controlador controlador;

    bool cofre;

    void Start ()
    {
        scriptInv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventario>();
        scriptCons = GameObject.FindGameObjectWithTag("Player").GetComponent<Construccion>();
        sel = GameObject.FindGameObjectWithTag("Selecion");
        obj = GameObject.FindGameObjectWithTag("Objetos");

        controlador = GameObject.FindGameObjectWithTag("Controlador").GetComponent<Controlador>();
    }

    void Update ()
    {
        if (pulsado)
        {
            transform.position = Input.mousePosition;
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        scriptCons.Falso();

        if (Controlador.muerto == false)
        {
            pulsado = true;

            cofre = true;

            guardado = transform.position;

            for (int i = 0; i < scriptInv.posicionesx.Length; i++)
            {
                if (guardado.x >= scriptInv.posicionesx[i] - 5 && guardado.x <= scriptInv.posicionesx[i] + 5)
                {
                    anteriorx = i;
                }
            }

            for (int i = 0; i < scriptInv.posicionesy.Length; i++)
            {
                if (guardado.y >= scriptInv.posicionesy[i] - 5 && guardado.y <= scriptInv.posicionesy[i] + 5)
                {
                    anteriory = i;
                    cofre = false;
                }
            }

            if (cofre)
            {
                for (int i = 0; i < controlador.posicionesx.Length; i++)
                {
                    if (guardado.x >= controlador.posicionesx[i] - 5 && guardado.x <= controlador.posicionesx[i] + 5)
                    {
                        anteriorx = i;
                    }
                }

                for (int i = 0; i < controlador.posicionesy.Length; i++)
                {
                    if (guardado.y >= controlador.posicionesy[i] - 5 && guardado.y <= controlador.posicionesy[i] + 5)
                    {
                        anteriory = i;
                    }
                }

                scriptInv.cofre.obj[anteriory, anteriorx].transform.SetParent(sel.transform);
            }
            else
            {
                scriptInv.obj[anteriory, anteriorx].transform.SetParent(sel.transform);
            }


            numerox = anteriorx;
            numeroy = anteriory;
        }      
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Controlador.muerto == false)
        {
            bool fuera = true;
            bool cambio = true;
            bool unico = true;

            pulsado = false;

            if (cofre)
                scriptInv.cofre.obj[anteriory, anteriorx].transform.SetParent(obj.transform);
            else
                scriptInv.obj[anteriory, anteriorx].transform.SetParent(obj.transform);


            for (int i = 0; i < scriptInv.posicionesx.Length; i++)
            {
                for (int j = 0; j < scriptInv.posicionesy.Length; j++)
                {
                    if (Mathf.Abs(transform.position.x) >= scriptInv.posicionesx[i] - 25 && Mathf.Abs(transform.position.x) <= scriptInv.posicionesx[i] + 25 &&
                        Mathf.Abs(transform.position.y) >= scriptInv.posicionesy[j] - 45 && Mathf.Abs(transform.position.y) <= scriptInv.posicionesy[j] + 45)
                    {
                        if (unico)
                        {
                            fuera = false;
                            unico = false;

                            numerox = i;
                            numeroy = j;

                            if (scriptInv.inv[j, i] == false)
                            {
                                guardado = new Vector3(scriptInv.posicionesx[i], scriptInv.posicionesy[j], guardado.z);

                                scriptInv.inv[numeroy, numerox] = true;

                                if (cofre)
                                    scriptInv.obj[numeroy, numerox] = scriptInv.cofre.obj[anteriory, anteriorx];
                                else
                                    scriptInv.obj[numeroy, numerox] = scriptInv.obj[anteriory, anteriorx];

                                Desactivar(anteriory, anteriorx);
                            }
                            else
                            {
                                cambio = false;

                                GameObject aux;

                                if (cofre)
                                {
                                    scriptInv.cofre.obj[anteriory, anteriorx].transform.position =
                                        new Vector3(scriptInv.posicionesx[numerox], scriptInv.posicionesy[numeroy], 0);
                                    scriptInv.obj[numeroy, numerox].transform.transform.position =
                                        new Vector3(controlador.posicionesx[anteriorx], controlador.posicionesy[anteriory], 0);

                                    aux = scriptInv.obj[numeroy, numerox];
                                    scriptInv.obj[numeroy, numerox] = scriptInv.cofre.obj[anteriory, anteriorx];
                                    scriptInv.cofre.obj[anteriory, anteriorx] = aux;
                                }
                                else
                                {
                                    scriptInv.obj[anteriory, anteriorx].transform.position =
                                        new Vector3(scriptInv.posicionesx[numerox], scriptInv.posicionesy[numeroy], 0);
                                    scriptInv.obj[numeroy, numerox].transform.transform.position =
                                        new Vector3(scriptInv.posicionesx[anteriorx], scriptInv.posicionesy[anteriory], 0);

                                    aux = scriptInv.obj[numeroy, numerox];
                                    scriptInv.obj[numeroy, numerox] = scriptInv.obj[anteriory, anteriorx];
                                    scriptInv.obj[anteriory, anteriorx] = aux;

                                }
                            }
                        }
                    }
                }
            }

            if (controlador.abiertoCofre && unico)
            {
                for (int i = 0; i < controlador.posicionesx.Length; i++)
                {
                    for (int j = 0; j < controlador.posicionesy.Length; j++)
                    {
                        if (Mathf.Abs(transform.position.x) >= controlador.posicionesx[i] - 25 && Mathf.Abs(transform.position.x) <= controlador.posicionesx[i] + 25 &&
                            Mathf.Abs(transform.position.y) >= controlador.posicionesy[j] - 45 && Mathf.Abs(transform.position.y) <= controlador.posicionesy[j] + 45)
                        {
                            if (unico)
                            {
                                fuera = false;
                                unico = false;

                                numerox = i;
                                numeroy = j;

                                if (scriptInv.cofre.inv[j, i] == false)
                                {
                                    guardado = new Vector3(controlador.posicionesx[i], controlador.posicionesy[j], guardado.z);

                                    scriptInv.cofre.inv[numeroy, numerox] = true;

                                    if (cofre)
                                        scriptInv.cofre.obj[numeroy, numerox] = scriptInv.cofre.obj[anteriory, anteriorx];
                                    else
                                        scriptInv.cofre.obj[numeroy, numerox] = scriptInv.obj[anteriory, anteriorx];

                                    Desactivar(anteriory, anteriorx);
                                }
                                else
                                {
                                    cambio = false;

                                    GameObject aux;

                                    if (cofre)
                                    {
                                        scriptInv.cofre.obj[anteriory, anteriorx].transform.position =
                                            new Vector3(controlador.posicionesx[numerox], controlador.posicionesy[numeroy], 0);
                                        scriptInv.cofre.obj[numeroy, numerox].transform.transform.position =
                                            new Vector3(controlador.posicionesx[anteriorx], controlador.posicionesy[anteriory], 0);

                                        aux = scriptInv.cofre.obj[numeroy, numerox];
                                        scriptInv.cofre.obj[numeroy, numerox] = scriptInv.cofre.obj[anteriory, anteriorx];
                                        scriptInv.cofre.obj[anteriory, anteriorx] = aux;
                                    }
                                    else
                                    {
                                        scriptInv.cofre.obj[numeroy, numerox].transform.position =
                                            new Vector3(scriptInv.posicionesx[anteriorx], scriptInv.posicionesy[anteriory], 0);
                                        scriptInv.obj[anteriory, anteriorx].transform.transform.position =
                                            new Vector3(controlador.posicionesx[numerox], controlador.posicionesy[numeroy], 0);

                                        aux = scriptInv.cofre.obj[numeroy, numerox];
                                        scriptInv.cofre.obj[numeroy, numerox] = scriptInv.obj[anteriory, anteriorx];
                                        scriptInv.obj[anteriory, anteriorx] = aux;
                                    }
                                }
                            }

                        }
                    }
                }
            }

            if (fuera && !cofre)
                scriptInv.Lanzar(anteriory, anteriorx);

            if (fuera && cofre)
                scriptInv.cofre.Lanzar(anteriory, anteriorx);

            if (cambio)
                transform.position = guardado;
        }

        scriptCons.Comprobar(scriptInv.num);
    }

    void Desactivar(int num1, int num2)
    {
        if (cofre)
        {
            scriptInv.cofre.inv[num1, num2] = false;
            scriptInv.cofre.obj[num1, num2] = null;
        }
        else
        {
            scriptInv.inv[num1, num2] = false;
            scriptInv.obj[num1, num2] = null;
        }           
    }
}
