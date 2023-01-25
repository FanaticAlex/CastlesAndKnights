using Assets.Scripts;
using Assets.Scripts.Menu;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Menu handlers script
/// init menu frames and buttons
/// </summary>
public class MainMenu : MonoBehaviour
{
    private static List<BaseMenuManager> menuManagers;
    public static bool IsWaitingForStart;
    public static bool IAmGameMaster;

    private List<BaseMenuManager> GetMenuManagers()
    {
        return GameObject.FindObjectsOfType<BaseMenuManager>(true).ToList();
    }

    void Start()
    {
        menuManagers = GetMenuManagers();
        SwitchToMenuPanel(MenuPanel.Login);
        GameManager.Instance.SetOnlineMode();
    }

    public static void SwitchToMenuPanel(MenuPanel menu)
    {
        foreach(var manager in menuManagers)
        {
            if (menu == manager.MenuPanel)
                manager.Enable();
            else
                manager.Disable();
        }
    }

    
}