﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownController : MonoBehaviour
{
    public TMP_Dropdown terrainDropdown;
    public TMP_Dropdown objectDropdown;
    public TMP_Dropdown itemDropdown;

    public DungeonCreationManager creationManager;

    void Start()
    {
        // Initialize dropdowns with empty options and hide them
        terrainDropdown.ClearOptions();
        objectDropdown.ClearOptions();
        itemDropdown.ClearOptions();

        terrainDropdown.gameObject.SetActive(false);
        objectDropdown.gameObject.SetActive(false);
        itemDropdown.gameObject.SetActive(false);
    }

    public void OnButtonClick(string mode)
    {
        // Set the current mode in the creation manager
        creationManager.currentMode = (Mode)System.Enum.Parse(typeof(Mode), mode);

        // Hide all dropdowns first
        terrainDropdown.gameObject.SetActive(false);
        objectDropdown.gameObject.SetActive(false);
        itemDropdown.gameObject.SetActive(false);

        // Clear all dropdown options
        terrainDropdown.ClearOptions();
        objectDropdown.ClearOptions();
        itemDropdown.ClearOptions();

        // Determine which dropdown to populate and show based on the button pressed
        switch (mode)
        {
            case "Terrain":
                PopulateDropdown(terrainDropdown, creationManager.GetNamesForCurrentMode());
                terrainDropdown.gameObject.SetActive(true);
                break;
            case "Object":
                PopulateDropdown(objectDropdown, creationManager.GetNamesForCurrentMode());
                objectDropdown.gameObject.SetActive(true);
                break;
            case "Item":
                PopulateDropdown(itemDropdown, creationManager.GetNamesForCurrentMode());
                itemDropdown.gameObject.SetActive(true);
                break;
            default:
                Debug.LogError("Invalid mode!");
                break;
        }
    }

    public void OnDropdownValueChanged(int index)
    {
        string selectedName = "";
        switch (creationManager.currentMode)
        {
            case Mode.Terrain:
                selectedName = terrainDropdown.options[index].text;
                break;
            case Mode.Object:
                selectedName = objectDropdown.options[index].text;
                break;
            case Mode.Item:
                selectedName = itemDropdown.options[index].text;
                break;
        }
        creationManager.SetSelectedPrefab(selectedName);
    }

    private void PopulateDropdown(TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.AddOptions(options);
    }
}