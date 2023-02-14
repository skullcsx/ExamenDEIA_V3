using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infiltrador : MonoBehaviour
{
    //creo un gameObject para poder instanciar al infiltrador
    public GameObject infi; 

    void Update()
    {
        //creo una comprobacion de que cuando toque el espacio reactivamos al infiltrador
        if(Input.GetKeyDown("space"))
        {
            infi.SetActive(true);
        }
    }
}
