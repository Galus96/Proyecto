using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVidas : MonoBehaviour
{
    [Header("Vidas")]
    [SerializeField] private List<GameObject> ListaCorazones;
    [SerializeField] private Sprite corazonDesactivado;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RestarCorazones(int indice)
    {
        Image imageHearth = ListaCorazones[indice].GetComponent<Image>();
        imageHearth.sprite = corazonDesactivado;
    }
}
