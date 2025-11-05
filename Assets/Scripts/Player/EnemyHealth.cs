using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : BaseHealth {
  [Tooltip("Amount player heals on enemy kill, default is 2")]
  public int healAmountOnDeath = 2;
  public int summonDamage;
  public bool bossSummon;
  private Animator _animator;
  [SerializeField] private AnimationClip clip;

  public void Start() {
    _animator = GetComponent<Animator>();
  }

  public override void TakeDamage(int amount) {
    if (health <= 0) return;
    base.TakeDamage(amount);
    if (health <= 0) {
      GameManager.Instance.playerHealth.Heal(healAmountOnDeath);
      _animator.SetTrigger("dead");
      if (bossSummon) {
        HandleBossSummonDeath();
      }
      GameManager.Instance.enemies.Remove(GetComponent<Enemy>());
      GetComponent<NavMeshAgent>().isStopped = true;
      Destroy(gameObject, clip.length);
    }
  }

  private void HandleBossSummonDeath() {
    GameManager.Instance.bossHealth.TakeDamage(summonDamage, true);
  }
}