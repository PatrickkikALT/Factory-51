using UnityEngine;

public class BasicEnemy : Enemy {
  
  protected override void UpdateGoal() {
    var position = player.position;
    var distance = Vector3.Distance(position, transform.position);
    agent.destination = position + player.forward * maxDistance;

    ticks++;
    if (ticks == shootSpeed) {
      Shoot();
      ticks = 0;
    }
  }
}