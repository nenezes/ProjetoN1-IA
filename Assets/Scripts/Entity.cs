using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public List<GridObject> availableMoves = new List<GridObject>();
    [SerializeField] public Transform moveCheckers;
    [SerializeField] public LayerMask gridObjectsLayer;


    virtual public void Update() {

    }

    public void CheckAvailableMoves() {
        availableMoves.Clear();

        foreach (Transform child in moveCheckers) {
            if (!Physics.Raycast(new Ray(child.transform.position, Vector3.down), out RaycastHit hit, gridObjectsLayer)) continue;

            if (hit.collider.gameObject.TryGetComponent<GridObject>(out GridObject availableMove)) {
                //GridObject availableMove = hit.collider.gameObject.GetComponent<GridObject>();
                Debug.Log(availableMove);
                availableMoves.Add(availableMove);
            }
        }
    }

    public void MoveRandom() {
        if (availableMoves.Count > 0) {
            GridObject selectedMove = availableMoves[Random.Range(0, availableMoves.Count)];
            Debug.Log(selectedMove);
            Vector3 newPos = new Vector3(selectedMove.transform.position.x, 0, selectedMove.transform.position.z);
            this.transform.position = newPos;
        }
    }
}
