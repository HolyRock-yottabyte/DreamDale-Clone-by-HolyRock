using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform world;
    Chunk[,] grid;

    [Header("Data")]
    private WorldData worldData;
    private string dataPath;
    private bool shouldSave;

    [Header("Data")]
    [SerializeField] private int gridSize;
    [SerializeField] private int gridScale;


    // Start is called before the first frame update
    void Start()
    {
        dataPath = Application.dataPath + "/WorldData.txt";
        LoadWorld();
        Initialize();

        InvokeRepeating("TrySaveGame", 1, 1);
    }

    private void InitializeGrid()
    {
        grid = new Chunk[gridSize, gridSize];

        for (int i = 0; i< world.childCount; i++)
        {
            Chunk chunk = world.GetChild(i).GetComponent<Chunk>();

            Vector2Int chunkGridPosition = new Vector2Int((int)chunk.transform.position.x / gridScale,
                (int)chunk.transform.position.z / gridScale);

            chunkGridPosition += new Vector2Int(gridSize / 2, gridSize / 2);

            grid[chunkGridPosition.x, chunkGridPosition.y] = chunk;

        }
    }

    private void Initialize()
    {
        for (int i = 0; i < world.childCount; i++)
            world.GetChild(i).GetComponent<Chunk>().Initialize(worldData.chunkPrices[i]);

        InitializeGrid();

        UpdateChunkWalls();
    }

    private void UpdateChunkWalls()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];

                if (chunk == null)
                    continue;

                Chunk frontChunk = null;

                if (IsValidGridPosition(x, y + 1))
                    frontChunk = grid[x, y + 1];

                Chunk rightChunk = null;

                if (IsValidGridPosition(x + 1, y))
                    rightChunk = grid[x, y + 1];
                Chunk backChunk = null;

                if (IsValidGridPosition(x, y - 1))
                    backChunk = grid[x, y + 1];
                Chunk leftChunk = null;

                if (IsValidGridPosition(x - 1, y))
                    leftChunk = grid[x, y + 1];

                int configuration = 0;

                if (frontChunk !=null && frontChunk.IsUnlocked())
                    configuration = configuration + 1;

                if (rightChunk != null && rightChunk.IsUnlocked())
                    configuration = configuration + 2;

                if (backChunk != null && backChunk.IsUnlocked())
                    configuration = configuration + 4;

                if (leftChunk != null && leftChunk.IsUnlocked())
                    configuration = configuration + 8;

                chunk.UpdateWalls(configuration);
            }
        }
    }

    private bool IsValidGridPosition(int x, int y)
    {
        if (x < 0 || x >= gridSize || y < 0 || y >= gridSize)
            return false;

        return true;
    }


    private void Awake()
    {
        Chunk.onUnlocked += ChunkUnlockedCallback;
        Chunk.onPriceChanged += ChunkPriceChangedCallback;
    }

    private void OnDestroy()
    {
        Chunk.onUnlocked -= ChunkUnlockedCallback;
        Chunk.onPriceChanged -= ChunkPriceChangedCallback;

    }

    private void ChunkUnlockedCallback()
    {
        Debug.Log("Chunk Unlocked !");

        SaveWorld();
    }

    private void ChunkPriceChangedCallback()
    {
        shouldSave = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TrySaveGame()
    {

        Debug.Log("Trying to save");
        if (shouldSave)
        {
            SaveWorld();
            shouldSave = false;

        }
    }

    private void LoadWorld()
    {
        string data = "";

        if (!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create);

            worldData = new WorldData();

            for(int i = 0; i < world.childCount; i++)
            {
                int chunkInitialPrice = world.GetChild(i).GetComponent<Chunk>().GetInitialPrice();
                worldData.chunkPrices.Add(chunkInitialPrice);

            }

            string worldDataString = JsonUtility.ToJson(world, true);

            byte[] worldDataBytes = Encoding.UTF8.GetBytes(worldDataString);

            fs.Write(worldDataBytes);

            fs.Close();
        }

        else
        {
            data = File.ReadAllText(dataPath);
            worldData = JsonUtility.FromJson<WorldData>(data);

            if (worldData.chunkPrices.Count < world.childCount)
                UpdateData();
        }

    }

    private void UpdateData()
    {
        int missingData = world.childCount - worldData.chunkPrices.Count;

        for(int i = 0; i < missingData; i++)
        {
            int chunkIndex = world.childCount - missingData + i;
            int chunkPrice = world.GetChild(chunkIndex).GetComponent<Chunk>().GetInitialPrice();
        }
    }

    private void SaveWorld()
    {
        if (worldData.chunkPrices.Count != world.childCount)
            worldData = new WorldData();

        for (int i = 0; i < world.childCount; i++)
        {
            int chunkCurrentPrice = world.GetChild(i).GetComponent<Chunk>().GetInitialPrice();

            if (worldData.chunkPrices.Count > i)
                worldData.chunkPrices.Add(chunkCurrentPrice);
            else
                worldData.chunkPrices.Add(chunkCurrentPrice);

        }

        string data = JsonUtility.ToJson(worldData, true);

        File.WriteAllText(dataPath, data);

        Debug.LogWarning("Data Saved");
    }
}
