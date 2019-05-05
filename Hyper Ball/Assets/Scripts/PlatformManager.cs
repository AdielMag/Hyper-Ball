using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class PlatformManager : SerializedMonoBehaviour
{
    public Text scoreText;
    float score;

    public int tilesCount;
    readonly int maxTilesCount = 200;

    [HideInInspector]
    public bool gameIsRunning, pauseGame;
    float platformSpeed;
    [Range(1,7)]
    public float platformSpeedMultiplier;

    ObjectPooler objP;
    PlayerController pCon;

    private void Start()
    {
        objP = ObjectPooler.instance;
        pCon = PlayerController.instance;

        scoreText.text = 0.ToString();
    }

    #region Singelton
    public static PlatformManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    void FixedUpdate()
    {
        if (!gameIsRunning || pauseGame)
            return;

        scoreText.text = Mathf.Round(score++ / 60).ToString();

        platformSpeed = (platformSpeed >= .65f) ? .65f : platformSpeed + (platformSpeedMultiplier / 10000);

        transform.Translate(-transform.forward * platformSpeed);

        SpawnTiles();
    }

    public void StartNewRun()
    {
        score = 0;
        platformSpeed = 0.04f;
        gameIsRunning = true;
        transform.position = Vector3.zero;

        pCon.transform.position = Vector3.zero + Vector3.up * 1;
        pCon.lane = 0;

        newTileZpos = 0;
        pCon.gameObject.SetActive(true);
    }

    public void LostRun() 
    {
        RegualrTile[] tiles = FindObjectsOfType<RegualrTile>();

        for (int x = 0; x < tiles.Length; x++)
            tiles[x].GetComponent<Animator>().SetTrigger("Fall");

        gameIsRunning = false;
        // show score and suggest double coin by watching ads or play again or main menu.
    }

    int newTileZpos = 0;
    float spawnTimer;
    private void SpawnTiles()
    {

        if (tilesCount >= maxTilesCount || pauseGame)
            return;

        if (Time.time < spawnTimer)
            return;

        #region Tutorial
        if (newTileZpos < 10)
        {
            for (int x = -1; x < 2; x++)
            {
                objP.SpawnFromPool("GreenTile", Quaternion.identity, new Vector3(x, 0, newTileZpos), transform);
            }
            newTileZpos++;
            UpdateSpawnTimer(.05f);
            return;
        }   // Green Tiles
        if (newTileZpos < 20)
        {
            for (int x = -2; x < 3; x++)
            {
                objP.SpawnFromPool("BlueTile", Quaternion.identity, new Vector3(x, 0, newTileZpos), transform);
            }
            newTileZpos++;
            UpdateSpawnTimer(.05f);
            return;
        }   // Blue Tiles
        #endregion

        TilesSpawnerBrain();
    }

    void UpdateSpawnTimer(float timeToWait)
    {
        spawnTimer = Time.time + timeToWait;
    }

    int pathX, pathZ, oldPathX;
    string tileType;

    void TilesSpawnerBrain()
    {
        if (pathZ == 0)
            GenerateNewPath();

        for (int z = 0; z < pathZ; z++)
        {
            for (int x = 0; x < pathX; x++)
            {
                if (tileType == "Empty") { }

                else
                objP.SpawnFromPool(tileType, Quaternion.identity, new Vector3(x + pathXoffset, 0, newTileZpos), transform);
            }
            newTileZpos++;
            pathZ--;
            UpdateSpawnTimer(0);
        }
    }

    int pathXoffset, oldPathXoffset;
    int pathXrangeMax = 6;
    bool newPathCompatible;
    void GenerateNewPath()
    {
        newPathCompatible = false;

        while (!newPathCompatible)
        {
            pathXrangeMax = 6;
            int tempTileType = Random.Range(1, 8);

            pathXoffset = Random.Range(-2, 3); // 2

            switch (pathXoffset)
            {
                case -1:
                    pathXrangeMax -= 1;
                    break;
                case 0:
                    pathXrangeMax -= 2;
                    break;
                case 1:
                    pathXrangeMax -= 3;
                    break;
                case 2:
                    pathXrangeMax -= 4;
                    break;
            }

            pathZ = Random.Range(3, 9);
            pathX = Random.Range(1, pathXrangeMax); //4


            if (tempTileType < 5)
                tileType = "GreenTile";
            else if (tempTileType < 7)
                tileType = "BlueTile";
            else
            {
                tileType = "Empty";
                pathZ = Random.Range(1, 3);
            }
            if (oldPathX == 0)
            {
                oldPathX = pathX + pathXoffset;
                oldPathXoffset = pathXoffset;
            }

            for (int x = 0; x < pathX; x++) 
            {
                for(int oX = 0; oX < oldPathX; oX++) 
                {
                    newPathCompatible |= x + pathXoffset == oX + oldPathXoffset;
                }
            }
        }

        oldPathX = pathX;
        oldPathXoffset = pathXoffset;
    }


    public void PausePlayGame(bool pause)
    {
        pauseGame = pause;
    }
}
