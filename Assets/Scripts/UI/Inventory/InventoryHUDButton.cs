using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUDButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Keybind;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private UiInventory inventoryMenu;

    // on Awake
    private void Awake()
    {
        // when the ability button is clicked, it will toggle the ability menu
        inventoryButton.onClick.AddListener(() => {
            if(!inventoryMenu.GetIsMenuOpen())
            {
                inventoryMenu.Show();
            }
            else
            {
                inventoryMenu.Hide();
            }
        });
    }
}
