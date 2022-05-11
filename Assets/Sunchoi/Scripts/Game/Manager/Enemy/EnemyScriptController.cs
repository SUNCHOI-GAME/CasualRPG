using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptController : MonoBehaviour
{
    /// <summary>
    /// EnemyMovementController
    /// </summary>
    [SerializeField]
    private EnemyMovementController enemyMovementController;
    public EnemyMovementController EnemyMovementController { get => this.enemyMovementController; }
}
