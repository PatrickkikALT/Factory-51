using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemy : Enemy {

  protected new void Start() {
    base.Start();
  }
  
  protected override void UpdateGoal() {
    var position = player.position;
    var distance = Vector3.Distance(position, transform.position);
    agent.destination = position + player.forward * maxDistance;

    ticks++;
    if (ticks == shootSpeed) {
      if (!firstShootTick) {
        firstShootTick = true;
      }
      else {
        Shoot();
        ticks = 0;
      }
    }
  }
}