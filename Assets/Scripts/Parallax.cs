using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMov;

    private Vector2 offset;
    private Material material;
    private Rigidbody2D rbplayer;

    void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        rbplayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        offset = (rbplayer.velocity * 0.1f) * velocidadMov * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
