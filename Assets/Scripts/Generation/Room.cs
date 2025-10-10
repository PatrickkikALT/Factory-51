using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Tooltip("1: Right, 2: Left, 3: Up, 4: Down")]
    public GameObject[] doors;

    public Transform[] chestLocations;
    public SpawnBox enemySpawnLocations;

    public bool disabled;

    private void OnTriggerEnter(Collider other) {
      if (disabled) return;
      if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.TryGetComponent(out PlayerReference _)) {
        GameManager.Instance.SetRoom(this);
      }
    }

    private void OnDrawGizmos() {
      Gizmos.color = enemySpawnLocations.GizmoColor;
      Gizmos.DrawWireCube(enemySpawnLocations.BoxPos + transform.position, enemySpawnLocations.BoxSize);
    }
}
