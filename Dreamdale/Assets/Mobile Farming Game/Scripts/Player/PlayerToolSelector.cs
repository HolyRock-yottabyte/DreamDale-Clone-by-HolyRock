using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerToolSelector : MonoBehaviour
{
    public enum Tool { None, Sow, Water, Harvest }
    private Tool activeTool;

    [Header(" Elements ")]
    [SerializeField] private Image[] toolImages;

    [Header(" Settings ")]
    [SerializeField] private Color selectedToolColor;

    [Header(" Actions ")]
    public Action<Tool> onToolSelected;

    // Start is called before the first frame update
    void Start()
    {
        SelectTool(0);
    }

    public void SelectTool(int toolIndex)
    {
        activeTool = (Tool)toolIndex;

        for (int i = 0; i < toolImages.Length; i++)
            toolImages[i].color = i == toolIndex ? selectedToolColor : Color.white;

        onToolSelected?.Invoke(activeTool);
    }

    public bool CanSow()
    {
        return activeTool == Tool.Sow;
    }

    public bool CanWater()
    {
        return activeTool == Tool.Water;
    }

    public bool CanHarvest()
    {
        return activeTool == Tool.Harvest;
    }
}
