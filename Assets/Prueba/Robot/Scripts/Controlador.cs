using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controlador : MonoBehaviour {

    public static bool muerto = false;

    public bool abiertoCofre = false;

    public GameObject invCofre;

    public GameObject[] ranx = new GameObject[6];
    public GameObject[] rany = new GameObject[2];

    public float[] posicionesx = new float[6];
    public float[] posicionesy = new float[2];

    void Start () {
        Ordenacionx(new float[] { ranx[0].transform.position.x, ranx[1].transform.position.x, ranx[2].transform.position.x, ranx[3].transform.position.x,
            ranx[4].transform.position.x, ranx[5].transform.position.x });

        Ordenaciony(new float[] { rany[0].transform.position.y, rany[1].transform.position.y});

        Controlador.muerto = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivarInvCofre(bool activar)
    {
        invCofre.SetActive(activar);
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

    public void Reiniciar()
    {
        SceneManager.LoadScene("Scene");
    }
}
