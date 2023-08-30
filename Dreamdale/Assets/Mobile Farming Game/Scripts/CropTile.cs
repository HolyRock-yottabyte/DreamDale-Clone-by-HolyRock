using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CropTile : MonoBehaviour
{
    private TileFieldState state;

    [Header(" Elements ")]
    [SerializeField] private Transform cropParent;
    [SerializeField] private MeshRenderer tileRenderer;
    private Crop crop;
    private CropData cropData;

    [Header(" Events ")]
    public static Action<CropType> onCropHarvested;

    // Start is called before the first frame update
    void Start()
    {
        state = TileFieldState.Empty;
    }

    public void Sow(CropData cropData)
    {
        state = TileFieldState.Sown;
        crop = Instantiate(cropData.cropPrefab, transform.position, Quaternion.identity, cropParent);

        this.cropData = cropData;
    }

    public void Water()
    {
        state = TileFieldState.Watered;
        crop.ScaleUp();
        tileRenderer.gameObject.LeanColor(Color.white * .3f, 1);
    }

    public void Harvest()
    {
        state = TileFieldState.Empty;
        crop.ScaleDown();
        tileRenderer.gameObject.LeanColor(Color.white, 1);

        onCropHarvested?.Invoke(cropData.cropType);
    }

    public bool IsEmpty()
    {
        return state == TileFieldState.Empty;
    }

    public bool IsSown()
    {
        return state == TileFieldState.Sown;
    }
}
