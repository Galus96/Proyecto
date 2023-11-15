using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private int da�o;

    private Rigidbody2D rb;
    private Vector3 Direction;

    private void Update()
    {
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            other.GetComponent<PatrullaEnemigo>().TomarDa�os(da�o);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
