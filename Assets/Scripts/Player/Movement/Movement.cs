using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {
  public float speed;
  private Vector2 _input;
  private Rigidbody _rb;
  [HideInInspector] public bool canMove = true;
  private Transform _camera;

  [Header("Walking Sound")] [SerializeField]
  private AudioClip[] clips;

  [SerializeField] private AudioSource source;

  public void OnMove(InputAction.CallbackContext ctx) {
    _input = ctx.ReadValue<Vector2>();
  }

  public void OnJump(InputAction.CallbackContext ctx) {
    if (!ctx.performed || !canMove) return;
    if (onGround)
    {
      jumping = true;
      _rb.linearVelocity = new Vector3(0, jumpStrength, 0);
    }
  }

  private void Start() {
    _rb = GetComponent<Rigidbody>();
    _camera = Camera.main.transform;
    // StartCoroutine(WalkSoundLoop());
  }

  private void Update() {
    if (!canMove) return;
    Vector3 dir = transform.TransformDirection(new Vector3(_input.x, 0, _input.y));
    _rb.linearVelocity = new Vector3(dir.x * speed, _rb.linearVelocity.y, dir.z * speed);
    if (jumping)
    {
      UpdateJumpMomentum();
    }
  }

  [Header("Jumping")] [SerializeField] private bool jumping;
  [SerializeField] private float fallVel;
  [SerializeField] private float jumpStrength;
  [SerializeField] private Transform groundCheckTransform;
  [SerializeField] private LayerMask groundLayers;
  [SerializeField] private float overlapSphereSize;

  public bool onGround =>
    Physics.OverlapSphereNonAlloc(groundCheckTransform.position, overlapSphereSize, new Collider[10], groundLayers) > 0;


  private void UpdateJumpMomentum() {
    if (_rb.linearVelocity.y < 0.5)
    {
      _rb.linearVelocity -= new Vector3(0, fallVel * Time.deltaTime, 0);

      if (onGround)
      {
        jumping = false;
      }
    }
  }

  // private IEnumerator WalkSoundLoop() {
  //     while (true) {
  //         if (_input.magnitude <= 0) {
  //             yield return new WaitUntil(() => _input.magnitude > 0);
  //         }
  //         yield return null;
  //         var clip = clips[Random.Range(0, clips.Length - 1)];
  //         source.clip = clip;
  //         source.Play();
  //         yield return new WaitForSeconds(clip.length * 3.5f);
  //     }
  // }
}