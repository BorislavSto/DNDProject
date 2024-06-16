using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownController : MonoBehaviour
{
    public TMP_Dropdown terrainDropdown;
    public TMP_Dropdown objectDropdown;
    public TMP_Dropdown itemDropdown;

    public DungeonCreationManager creationManager;

    private void Awake()
    {
        creationManager.OnModeChanged += CreationManager_OnModeChanged;
    }

    private void OnDestroy()
    {
        creationManager.OnModeChanged -= CreationManager_OnModeChanged;
    }

    void Start()
    {
        // Initialize dropdowns with empty options and hide them
        terrainDropdown.ClearOptions();
        objectDropdown.ClearOptions();
        itemDropdown.ClearOptions();

        terrainDropdown.gameObject.SetActive(false);
        objectDropdown.gameObject.SetActive(false);
        itemDropdown.gameObject.SetActive(false);

        terrainDropdown.onValueChanged.AddListener(OnTerrainDropdownValueChanged);
        objectDropdown.onValueChanged.AddListener(OnObjectDropdownValueChanged);
        itemDropdown.onValueChanged.AddListener(OnItemDropdownValueChanged);
    }

    public void OnButtonClick(string mode)
    {
        Debug.LogWarning(mode);
        // Set the current mode in the creation manager
        creationManager.SetCurrentMode(mode);

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
                OnTerrainDropdownValueChanged(0);
                break;
            case "Object":
                PopulateDropdown(objectDropdown, creationManager.GetNamesForCurrentMode());
                objectDropdown.gameObject.SetActive(true);
                OnObjectDropdownValueChanged(0);
                break;
            case "Item":
                PopulateDropdown(itemDropdown, creationManager.GetNamesForCurrentMode());
                itemDropdown.gameObject.SetActive(true);
                OnItemDropdownValueChanged(0);
                break;
            default:
                Debug.LogError("Invalid mode!");
                break;
        }
    }

    private void CreationManager_OnModeChanged(Mode mode)
    {
        if (mode == Mode.None)
        {
            terrainDropdown.gameObject.SetActive(false);
            objectDropdown.gameObject.SetActive(false);
            itemDropdown.gameObject.SetActive(false);
        }
    }

    public void OnTerrainDropdownValueChanged(int index)
    {
        string selectedName = terrainDropdown.options[index].text;
        creationManager.SetSelectedPrefab(selectedName);
    }

    public void OnObjectDropdownValueChanged(int index)
    {
        string selectedName = objectDropdown.options[index].text;
        creationManager.SetSelectedPrefab(selectedName);
    }

    public void OnItemDropdownValueChanged(int index)
    {
        string selectedName = itemDropdown.options[index].text;
        creationManager.SetSelectedPrefab(selectedName);
    }

    private void PopulateDropdown(TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.AddOptions(options);
    }
}
