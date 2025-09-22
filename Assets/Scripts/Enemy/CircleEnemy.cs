using UnityEngine;

public class CircleEnemy : Enemy {
  [SerializeField] private int preferredDistance;
  [SerializeField] private int panicDistance;

  [SerializeField] private int panicWalkSpeed;
  private Vector3 _currentDestination;

  private new void Start() {
    base.Start();
    _currentDestination = CalculateDestination();
  }

  protected override void UpdateGoal() {
    if (Vector3.Distance(transform.position, player.position) <= panicDistance) {
      _currentDestination = PanicDestination();
      agent.destination = _currentDestination;
      agent.speed = panicWalkSpeed;
      return;
    }

    agent.speed = walkSpeed;
    if (Vector3.Distance(transform.position, _currentDestination) <= preferredDistance) {
      ticks++;
      if (ticks >= shootSpeed) {
        Shoot();
        ticks = 0;
      }

      _currentDestination = CalculateDestination();
    }

    agent.destination = _currentDestination;
  }

  private Vector3 CalculateDestination() {
    var destination = player.position + player.forward * preferredDistance;
    return destination;
  }

  private Vector3 PanicDestination() {
    var pos = player.position;
    pos += player.forward * preferredDistance;
    return pos;
  }
}