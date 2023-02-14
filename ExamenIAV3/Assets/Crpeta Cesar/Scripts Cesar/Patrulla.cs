using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrulla : MonoBehaviour
{
    //declaramos variables
    public float patrolSpeed = 0f;
    public float changeTargetDistance = 1f;
    public Transform[] patrolPoints;

    int currentTarget = 0;


    void Update()
    {
        //hacemos la comprobacion que hara que nuestro objetivo actual se cambie al siguiente objetivo
        if(MoveToTarget())
        {
            currentTarget = GetNextTarget();
        }
    }

    //creamos la funcion MoveToTarget
    private bool MoveToTarget()
    {
        //creamos el distanceVector y le asignamos la posicion del current target - el transform.position
        Vector3 distanceVector = patrolPoints[currentTarget].position - transform.position;
        //haremos la comprobacion de si la magnitud es menor al cambio de objetivo nos regresara true si ya hemos llegado y false si no hemos llegado
        if(distanceVector.magnitude < changeTargetDistance)
        {
            return true;
        }

        Vector3 velocityVector = distanceVector.normalized;
        transform.position += velocityVector * patrolSpeed * Time.deltaTime;

        return false;
    }

    //creamos la funcion del nextTarget
    private int GetNextTarget()
    {
        //aumentamos uno al current taget y lo comparamos con nuestra lista
        currentTarget++;
        if(currentTarget >= patrolPoints.Length)
        {
            currentTarget = 0;
        }
        return currentTarget;
    }
}
