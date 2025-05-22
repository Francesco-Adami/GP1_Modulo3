using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI enemiesAlive;
    [SerializeField] private GameObject winCondition;

    private void OnEnable()
    {
        SetEnemyText();
    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.Instance.ShowUI(UIManager.GameUI.Pause);
            Door.Instance.PauseGame();
        }
    }

    public void SetEnemyText()
    {
        enemiesAlive.text = Door.Instance.GetEnemyCount().ToString();
        if (Door.Instance.GetEnemyCount() <= 0)
        {
            StartCoroutine(WinCondition());
        }
    }

    private IEnumerator WinCondition()
    {
        while (true)
        {
            winCondition.SetActive(!winCondition.activeInHierarchy);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
