using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrullaje : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
   
    [SerializeField] private float distanciaMinima;
    private int siguientePaso = 0;
    public Vector2 target;
    public float speed;
    public GameObject wp;
    public List<Transform> puntosMovimientos = new List<Transform>();
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

   
   private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntosMovimientos[siguientePaso].position, velocidadMovimiento * Time.deltaTime);

        if (Vector2.Distance(transform.position, puntosMovimientos[siguientePaso].position) < distanciaMinima)
        {
            siguientePaso +=1;
            if (siguientePaso >= puntosMovimientos.Count)
            {
                siguientePaso = 0;
            }
        }
    }
    void FixedUpdate()
    {
        
        if (Input.GetMouseButtonDown(0) )
        {
            
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            Instantiate(wp, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        }
    }
}
