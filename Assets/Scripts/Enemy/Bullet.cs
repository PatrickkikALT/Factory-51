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
    speed *= 1.001f;
    rb.AddForce(direction * speed);
  }

  private void OnTriggerEnter(Collider other) {
    if (other.TryGetComponent(out BaseHealth health)) {                     
      health.TakeDamage(Mathf.RoundToInt(damage * GameManager.Instance.playerWeapon.currentDamageMultiplier));                                            
    }                                                                       
    Destroy(gameObject);                                                    
  }
}