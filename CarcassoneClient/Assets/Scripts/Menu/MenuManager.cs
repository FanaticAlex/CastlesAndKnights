using Assets.Scripts;
using Assets.Scripts.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Инициализирует окна меню и управляет их переключением.
/// </summary>
public class MenuManager : MonoBehaviour
{
    private static List<BaseMenuWindowController> menuManagers;
    public static bool IAmGameMaster { get; set; }

    private List<BaseMenuWindowController> GetMenuControllers()
    {
        return GameObject.FindObjectsOfType<BaseMenuWindowController>(true).ToList();
    }

    void Awake()
    {
        menuManagers = GetMenuControllers();
        SwitchToMenuPanel(MenuWindowType.Login);
        GameObject.Find("Version").GetComponent<TMP_Text>().text = "Version : " + Application.version;
    }

    public static void SwitchToMenuPanel(MenuWindowType menu)
    {
        menuManagers.ForEach(manager => { manager.Disable(); });
        menuManagers.Single(manager => (manager.MenuPanelType == menu)).Enable();
    }
}