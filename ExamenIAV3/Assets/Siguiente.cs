using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siguiente : MonoBehaviour
{
    public Patrullaje trulla;
    void Start()
    {
        
        trulla = FindObjectOfType<Patrullaje>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        trulla.puntosMovimientos.Add(transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
