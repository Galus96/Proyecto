using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jefe : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator animator;
    private AdventureState currentState;

    [Header("Vida")]
    [SerializeField] private BossUI barraVidaJefe;
    [SerializeField] private int maxHealth;
    private int Health;

    [Header("Ataque")]
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private int tiempoDisparo = 2;

    private enum AdventureState
    {
        BossIdle,
        BossAtack
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //ChangeState(AdventurerState.Idle);
            //playerInRange = true;
            Atacar();
            //collision.GetComponent<Jugador>().TomarDañoEnemigo();
        }
    }

    private void Atacar()
    {
        Debug.Log("Jugador Detectado");
    }

    private void ChangeState(AdventureState newState)
    {
        if (newState == currentState) return;

        currentState = newState;

        switch (newState)
        {
            case AdventureState.BossIdle:
                animator.SetTrigger("JefeIdle");
                break;
            case AdventureState.BossAtack:
                animator.SetTrigger("JefeAtack");
                break;
        }
    }
}
