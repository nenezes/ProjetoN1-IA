using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : Entity
{
    private PreyState state;
    private float awarenessRange = 7f;

    private void Start() {
        GameManager.Instance.OnPreyTurn += GameManager_OnPreyTurn;
    }

    private void GameManager_OnPreyTurn(object sender, EventArgs e) {
        switch (state) {
            case PreyState.Walking:
                CheckAvailableMoves();
                MoveRandom();
                if (IsHunterCloseEnough()) state = PreyState.Running;
                break;

            case PreyState.Running:
                if (!IsBeingChased()) state = PreyState.Walking;
                CheckAvailableMoves();
                MoveToFurthestFromHunter();
                break;
        }
    }

    public override void Update() {

    }

    public void Die() {
        Destroy(gameObject);
    }

    private bool IsHunterCloseEnough() {
        if (Vector3.Distance(transform.position, Hunter.Instance.transform.position) < awarenessRange) {
            return true;
        }
        
        return false;
    }

    private bool IsBeingChased() {
        if (Hunter.Instance.GetCurrentTarget() == this.gameObject) return true;

        return false;
    }

    private void MoveToFurthestFromHunter() {
        CheckAvailableMoves();
        GridObject furthestMove = null;
        foreach (GridObject availableMove in availableMoves) {
            if (furthestMove == null) furthestMove = availableMove;
            else {
                if (Vector3.Distance(availableMove.transform.position, Hunter.Instance.transform.position) > Vector3.Distance(furthestMove.transform.position, Hunter.Instance.transform.position)) {
                    furthestMove = availableMove;
                }
            }
        }

        Vector3 furthestPos = new Vector3(furthestMove.transform.position.x, 0, furthestMove.transform.position.z);
        this.transform.position = furthestPos;
    }

    private void OnDestroy() {
        GameManager.Instance.OnPreyTurn -= GameManager_OnPreyTurn;
    }
}

public enum PreyState {
    Walking,
    Running
}