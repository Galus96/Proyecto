using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefeActivacion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Area del jefe");
            GameManager.instance.ActivarJefe();

            //Llamamos entrada del jefe
            StartCoroutine(EsperaJefe());
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    GameManager.instance.JefeDesactivado();
    //    Debug.Log("Saliste del area del jefe");
    //}

    IEnumerator EsperaJefe()
    {
        var velocidadActual = Jugador.instance.speed;
        Jugador.instance.speed = 0;

        yield return new WaitForSeconds(3f);
        Jugador.instance.speed = velocidadActual;

        Destroy(gameObject);
    }
}
