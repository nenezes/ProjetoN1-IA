using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Entity
{
    public EventHandler OnHunterPlayed, OnPreyKilled;

    public static Hunter Instance;
    private float detectionRadius = 5.55f;
    private float attackrange = 1.55f;
    [SerializeField] LayerMask preyLayerMask;
    private Prey currentTarget = null;
    private State state;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GameManager.Instance.OnHunterTurn += GameManager_OnHunterTurn;
        state = State.Patrolling;
    }

    private void GameManager_OnHunterTurn(object sender, EventArgs e) {
        switch (state) {
            case State.Patrolling:
                CheckAvailableMoves();
                CheckForPrey();
                if (currentTarget != null) state = State.Chasing;
                else MoveRandom();

                break;

            case State.Chasing:
                CheckAvailableMoves();
                if (IsInAttackRange()) {
                    state = State.Attacking;
                }
                else {
                    MoveInTargetDirection(currentTarget);
                }

                break;

            case State.Attacking:
                currentTarget.Die();
                OnPreyKilled?.Invoke(this, EventArgs.Empty);
                currentTarget = null;
                state = State.Patrolling;
                break;
        }

        OnHunterPlayed?.Invoke(this, EventArgs.Empty);
    }

    override public void Update() {

    }

    private bool IsInAttackRange() {
        if (currentTarget == null) return false;

        if (Vector3.Distance(this.transform.position, currentTarget.transform.position) < attackrange) return true;
        
        return false;
    }

    private void MoveInTargetDirection(Prey target) {
        Debug.Log("Moving in target direction!");
        GridObject closestPosition = GetClosestMoveToTarget(currentTarget);
        Vector3 closestPos = new Vector3(closestPosition.transform.position.x, 0, closestPosition.transform.position.z);
        this.transform.position = closestPos;
    }

    private GridObject GetClosestMoveToTarget(Prey target) {
        GridObject closestMove = null;
        foreach (GridObject availableMove in availableMoves) {
            if (closestMove == null) closestMove = availableMove;
            else {
                if (Vector3.Distance(availableMove.transform.position, target.transform.position) < Vector3.Distance(closestMove.transform.position, target.transform.position)) {
                    closestMove = availableMove;
                }
            }
        }

        return closestMove;
    }

    private void CheckForPrey() {
        Collider[] cols = Physics.OverlapSphere(transform.position, detectionRadius, preyLayerMask);
        foreach (Collider collider in cols) {
            if (collider.gameObject.TryGetComponent<Prey>(out Prey prey)) 

            if (currentTarget == null) {
                currentTarget = prey;
            }
            else {
                if (Vector3.Distance(transform.position, currentTarget.transform.position) > Vector3.Distance(transform.position, prey.transform.position)) {
                    currentTarget = prey;
                }
            }
        }
    }

    public Prey GetCurrentTarget() => currentTarget;

    private void OnDestroy() {
        GameManager.Instance.OnHunterTurn += GameManager_OnHunterTurn;
    }
}

public enum State {
    Patrolling,
    Chasing,
    Attacking
}
