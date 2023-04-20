using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour 
{
    public static GridManager Instance;
    public EventHandler OnGridSpawned;
    [SerializeField] private GameObject gridObjectPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.green, 1000);
                Instantiate(gridObjectPrefab, GetWorldPosition(x, z), Quaternion.identity, this.transform);
            }
        }
        OnGridSpawned?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetWorldPosition(int x, int z) {
        return new Vector3(x, 0, z) * cellSize;
    }
}
