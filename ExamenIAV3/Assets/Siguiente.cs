using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siguiente : MonoBehaviour
{
    //definimos  la variable patrulla que vamos a heredar del codigo de patrullaje
    public Patrullaje patrulla;
    void Start()
    {
        //igualamos patrulla con el objeto findobjectoftype
        patrulla = FindObjectOfType<Patrullaje>();
        //guarda una nueva posicion 
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //agrega la posicion del nuevo punto a la lista que creamos en el codigo de patrullaje
        patrulla.puntosMovimientos.Add(transform);

    }
}
