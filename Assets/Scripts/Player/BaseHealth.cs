using UnityEngine;

public class BaseHealth : MonoBehaviour {
  public int health;
  
  protected int MaxHealth;
  public virtual void TakeDamage(int amount) {
    health -= amount;
  }

  public virtual void Heal(int amount) {
    health += amount;
    health = Mathf.Clamp(health, 0, MaxHealth);
  }
}
