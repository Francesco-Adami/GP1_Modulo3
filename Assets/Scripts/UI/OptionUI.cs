using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : BaseUI
{
    public void GoToMainMenu()
    {
        UIManager.Instance.ShowUI(UIManager.GameUI.MainMenu);
        // dipende se serve ricaricare la scena
        // SceneManager.LoadScene(0);
    }
}
