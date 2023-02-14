using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoInfiltrador : MonoBehaviour
{
    //definimos variables
    public float speed = 5.0f;
    private Vector3 target;
    public GameObject player;

    void Start()
    {
        //le asignamos el transform.position al target al momento de correr el juego
        target = transform.position;
    }

    void Update()
    {
        //creamos la comprobacion de si doy click en alguna parte de la pantalla el infiltrador se movera a dicho punto
        if (Input.GetMouseButtonDown(0))
        {
            //aqui hacemos el seguimiento del punto en sonde demos clic 
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
        }
        //nos movemos al punto donde se da el click
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    //creamos un private void OnTriggerEnter2D donde al momento de tocar al guardia se desactivara el infiltrador
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Guardia"))
        {
            player.SetActive(false);
        }
    }
}
