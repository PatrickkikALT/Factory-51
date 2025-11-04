using System;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Animator))]
public class BossHealth : BaseHealth {
    public Material eyeMaterial;
    private Animator _animator;
    public bool isBlocking;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    public new void TakeDamage(int damage, bool bossSummon = false) {
        if (isBlocking) return;
        print("not blocking");
        base.TakeDamage(damage);
        if (health <= 0) {
            GetComponent<BossEnemy>().state = BossState.DEAD;
            _animator.SetTrigger("Death");
            GetComponent<NavMeshAgent>().isStopped = true;
            GameManager.Instance.EndGame(true);
            return;
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
