using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrullaEnemigo : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private float velocidadMov;
    [SerializeField] private Transform[] puntosMov;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private AdventurerState currentState;
    private int siguientePaso = 0;

    [Header("Vida")]
    [SerializeField] private BarraVida barraVidaEnemigo;
    [SerializeField] private int maxHealth;
    private int Health;

    private bool isDamage = false;
    private bool playerInRange = false;

    [Header("Ataque")]
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private int tiempoDisparo = 2;

    private bool canShoot = true;

    private enum AdventurerState
    {
        Idle,
        Walking
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth;
        barraVidaEnemigo.UpdateHelathBar(maxHealth, Health);

        //Prueba
        //ChangeState(AdventurerState.Walking);
        Girar();
        //Patrulla();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            ChangeState(AdventurerState.Idle);

            if (canShoot)
            {
                StartCoroutine(Disparar());
            }
        }
        else
        {
            Patrulla();
        }
        
    }

    private void Patrulla()
    {
        ChangeState(AdventurerState.Walking);
        transform.position = Vector2.MoveTowards(transform.position, puntosMov[siguientePaso].position, velocidadMov * Time.deltaTime);

        if (Vector2.Distance(transform.position, puntosMov[siguientePaso].position) < 1)
        {
            siguientePaso += 1;

            if(siguientePaso >= puntosMov.Length)
            {
                siguientePaso = 0;
            }

            Girar();
        }
    }

    private void Girar()
    {
        if (transform.position.x < puntosMov[siguientePaso].position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            //spriteRenderer.flipX = true;
            //boxCollider.size = new Vector2(Mathf.Abs(boxCollider.size.x), boxCollider.size.y);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            //spriteRenderer.flipX = false;
            //boxCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //ChangeState(AdventurerState.Idle);
            playerInRange = true;
            Atacar();
            //collision.GetComponent<Jugador>().TomarDañoEnemigo();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //ChangeState(AdventurerState.Walking);
            playerInRange = false;
        }
    }

    private void Atacar()
    {
        Debug.Log("Jugador Detectado");
    }

    public void TomarDaños(int daño)
    {
        if (!isDamage)
        {
            StartCoroutine(DamageFlash());
            Health -= daño;

            barraVidaEnemigo.UpdateHelathBar(maxHealth, Health);

            if (Health <= 0)
            {
                Muerte();
            }
        }
    }

    IEnumerator DamageFlash()
    {
        isDamage = true;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;

        isDamage = false;
    }

    IEnumerator Disparar()
    {
        canShoot = false;

        GameObject bulletInstance = Instantiate(balaPrefab, controladorDisparo.position, controladorDisparo.rotation);
        bulletInstance.GetComponent<BalaEnemigo>().SetDirection(controladorDisparo.right);

        Vector3 playerScale = transform.localScale;

        // Ajustar la rotación del proyectil según la dirección del jugador
        if (playerScale.x < 0)
        {
            // Invertir la rotación en el eje Y si el jugador mira hacia la izquierda
            bulletInstance.transform.Rotate(new Vector3(0, 180, 0));
        }

        yield return new WaitForSeconds(tiempoDisparo);

        canShoot = true;
    }

    private void Muerte()
    {
        Destroy(gameObject);
    }

    private void ChangeState(AdventurerState newState)
    {
        if (newState == currentState) return;

        currentState = newState;

        switch (newState)
        {
            case AdventurerState.Idle:
                animator.SetTrigger("EnemyIdle");
                break;
            case AdventurerState.Walking:
                animator.SetTrigger("EnemyWalk");
                break;
        }
    }
}
