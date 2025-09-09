using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
  [Header("AI Navigation")]
  [SerializeField] private NavMeshAgent agent;
  public Transform player;
  
  [Header("Attack variables")]
  [SerializeField] private float maxDistance;
  private bool _isAttacking;
  [SerializeField] private GameObject bullet;
  [Tooltip("Amount of ticks until shot")]
  [SerializeField] private int shootSpeed;

  private int _ticks = 0;
  private void Start() {
    Ticker.Instance.OnTickEvent += UpdateGoal;
  }
  
  public void UpdateGoal() {
    float distance = Vector3.Distance(player.position, transform.position);
    if (distance <= maxDistance) {
      agent.isStopped = true;
      _isAttacking = true;
    }

    if (_isAttacking) {
      transform.LookAt(player.position);
      _ticks++;
      if (_ticks == shootSpeed) {
        Shoot();
        _ticks = 0;
      }
    }
    else {
      agent.destination = player.position;
    }
  }

  private void Shoot() {
    Vector3 pos = transform.position + transform.forward;
    Instantiate(bullet, pos, transform.rotation);
    bullet.GetComponent<Bullet>().direction = Vector3.RotateTowards(transform.forward, player.position, 0.0f, 0.0f);
  }
}
