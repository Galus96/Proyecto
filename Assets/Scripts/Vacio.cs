using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacio : MonoBehaviour
{
    private Jugador jugador;

    private void Start()
    {
        jugador = FindObjectOfType<Jugador>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //FindObjectOfType<Jugador>().Reubicar();

        if(collision.CompareTag("Player"))
        {
            jugador.Reubicar();
            jugador.RestarVida();
        }
    }
}
