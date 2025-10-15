using System;
using UnityEngine;
using UnityEngine.AI;

public class BossHealth : BaseHealth {
    public Material eyeMaterial;
    private Animator _animator;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);
        if (health <= 0) {
            _animator.SetTrigger("Death");
            GetComponent<NavMeshAgent>().isStopped = true;
            GameManager.Instance.EndGame();
        }
        var color = Color.Lerp(eyeMaterial.GetColor("_EmissionColor"), Color.black, 10 / health);
        eyeMaterial.SetColor("_EmissionColor", color);
    }

    #if UNITY_EDITOR
    [ContextMenu("Take Damage (Test)")]
    public void Test() {
        TakeDamage(10);
    }
    #endif
}
