using UnityEngine;

public class PlayerHealth : BaseHealth {

  #if UNITY_EDITOR
  [Header("Dev")] public bool immortal = false;
  #endif
  
  public override void TakeDamage(int amount) {
    if (immortal) return;
    base.TakeDamage(amount);
    if (health <= 0) {
      //dead
      //todo: functionality for death player
    }
  }

  public override void Heal(int amount) {
    base.Heal(amount);
    if (health >= MaxHealth) {
      health = MaxHealth;
    }
  }
}
