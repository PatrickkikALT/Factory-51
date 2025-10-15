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
  protected float shootSpeed;

  [SerializeField] protected int rotationSpeed;
  [SerializeField] protected float walkSpeed;

  protected int ticks;
  
  protected void Start() {
    agent.speed = walkSpeed;
    player = GameManager.Instance.player;
    Ticker.Instance.OnTickEvent += UpdateGoal;
  }

  protected virtual void Update() {
    Quaternion directionToPlayer = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotationSpeed * Time.deltaTime);
    transform.rotation = directionToPlayer;
  }
  public void OnDestroy() {
    Ticker.Instance.OnTickEvent -= UpdateGoal;
  }
  protected virtual void UpdateGoal() {
    // if (distance <= maxDistance) {
    //   agent.destination = player.position;
    //   Shoot();
    // }
  }

  

  protected void Shoot() {
    var pos = shootPos.position;

    GameObject obj;
    if (PoolManager.TryDequeue(out obj)) {
      obj.SetActive(true);
      obj.transform.position = pos;
    }
    else {
      obj = Instantiate(bullet, pos, transform.rotation);
    }
    var b = obj.GetComponent<Bullet>();
    b.direction = Vector3.RotateTowards(transform.forward, player.position, 0.0f, 0.0f);
    b.gameObject.layer = gameObject.layer;
    b.type = BulletType.ENEMY;
  }
}