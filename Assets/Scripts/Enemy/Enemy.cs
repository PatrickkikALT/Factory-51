using UnityEngine;
using UnityEngine.AI;
using static Extensions;
public abstract class Enemy : MonoBehaviour {
  [Header("AI Navigation")] [SerializeField]
  protected NavMeshAgent agent;

  public Transform player;
  public Transform shootPos;
  [Header("Attack variables")] [SerializeField]
  protected float maxDistance;

  [SerializeField] protected GameObject bullet;

  [Tooltip("Amount of ticks until shot")] [SerializeField]
  protected int shootSpeed;

  [SerializeField] protected float walkSpeed;

  protected int ticks;

  protected void Start() {
    agent.speed = walkSpeed;
    player = GameManager.Instance.player;
    Ticker.Instance.OnTickEvent += UpdateGoal;
  }

  protected void Update() {
    Quaternion directionToPlayer = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), 2 * Time.deltaTime);
    transform.rotation = directionToPlayer;
  }
  public void OnDestroy() {
    Ticker.Instance.OnTickEvent -= UpdateGoal;
  }

  protected virtual void UpdateGoal() {
    var position = player.position;
    var distance = Vector3.Distance(position, transform.position);
    agent.destination = position + player.forward * maxDistance;

    ticks++;
    if (ticks == shootSpeed) {
      Shoot();
      ticks = 0;
    }
    // if (distance <= maxDistance) {
    //   agent.destination = player.position;
    //   Shoot();
    // }
  }

  

  protected void Shoot() {
    var pos = shootPos.position + transform.forward;
    var b = Instantiate(bullet, pos, transform.rotation).GetComponent<Bullet>();
    b.direction = Vector3.RotateTowards(transform.forward, player.position, 0.0f, 0.0f);
    b.gameObject.layer = gameObject.layer;
    b.type = BulletType.ENEMY;
  }
}