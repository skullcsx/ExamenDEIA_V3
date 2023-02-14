using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrullaje : MonoBehaviour
{
    //definimos las variables que usaremos para la lista de los puntos de movimiento
    [SerializeField] private float velocidadMovimiento;
   
    [SerializeField] private float distanciaMinima;
    private int siguientePaso = 0;
    public Vector2 target;
    public float speed;
    public GameObject wp;
    public List<Transform> puntosMovimientos = new List<Transform>();


   private void Update()
   {
        //usamos el transform.position para guardar la posicion e ir al siguiente punto que se crea en la lista
        transform.position = Vector2.MoveTowards(transform.position, puntosMovimientos[siguientePaso].position, velocidadMovimiento * Time.deltaTime);

        //se crea la comprobacion de si vector2.distance es menor a la distancia minima
        if (Vector2.Distance(transform.position, puntosMovimientos[siguientePaso].position) < distanciaMinima)
        {
            //si si aumentamos uno al siguientepaso
            siguientePaso +=1;
            //y si es mayor o igual al contador igualamos a 0 el siguientepaso
            if (siguientePaso >= puntosMovimientos.Count)
            {
                siguientePaso = 0;
            }
        }
    }
    void FixedUpdate()
    {
        //utilizamos el boton izquierdo del mouse para crear los puntos por los cuales pasara nuestro agente square
        if (Input.GetMouseButtonDown(0) )
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            Instantiate(wp, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        }
    }
}
