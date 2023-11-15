using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{
    [Header("Dialogo")]
    [SerializeField, TextArea(4, 6)] private string[] lineasDialogo;
    [SerializeField] private GameObject dialogoPanel;
    [SerializeField] private TMP_Text dialogoTexto;
    [SerializeField] private GameObject imageMark;

    private float typingTime = 0.05f;

    private bool isPlayerinRange;
    private bool didDialogoStart;
    private int lineIndex;

    // Update is called once per frame
    void Update()
    {
        if(isPlayerinRange && Input.GetButtonDown("Fire1"))
        {
            if (!didDialogoStart)
            {
                StartDialogo();
            }else if(dialogoTexto.text == lineasDialogo[lineIndex])
            {
                SiguienteDialogo();
            }
            else
            {
                StopAllCoroutines();
                dialogoTexto.text = lineasDialogo[lineIndex];
            }
        }
    }

    private void StartDialogo()
    {
        didDialogoStart = true;
        dialogoPanel.SetActive(true);
        lineIndex = 0;

        StartCoroutine(ShowLine());
    }

    private IEnumerator ShowLine()
    {
        dialogoTexto.text = string.Empty;

        foreach(char ch in lineasDialogo[lineIndex])
        {
            dialogoTexto.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }

    private void SiguienteDialogo()
    {
        lineIndex++;
        if(lineIndex < lineasDialogo.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialogoStart = false;
            dialogoPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerinRange = true;
            imageMark.SetActive(true);
            Debug.Log("Puedes iniciar un dialogo");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerinRange = false;
        imageMark.SetActive(false);
        Debug.Log("No puedes iniciar un dialogo");
    }
}
