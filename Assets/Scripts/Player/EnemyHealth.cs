using UnityEngine;

public class EnemyHealth : BaseHealth {
  [Tooltip("Amount player heals on enemy kill, default is 2")]
  public int healAmountOnDeath = 2;
  public override void TakeDamage(int amount) {
    base.TakeDamage(amount);
    if (health <= 0) {
      GameManager.Instance.playerHealth.Heal(healAmountOnDeath);
      Destroy(gameObject);
    }
  }
}