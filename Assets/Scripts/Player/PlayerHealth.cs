using UnityEngine;

public class PlayerHealth : BaseHealth {
#if UNITY_EDITOR
  [Header("Dev")] public bool immortal;
#endif

  public override void TakeDamage(int amount) {
    #if UNITY_EDITOR
    if (immortal) return;
    #endif
    health -= amount;
    if (health <= 0) {
      GameManager.Instance.player.GetComponent<Animator>().SetTrigger("Die");
      GameManager.Instance.player.GetComponent<Movement>().dead = true;
      
    }
  }

  public override void Heal(int amount) {
    base.Heal(amount);
    if (health >= MaxHealth) health = MaxHealth;
  }
}