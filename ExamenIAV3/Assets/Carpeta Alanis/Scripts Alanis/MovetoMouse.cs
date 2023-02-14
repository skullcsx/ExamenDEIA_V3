using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovetoMouse : MonoBehaviour
{
    private Vector3 target;

    //se crea una variable llamada cambio que nos ayudara para cambiar de un estado a otro
    public bool cambio = false;
    public Rigidbody myRigidbody = null;
    public float fMaxSpeed = 1.0f;
    public float fMaxForce = 0.5f;

    public float fArriveRadius = 3.0f;
    public bool bUseArrive = true;
    public enum SteeringBehavior { Seek, Flee, Pursue, Evade, Arrive, Wander }
    public SteeringBehavior currentBehavior = SteeringBehavior.Seek;

    GameObject PursuitTarget = null;
    Rigidbody PursuitTargetRB = null;

    Vector3 v3TargetPosition = Vector3.zero;
    Vector3 v3SteeringForceAux = Vector3.zero;
    //se declara un vector publico en donde guardaremos la posicion del obstaculo
    public Vector3 v3PosicionCubo = Vector3.zero;
    
    void Start()
    {
        target = transform.position;
    }

    
    private void FixedUpdate()
    {
        Vector3 TargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TargetPosition.z = 0.0f;
        //Direccion deseada es punta ("a donde quiero llegar") - cola (donde estoy ahorita)
        Vector3 v3SteeringForce = Vector3.zero;

        switch (currentBehavior)
        {

         
            case SteeringBehavior.Flee:
                v3SteeringForce = Flee(TargetPosition);
                break;
    

            case SteeringBehavior.Arrive:
                v3SteeringForce = Arrive(TargetPosition);

                v3TargetPosition = TargetPosition;
                break;


        }
        v3SteeringForce = Arrive(TargetPosition);
        //se crea una condicion de si el cambio que en este caso es para que entre el Flee
        //es verdadero se manda a llamar Flee y pueda evadir ese obstaculo
        if (cambio == true)
        {
            
            v3SteeringForce += Flee(v3PosicionCubo);
            Debug.Log("Entro");
        }
        //currentVelocity += v3SteeringForce * Time.deltaTime;
        v3SteeringForceAux = v3SteeringForce;

        //Idealmente, usariamos el ForceMode de Force, para tomar en cuenta la masa del objeto
        //Aqui ya no usamos el deltaTome porque viene integrado en como funciona AddForce
        myRigidbody.AddForce(v3SteeringForce, ForceMode.Force);
        //Hacemos un clamp para que no exceda la velocidad maxima que puede tener el agente
        myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, fMaxSpeed);

    }
    public Vector3 Flee(Vector3 in_v3TargetPosition)
    {
        // Dirección deseada es punta ("a dónde quiero llegar") - cola (dónde estoy ahorita)
        Vector3 v3DesiredDirection = -1.0f * (in_v3TargetPosition - transform.position);
        Vector3 v3DesiredVelocity = v3DesiredDirection.normalized * fMaxSpeed;

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;
        // Igual aquí, haces este normalized*maxSpeed para que la magnitud de la
        // fuerza nunca sea mayor que la maxSpeed.
        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, fMaxForce);
        return v3SteeringForce;
    }
  
    Vector3 Arrive(Vector3 in_v3TargetPosition)
    {
        Vector3 v3Diff = in_v3TargetPosition - transform.position;
        float fDistance = v3Diff.magnitude;
        float fDesiredMagnitude = fMaxSpeed;
        if (fDistance < fArriveRadius)
        {
            fDesiredMagnitude = Mathf.InverseLerp(0.0f, fArriveRadius, fDistance);

            print("deaccelerating, inverse lerp is: " + fDesiredMagnitude);
        }

        Vector3 v3DesiredVelocity = v3Diff.normalized * fDesiredMagnitude;

        Vector3 v3SteeringForce = v3DesiredVelocity - myRigidbody.velocity;
        // Igual aquí, haces este normalized*maxSpeed para que la magnitud de la
        // fuerza nunca sea mayor que la maxSpeed.
        v3SteeringForce = Vector3.ClampMagnitude(v3SteeringForce, fMaxForce);
        return v3SteeringForce;
    }

    
    private void OnTriggerEnter(Collider other)
    {   //se pone una condicion de que si el tag llamado Obstaculo hace colision con otro el cambio se haga verdadero
        if (other.CompareTag("Obstaculos"))
        {
            cambio = true;
            //se iguala el vector con la posicion del obstaculo
            v3PosicionCubo = other.transform.position;
            
            Debug.Log("Choco");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        //se pone otra condicion para que se haga falso
        if (other.CompareTag("Obstaculos"))
        {
            cambio = false;

           
        }
    }
}
