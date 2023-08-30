using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


[RequireComponent(typeof(ChunkWalls))]
public class Chunk : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject unlockedElements;
    [SerializeField] private GameObject lockedElements;
    [SerializeField] private TextMeshPro priceText;
    private ChunkWalls chunkWalls;

    [Header(" Settings ")]
    [SerializeField] private int initialPrice;
    private int currentPrice;
    private bool unlocked;

    [Header("Actions")]
    public static Action onUnlocked;
    public static Action onPriceChanged;

    private void Awake()
    {
        chunkWalls = GetComponent<ChunkWalls>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(int loadedPrize)
    {
        currentPrice = loadedPrize;
        priceText.text = currentPrice.ToString();

        if (currentPrice <= 0)
            Unlock();
    }

    public void TryUnlock()
    {
        if (CashManager.instance.GetCoins() <= 0)
            return;

        currentPrice--;
        CashManager.instance.UseCoins(1);

        onPriceChanged?.Invoke();

        priceText.text = currentPrice.ToString();

        if (currentPrice <= 0)
            Unlock(false);
    }

    private void Unlock(bool trigerAction = true)
    {
        unlockedElements.SetActive(true);
        lockedElements.SetActive(false);
        unlocked = true;

        if(trigerAction)
            onUnlocked?.Invoke();
    }

    public void UpdateWalls(int configuration)
    {
        chunkWalls.Configure(configuration);
    }


    public bool IsUnlocked()
    {
        return unlocked;
    }

    public int GetInitialPrice()
    {
        return initialPrice;
    }

    public int GetCurrentPrice()
    {
        return currentPrice;
    }
}
