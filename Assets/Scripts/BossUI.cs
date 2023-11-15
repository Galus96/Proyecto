using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    //[SerializeField] private GameObject jefePanel;
    //[SerializeField] private GameObject muros;

    [SerializeField] private Image barImage;

    public static BossUI instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void UpdateHealthBar(float maxHealth, float health)
    {
        barImage.fillAmount = health / maxHealth;
    }
}
