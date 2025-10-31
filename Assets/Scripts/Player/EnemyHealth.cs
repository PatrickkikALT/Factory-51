using UnityEngine;

public class EnemyHealth : BaseHealth {
  [Tooltip("Amount player heals on enemy kill, default is 2")]
  public int healAmountOnDeath = 2;

  public int summonDamage;

  public bool bossSummon;
  public override void TakeDamage(int amount) {
    base.TakeDamage(amount);
    if (health <= 0) {
      GameManager.Instance.playerHealth.Heal(healAmountOnDeath);
      if (bossSummon) {
        HandleBossSummonDeath();
      }
      GameManager.Instance.enemies.Remove(GetComponent<Enemy>());
      Destroy(gameObject);
    }
  }

  private void HandleBossSummonDeath() {
    GameManager.Instance.bossHealth.TakeDamage(summonDamage, true);
  }
}