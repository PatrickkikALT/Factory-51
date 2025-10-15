using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth {
#if UNITY_EDITOR
  [Header("Dev")] public bool immortal;
#endif

  public Slider healthSlider;
  public override void TakeDamage(int amount) {
    #if UNITY_EDITOR
    if (immortal) return;
    #endif
    health -= amount;
    healthSlider.value = health;
    if (health <= 0) {
      GameManager.Instance.player.GetComponent<Animator>().SetTrigger("Die");
      GameManager.Instance.player.GetComponent<Movement>().dead = true;
      GameManager.Instance.EndGame(false);
    }
  }

  public override void Heal(int amount) {
    base.Heal(amount);
    healthSlider.value = health;
    if (health >= MaxHealth) health = MaxHealth;
  }
}