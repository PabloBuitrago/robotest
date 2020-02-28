using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construccion : MonoBehaviour {

    public Inventario inventario;

    public GameObject objetoConst;
    public Transform objetoPos;

    GameObject[] obj = new GameObject[1];

    public bool entrar = false;

    int num = 0;

	// Use this for initialization
	void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {
        if (entrar)
        {
            obj[num].transform.position = objetoPos.position;
            obj[num].transform.rotation = objetoPos.rotation;

            if (Input.GetMouseButtonDown(0) && Cursor.visible == false)
            {
                obj[num].transform.SetParent(null);
                obj[num].GetComponent<Collider>().isTrigger = false;
                num++;
                entrar = false;

                obj = new GameObject[num + 1];

                Correcto();
            }
        }
    }

    public void Comprobar(int num)
    {
        if (inventario.obj[0, num] != null && inventario.obj[0, num].name.Contains("Construccion"))
            Correcto();
        else
            Falso();
    }

    public void Correcto()
    {
        entrar = true;

        obj[num] = Instantiate(objetoConst);

        obj[num].transform.position = objetoPos.position;

        obj[num].transform.SetParent(objetoPos.transform);
    }

    public void Falso()
    {
        entrar = false;

        Destroy(obj[num]);
    }
}
