using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnCount;
    [SerializeField] TextMeshProUGUI remainingEnemies;

    private void Update() {
        turnCount.text = $"Turnos jogados: {GameManager.Instance.turnCount}";
        remainingEnemies.text = $"Presas restantes: {GameManager.Instance.preyCount}";
    }
}
