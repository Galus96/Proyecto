using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Panel Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;

    [Header("Panel Jefe")]
    [SerializeField] private GameObject jefePanel;
    [SerializeField] private GameObject muros;
    [SerializeField] private GameObject escaleras;
    [SerializeField] private GameObject jefe;

    public static GameManager instance;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        JefeDesactivado();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PauseButton();
        }
    }

    public void PauseButton()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayButton()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void PlayRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ActivarJefe()
    {
        jefePanel.SetActive(true);
        muros.SetActive(true);
        escaleras.SetActive(false);
        jefe.SetActive(true);
    }

    public void JefeDesactivado()
    {
        jefePanel.SetActive(false);
        muros.SetActive(false);
        escaleras.SetActive(true);
    }
}
