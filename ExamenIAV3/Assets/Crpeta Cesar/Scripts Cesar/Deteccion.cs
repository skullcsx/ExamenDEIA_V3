using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deteccion : MonoBehaviour
{
    //definimos variables que usaremos en el cofigo
    public Transform Infiltrador;
    public Transform Guardia;
    public Rigidbody2D rb;
    public GameObject Guard;

    //creamos un limite para que nos pueda detectar el guardia hasta solo 360 grados
    [Range(0f,360f)]
    public float VisionAngle = 30f;
    public float VisionDistance = 10f;
    public float FollowVelocity = 3f;

    //definimos variables que usaremos en el cofigo
    public bool detected = false;
    public bool rotate = true;
    public bool EstadoAtaque = false;

    //Creamos una velocidad con un vector2 y lo igualamos a 0
    Vector2 velocity = Vector2.zero;
    void Start()
    {
        //iniciamos una corrutina para que el guardia pueda girar
        StartCoroutine("rotarGuardia");
    }

    private void Update()
    {
        
        detected = false;
        //creamos el infiltradorVector que almacenara la resta de la posision del guardia menos la posision del infiltrador
        Vector2 infiltradorVector = Infiltrador.position - Guardia.position;
        //creamos una comprobacion de si 
        if (Vector3.Angle(infiltradorVector.normalized,Guardia.right) < VisionAngle * 0.5f)
        {
            if (infiltradorVector.magnitude < VisionDistance)
            {
                detected = true;
                EstadoDeAlerta();
                rotate = true;

            }
            else
            {
                detected = false;
            }
        }
    }

    private void OnDrawGizmos()
    {

        if (VisionAngle <= 0f) return;
        //multip´licamos la visionangle por .5 para obtener dos mitades, la mitad positiva y la negativa 
        float halfVisionAngle = VisionAngle * 0.5f;

        //creamos dos puntos
        Vector2 p1, p2;

        //hacemos que cada punto corresponda a para cada angulo, con esto crearemos el cono de vision 
        p1 = PointForAngle(halfVisionAngle, VisionDistance);
        p2 = PointForAngle(-halfVisionAngle, VisionDistance);

        //dibujamos las lineas de los gizmos que nos creara el cono de vision
        Gizmos.color = detected ? Color.green : Color.red;
        Gizmos.DrawLine(Guardia.position, (Vector2)Guardia.position + p1);
        Gizmos.DrawLine(Guardia.position, (Vector2)Guardia.position + p2);

        //Gizmos.DrawRay(Guardia.position, Guardia.right * 4f);
    }

    //Creamos el metodo de pointForAngle
    Vector3 PointForAngle (float angle, float distance)
    {
        //devolvemos un punto que se creara utilizando el coseno para las x y el seno para las y
        return Guardia.TransformDirection( new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad))) * distance;
    }

    //creamos la corrutina para que el guardia pueda rotar
    IEnumerator rotarGuardia()
    {
        //mientras rotar sea verdadero vamos a sumar 45 grados en z cada 5 segundos
        while (rotate == true)
        {
            yield return new WaitForSeconds(5);
            transform.Rotate(0, 0, +45);
        }
        
    }

    //creamos la funcion del estado de alerta
    private void EstadoDeAlerta()
    {
        if (detected == true)
        {
            //aumentaremos el angulo de vision de nuestro guardia
            VisionAngle = 50f;

            //avanzaremos al objetivo despues de que el cono se haya hecho mas grande
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Infiltrador.position.x, Infiltrador.position.y, Infiltrador.position.z), FollowVelocity*Time.deltaTime);
            //crearemos un vector 3 que nos ayudare a poder ver al objetivo, esto para hacer que se rote el guaardia segun donde este el infiltrador y no pierda tan facil al infiltrador
            Vector3 VerInfiltrado = Infiltrador.position - Guardia.position;
            //aqui le damos las posisiones del infiltrador tanto en x como en y
            float angle = Mathf.Atan2(VerInfiltrado.y, VerInfiltrado.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            //desactivamos la bool de rotate para que no haga la corrutina de rotarGuardia
            rotate = false;

            //asignamos como true el bool que nos permitira activar el estado de ataque
            EstadoAtaque = true;
            //llamamos a la funcion de estado de ataque
            EstadoAtacando();
        }
        // aqui hacemos una comprobacion para que si el infiltrador sale del cono de vision regrese al estado de rotacion 
        else if (detected == false)
        {
            rotate = true;
            EstadoAtaque = false;
        }
    }

    //creamos la funcion del estado de ataque
    private void EstadoAtacando()
    {
        if (EstadoAtaque == true)
        {
            //creamos una posision del objetivo que nos permitira seguir al objetivo aunque cambie de posision
            Vector2 TargetPosition = Camera.main.ScreenToWorldPoint(Infiltrador.position);
            Vector2 GuardiaDistance = (TargetPosition - (Vector2)transform.position);
            Vector2 desiredVelocity = GuardiaDistance.normalized * FollowVelocity;
            Vector2 sterring = desiredVelocity - velocity;

            transform.position += (Vector3)velocity * Time.deltaTime;
        }
    }
    //creamos un private void OnTriggerEnter2D para que el guardia vuelva a la posicion 0,0,0 cuando toque al infiltrador

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Guard.transform.position = new Vector3 (0, 0, 0);
            EstadoAtaque = false;
        }
    }
}
