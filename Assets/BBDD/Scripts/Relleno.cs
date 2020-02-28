using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Data;
using Mono.Data.SqliteClient;

public class Relleno : MonoBehaviour {
    public Text texto;
    string BDfile;

    public Text aux;

    IDbConnection conexionDB;

    void Start () {
        BDfile = "URI=file:" + Application.streamingAssetsPath + "/Prueba.sqlite";

        aux.text = Application.streamingAssetsPath + "/Prueba.sqlite";


        conexionDB = new SqliteConnection(BDfile);      

        Abrir();
        Monstrar();
        Cerrar();

    }

    void Abrir()
    {        
        conexionDB.Open();
    }

    void Cerrar()
    {
        conexionDB.Close();
    }

    void Insertar()
    {
        IDbCommand comandoBD = conexionDB.CreateCommand();      

        string insercionSQL = "INSERT INTO Datos (Nombre, Cantidad) VALUES('Victor', '45')";

        comandoBD.CommandText = insercionSQL;

        comandoBD.ExecuteReader();        
    }

    void Monstrar()
    {
        IDbCommand comandoBD = conexionDB.CreateCommand();

        string consultaSQL = "SELECT * FROM Datos";

        comandoBD.CommandText = consultaSQL;

        IDataReader puntero = comandoBD.ExecuteReader();     

        while (puntero.Read())
        {
            texto.text += "\n id:       " + puntero.GetInt32(0) + "       Nombre: " + puntero.GetString(1) + "       Cantidad: " + puntero.GetFloat(2);
        }

        puntero.Close();
    }
	
	void Update () {
		
	}
}
