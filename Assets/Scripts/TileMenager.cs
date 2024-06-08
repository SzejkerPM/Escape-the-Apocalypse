using System.Collections.Generic;
using UnityEngine;

public class TileMenager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private GameObject[] tilePrefabsTutorial;

    [SerializeField]
    private int maxTilesOnScreen;

    private List<GameObject> activeTiles = new();
    private float tileSpawnZ = 0f;
    //private float firstTileZ;
    private readonly float tileLength = 40f;
    private int previousTile = 1;

    [SerializeField]
    private GameStatesSO gameStates;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private TutorialManager tutorialManager;

    void Start()
    {
        if (!gameStates.isThisTutorial)
        {
            SpawnFirstTiles();
        }
        else
        {
            SpawnFirstTilesTutorial();
        }

    }

    void Update()
    {
        if (!gameStates.isThisTutorial)
        {
            SpawnEndless();
        }
        else
        {
            SpawnEndlessTutorial();
        }
    }

    private void SpawnTile(int tileIndex)
    {
        if (tileIndex != previousTile)
        {
            activeTiles.Add(Instantiate(tilePrefabs[tileIndex], transform.forward * tileSpawnZ, transform.rotation));
            tileSpawnZ += tileLength;
            previousTile = tileIndex;
        }
        else
        {
            SpawnTile(Random.Range(1, tilePrefabs.Length));
        }
    }

    private void SpawnTileTutorial(int tileIndex)
    {
        if (tutorialManager.IsInStage)
        {
            activeTiles.Add(Instantiate(tilePrefabsTutorial[tileIndex], transform.forward * tileSpawnZ, transform.rotation));
            tileSpawnZ += tileLength;
        }
        else
        {
            activeTiles.Add(Instantiate(tilePrefabsTutorial[0], transform.forward * tileSpawnZ, transform.rotation));
            tileSpawnZ += tileLength;
        }

    }

    private void SpawnFirstTiles()
    {
        player.GetComponent<PlayerController>().SetPlayerPositionAfterFirstAid();

        for (int i = 0; i < maxTilesOnScreen; i++)
        {
            if (i == 0)
            {
                SpawnTile(0);
            }
            else
            {
                SpawnTile(Random.Range(1, tilePrefabs.Length));
            }
        }
    }

    private void SpawnFirstTilesTutorial()
    {
        for (int i = 0; i < maxTilesOnScreen; i++)
        {
            if (i == 0)
            {
                SpawnTileTutorial(0);
            }
            else
            {
                SpawnTileTutorial(tutorialManager.CurrentMap);
            }
        }
    }

    private void SpawnEndless()
    {
        if (player.position.z - tileLength > tileSpawnZ - (maxTilesOnScreen * tileLength))
        {
            SpawnTile(Random.Range(1, tilePrefabs.Length));
            DeletePassedTiles();
        }
    }

    private void SpawnEndlessTutorial()
    {
        if (player.position.z - tileLength > tileSpawnZ - (maxTilesOnScreen * tileLength))
        {
            SpawnTileTutorial(tutorialManager.CurrentMap);
            DeletePassedTiles();
        }
    }

    private void DeletePassedTiles()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    internal void TutorialGenerateMapForNextStage()
    {
        foreach (var tile in activeTiles)
        {
            Destroy(tile);
        }

        activeTiles.Clear();

        tileSpawnZ = player.GetComponent<PlayerController>().GetPlayerPositionZ() + 6;

        SpawnFirstTilesTutorial();
    }
}
