using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    //Variables Serializadas
    [SerializeField] public float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float distanceGround;
    [SerializeField] private float speedLadder;
    [SerializeField] private GameObject gameOverMenu;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private SpriteRenderer sr;
    private float xInput;
    private float yInput;
    private bool juegoenCurso = true;

    [Header("Salto")]
    private CapsuleCollider2D bc2D;

    [Header("Escalar")]
    private BoxCollider2D boxCollider2D;
    private float gravedadInicial;
    private bool escalando;

    [Header("Animaciones")]
    private Animator animator;
    private AdventurerState currentState;

    [Header("Vida")]
    [SerializeField] private BarraVidaPlayer barraVidaPlayer;
    [SerializeField] private UIVidas uIVidas;
    [SerializeField] private int maxHealth;
    private int Health;
    private int vidasPersonaje = 3;

    [Header("Ataque")]
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject bala;

    //Posicion del jugador
    float xInicio, yInicio;

    public static Jugador instance;

    private enum AdventurerState
    {
        Idle,
        Running,
        Jumping,
        Climbing,
        Armed,
        Deathing,
        Atacking
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gravedadInicial = rb.gravityScale;

        xInicio = transform.position.x;
        yInicio = transform.position.y;

        Health = maxHealth;
        barraVidaPlayer.UpdateHealthBar(maxHealth, Health);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("VelocidadY", rb.velocity.y);

        Flip();

        if (Input.GetKeyDown(KeyCode.Space) && isGround())
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Atacar();
        }

        if (xInput == 0 && currentState != AdventurerState.Jumping && isGround())
        {
            ChangeState(AdventurerState.Armed);
        }
        else if (Mathf.Abs(xInput) > 0 && isGround())
        {
            ChangeState(AdventurerState.Running);
        }
    }

    private void FixedUpdate()
    {
        Movimiento();
        Escalando();
    }

    private void Movimiento()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        Vector2 move = new Vector2(xInput * speed, rb.velocity.y);
        rb.velocity = move;
    }

    private void Flip()
    {
        if(xInput > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(xInput < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Atacar()
    {
        // Instanciar la bala
        GameObject bulletInstance = Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation);

        // Obtener la escala actual del jugador (derecha o izquierda)
        Vector3 playerScale = transform.localScale;

        // Ajustar la rotación del proyectil según la dirección del jugador
        if (playerScale.x < 0)
        {
            // Invertir la rotación en el eje Y si el jugador mira hacia la izquierda
            bulletInstance.transform.Rotate(new Vector3(0, 180, 0));
        }

        Debug.Log("Atacando");
        ChangeState(AdventurerState.Atacking);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        ChangeState(AdventurerState.Jumping);
    }

    private bool isGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(bc2D.bounds.center, bc2D.bounds.size, 0f, Vector2.down, distanceGround, groundLayer);

        return hit.collider != null;
    }

    private void Escalando()
    {
        yInput = Input.GetAxisRaw("Vertical");

        if((yInput != 0 || escalando) && (bc2D.IsTouchingLayers(LayerMask.GetMask("Ladders"))))
        {
            Vector2 velocidadSubida = new Vector2(rb.velocity.x, yInput * speedLadder);
            rb.velocity = velocidadSubida;
            rb.gravityScale = 0;
            escalando = true;
            ChangeState(AdventurerState.Climbing);
        }
        else
        {
            rb.gravityScale = gravedadInicial;
            escalando = false;
        }

        if (isGround())
        {
            escalando = false;
        }
    }

    public void Reubicar()
    {
        Health = maxHealth;
        barraVidaPlayer.UpdateHealthBar(maxHealth, Health);

        transform.position = new Vector3(xInicio, yInicio, 0);
    }

    //private void OnMouseDown()
    //{
    //    StartCoroutine(GetDamage(daño));
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemigo"))
    //    {
    //        TomarDañoEnemigo(daño);
    //    }
    //}

    public void TomarDañoEnemigo(int daño)
    {
        StartCoroutine(GetDamage(daño));
    }

    IEnumerator GetDamage(int damage)
    {
        if (juegoenCurso)
        {
            float damageDuration = 0.1f;
            //damage = Random.Range(1, 5);
            Health -= damage;

            Debug.Log("Vida: " + Health);

            barraVidaPlayer.UpdateHealthBar(maxHealth, Health);

            if (Health > 0)
            {
                sr.color = Color.red;
                yield return new WaitForSeconds(damageDuration);
                sr.color = Color.white;
            }
            else
            {
                RestarVida();
            }
        }
    }

    public void RestarVida()
    {
        if (vidasPersonaje > 0)
        {
            vidasPersonaje--;
            uIVidas.RestarCorazones(vidasPersonaje);

            Debug.Log("Vidas: " + vidasPersonaje);
            Reubicar();

            if (vidasPersonaje == 0)
            {
                Debug.Log("Game Over");
                juegoenCurso = false; // El juego se detiene

                boxCollider2D.enabled = true;
                capsuleCollider.enabled = false;
                
                ChangeState(AdventurerState.Deathing);
                this.enabled = false;

                gameOverMenu.SetActive(true);
            }
        }
    }

    private void ChangeState(AdventurerState newState)
    {
        if (newState == currentState) return;

        currentState = newState;

        switch (newState)
        {
            case AdventurerState.Idle:
                animator.SetTrigger("PlayerIdle");
                break;
            case AdventurerState.Running:
                animator.SetTrigger("PlayerRun");
                break;
            case AdventurerState.Jumping:
                animator.SetTrigger("PlayerJump");
                break;
            case AdventurerState.Climbing:
                animator.SetTrigger("PlayerClimb");
                break;
            case AdventurerState.Armed:
                animator.SetTrigger("PlayerArmed");
                break;
            case AdventurerState.Deathing:
                animator.SetTrigger("PlayerDeath");
                break;
            case AdventurerState.Atacking:
                animator.SetTrigger("PlayerAtack");
                break;
        }
    }
}
