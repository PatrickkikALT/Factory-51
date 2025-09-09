using System;
using UnityEngine;

public class Bullet : MonoBehaviour {

  public Rigidbody rb;
  [SerializeField] private float speed;
  public Vector3 direction;
  private void Start() {
    rb = GetComponent<Rigidbody>();
    Destroy(gameObject, 10f);
  }

  public void Update() {
    speed *= 1.001f;
    rb.AddForce(direction * speed);
  }
  private void OnTriggerEnter(Collider other) {
    Destroy(gameObject);
  } 
}