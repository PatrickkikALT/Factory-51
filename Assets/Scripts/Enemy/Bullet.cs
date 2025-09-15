using System;
using UnityEngine;

public enum BulletType {
  PLAYER,
  ENEMY
}

public class Bullet : MonoBehaviour {
  public Rigidbody rb;
  [SerializeField] private float speed;
  public Vector3 direction;
  public int damage;
  public BulletType type;

  private void Start() {
    rb = GetComponent<Rigidbody>();
    Destroy(gameObject, 10f);
  }

  public void Update() {
    rb.AddForce(direction * speed);
  }

  private void OnTriggerEnter(Collider other) {
    //return early if hit collider is a bullet so they dont collide with each other
    if (other.TryGetComponent(out Bullet b)) {
      return;
    }
    if (other.TryGetComponent(out BaseHealth health)) {                     
      health.TakeDamage(Mathf.RoundToInt(damage * GameManager.Instance.playerWeapon.currentDamageMultiplier));
      Destroy(gameObject);
      return;
    }
    if (TryGetComponent(out BulletEmitter em)) {
      em.EmitBullets(0);
    }
    Destroy(gameObject);                                                    
  }
}