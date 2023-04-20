using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject hunterPrefab;
    [SerializeField] private GameObject preyPrefab;
    [SerializeField] private LayerMask gridLayerMask;

    private List<GridObject> availableSpawnLocations = new List<GridObject>();
    public EventHandler OnHunterTurn, OnPreyTurn; 
    public bool autoplay = false;
    public int turnCount;
    public int preyCount;

    private void Awake() {
        Instance = this;
        
    }

    private void Start() {
        GridManager.Instance.OnGridSpawned += GridManager_OnGridSpawned;
    }

    private void GridManager_OnGridSpawned(object sender, EventArgs e) {
        PrepareSpawn();
        SpawnHunter();
        Hunter.Instance.OnHunterPlayed += Hunter_OnHunterPlayed;
        Hunter.Instance.OnPreyKilled += Hunter_OnPreyKilled;
        SpawnPrey();
    }

    private void Hunter_OnPreyKilled(object sender, EventArgs e) {
        preyCount--;
    }

    private void Hunter_OnHunterPlayed(object sender, EventArgs e) {
        OnPreyTurn?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        if (preyCount <= 0) return;
        if (Input.GetKeyDown(KeyCode.Space)) autoplay = !autoplay;
        if (Input.GetKeyDown(KeyCode.T)) {
            OnHunterTurn?.Invoke(this, EventArgs.Empty);
            turnCount++;
        }

        if (autoplay) {
            OnHunterTurn?.Invoke(this, EventArgs.Empty);
            turnCount++;
        }
    }

    private void PrepareSpawn() {
        foreach (Transform child in GridManager.Instance.transform) {
            GridObject childGridObject = child.GetComponent<GridObject>();
            availableSpawnLocations.Add(childGridObject);
        }
    }

    private void SpawnHunter() {
        int spawnPos = UnityEngine.Random.Range(0, availableSpawnLocations.Count);
        GridObject selectedGrid = availableSpawnLocations[spawnPos];
        Instantiate(hunterPrefab, selectedGrid.transform.position, Quaternion.identity);

        Collider[] cols = Physics.OverlapSphere(selectedGrid.transform.position, 8f, gridLayerMask);
        foreach (Collider collider in cols) {
            if (collider.gameObject.TryGetComponent<GridObject>(out GridObject gridObject)) {
                availableSpawnLocations.Remove(gridObject);
            }
        }
    }

    private void SpawnPrey() {
        int preyToSpawn = UnityEngine.Random.Range(5, 10);
        preyCount = preyToSpawn;

        for (int i = 0; i < preyToSpawn; i++) {
            int spawnPos = UnityEngine.Random.Range(0, availableSpawnLocations.Count);
            GridObject selectedGrid = availableSpawnLocations[spawnPos];
            Instantiate(preyPrefab, selectedGrid.transform.position, Quaternion.identity);

            Collider[] cols = Physics.OverlapSphere(selectedGrid.transform.position, 2f, gridLayerMask);
            foreach (Collider collider in cols) {
                if (collider.gameObject.TryGetComponent<GridObject>(out GridObject gridObject)) {
                    availableSpawnLocations.Remove(gridObject);
                }
            }
        }
    }

}
