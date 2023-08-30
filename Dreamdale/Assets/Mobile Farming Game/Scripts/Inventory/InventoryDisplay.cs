using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform cropContainersParent;
    [SerializeField] private UICropContainer uiCropContainerPrefab;
 
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Configure(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer cropContainerInstance = Instantiate(uiCropContainerPrefab, cropContainersParent);

            Sprite cropIcon = DataManager.instance.GetCropSpriteFromCropType(items[i].cropType);

            cropContainerInstance.Configure(cropIcon, items[i].amount);
        }
    }

    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer containerInstance;

            if (i < cropContainersParent.childCount)
            {
                containerInstance = cropContainersParent.GetChild(i).GetComponent<UICropContainer>();
                containerInstance.gameObject.SetActive(true);
            }
            else
                containerInstance = Instantiate(uiCropContainerPrefab, cropContainersParent);
            

            Sprite cropIcon = DataManager.instance.GetCropSpriteFromCropType(items[i].cropType);
            containerInstance.Configure(cropIcon, items[i].amount);
        }

        int remainingContainers = cropContainersParent.childCount - items.Length;

        if (remainingContainers <= 0)
            return;

        for (int i = 0; i < remainingContainers; i++)
            cropContainersParent.GetChild(items.Length + i).gameObject.SetActive(false);
        
    }

        /*
        public void UpdateDisplay(Inventory inventory)
        {
            InventoryItem[] items = inventory.GetInventoryItems();

            // Clear the Crop Containers Parent if there are any ui crop containers
            while(cropContainersParent.childCount > 0)
            {
                Transform container = cropContainersParent.GetChild(0);
                container.SetParent(null);
                Destroy(container.gameObject);
            }

            // Create the UI Crop Containers from scratch again
            Configure(inventory);

            /*
            for (int i = 0; i < items.Length; i++)
            {
                UICropContainer cropContainerInstance = Instantiate(uiCropContainerPrefab, cropContainersParent);

                Sprite cropIcon = DataManager.instance.GetCropSpriteFromCropType(items[i].cropType);

                cropContainerInstance.Configure(cropIcon, items[i].amount);
            }

        }
        */
    }
