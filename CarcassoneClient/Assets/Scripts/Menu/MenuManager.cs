using Assets.Scripts;
using Assets.Scripts.Menu;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Инициализирует окна меню и управляет их переключением.
/// </summary>
public class MenuManager : MonoBehaviour
{
    private static List<BaseMenuWindowController> menuManagers;
    public static bool IsWaitingForStart { get; set; }
    public static bool IAmGameMaster { get; set; }

    private List<BaseMenuWindowController> GetMenuControllers()
    {
        return GameObject.FindObjectsOfType<BaseMenuWindowController>(true).ToList();
    }

    void Start()
    {
        menuManagers = GetMenuControllers();
        SwitchToMenuPanel(MenuWindowType.Login);
        GameManager.Instance.SetOnlineMode();
    }

    public static void SwitchToMenuPanel(MenuWindowType menu)
    {
        foreach(var manager in menuManagers)
        {
            if (menu == manager.MenuPanelType)
                manager.Enable();
            else
                manager.Disable();
        }
    }

    
}