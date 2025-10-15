using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WaveEnemy : Enemy {
  private Room currentRoom;
  public int spins = 3;
  public float spinSpeed = 360f;
  private Animator _animator;
  protected override void UpdateGoal() {
    currentRoom = GameManager.Instance.currentRoom;
  }
  
  protected override void Update() {
    //nothing this is just here to override the original update (look at player)
  }
  protected new void Start() {
    base.Start();
    _animator = GetComponent<Animator>();
    currentRoom = GameManager.Instance.currentRoom;
    StartCoroutine(AttackPattern());
  }

  public IEnumerator AttackPattern() {
    yield return new WaitUntil(() => agent);
    float rotated = 0f;
    while (!agent.isStopped) {
      
      if (NavMesh.SamplePosition(currentRoom.enemySpawnLocations.GetRandomPosition(currentRoom.transform.position),
            out var hit, 5f, NavMesh.AllAreas)) {
        agent.destination = hit.position;
      }

      yield return new WaitUntil(() => agent.hasPath);
      yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);
      _animator.SetTrigger("Prepare");
      _animator.SetBool("Shooting", true);
      yield return new WaitForSeconds(0.2f);
      while (rotated < 360f * spins) {
        float step = spinSpeed * Time.deltaTime;
        transform.Rotate(0f, step, 0f);
        Shoot();
        rotated += step;
        yield return new WaitForSeconds(shootSpeed);
      }
      _animator.SetBool("Shooting", false);
      rotated = 0f;
    }
    yield return new WaitUntil(() => !agent.isStopped);
    StartCoroutine(AttackPattern());
  }
  
  protected override void Shoot() {
    var pos = shootPos.position;

    GameObject obj;
    if (PoolManager.TryDequeue(BulletType.WAVE, out obj)) {
      obj.SetActive(true);
      obj.transform.position = pos;
    }
    else {
      obj = Instantiate(bullet, pos, transform.rotation);
    }
    var b = obj.GetComponent<Bullet>();
    b.direction = Vector3.RotateTowards(transform.forward, player.position, 0.0f, 0.0f);
    b.gameObject.layer = gameObject.layer;
    b.type = BulletType.WAVE;
  }
}
