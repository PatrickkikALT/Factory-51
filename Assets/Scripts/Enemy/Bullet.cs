using System;
using System.Collections;
using UnityEngine;

public enum BulletType {
  BOSS,
  CIRCLE,
  WAVE,
  PLAYER,
  BASIC,
  CIRCLEEM
}

public class Bullet : MonoBehaviour {
  public Rigidbody rb;
  [SerializeField] private float speed;
  public Vector3 direction;
  public int damage;
  public BulletType type;

  private void Awake() {
    rb = GetComponent<Rigidbody>();
  }

  private void OnTriggerEnter(Collider other) {
    //return early if hitting room collider
    if (other.TryGetComponent(out Room _)) return;
    //return early if hit collider is a bullet so they dont collide with each other
    // if (other.TryGetComponent(out Bullet _)) return;
    if (other.TryGetComponent(out PlayerReference player)) {
      player.transform.parent.TryGetComponent(out BaseHealth health);
      health.TakeDamage(Mathf.RoundToInt(damage));
      Enqueue();
      return;
    }

    if (other.TryGetComponent(out BaseHealth bHealth)) {
      bHealth.TakeDamage(Mathf.RoundToInt(damage));
    }

    //ugly lol but deadline is today so dont care just want it to work
    if (other.TryGetComponent(out BossHealth bossHealth)) {
      bossHealth.TakeDamage(Mathf.RoundToInt(damage), false);
    }

    if (TryGetComponent(out BulletEmitter em)) em.EmitBullets(0);
    Enqueue();
    
  }

  private void OnEnable() {
    StartCoroutine(ApplyForceNextFrame());
    Invoke(nameof(Enqueue), 10f);
  }
  //adding force at same frame of activation never adds force for some reason
  private IEnumerator ApplyForceNextFrame() {
    yield return null;
    rb.AddForce(direction * speed, ForceMode.Impulse);
  }


  private void OnDisable() {
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
    rb.Sleep();
    transform.position = Vector3.zero;
    CancelInvoke(nameof(Enqueue));
  }

  void Enqueue() {
    PoolManager.Enqueue(gameObject, type);
  }
}