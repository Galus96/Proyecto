using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaJefe : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private int Damage;

    private Rigidbody2D rb;
    private Vector3 Direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Translate(Direction * Speed * Time.deltaTime);
    }

    public void SetDirection(Vector3 newDirection)
    {
        Direction = newDirection;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Jugador>().TomarDañoEnemigo(Damage);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
