using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [SerializeField] private Image barImage;

    public void UpdateHelathBar(float maxHealth, float health)
    {
        barImage.fillAmount = health / maxHealth;
    }
}
