using System.Collections;
using UnityEngine;

public class EnemyHealth : BaseHealth {

  public override void TakeDamage(int amount) {
    base.TakeDamage(amount);
    if (health <= 0) {
      Destroy(gameObject);
    }
  }
}
