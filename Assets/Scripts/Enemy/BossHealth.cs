using System;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Animator))]
public class BossHealth : BaseHealth {
    public Material eyeMaterial;
    private Animator _animator;
    public bool isBlocking;
    private HealthSliderHelper _healthSlider;

    private void Start() {
        _animator = GetComponent<Animator>();
        _healthSlider = GameManager.Instance.bossHealthSlider;
        _healthSlider.SetValue(health);
    }

    //ugly but I want the takedamage to have a bool if the damage is done indirectly by a summon dying, but if I dont override it
    //fires the base health's takedamage too still causing damage being done to the boss while blocking
    public override void TakeDamage(int damage) {
        return;
    }
    public void TakeDamage(int damage, bool bossSummon = false) {
        if (!bossSummon) {
            if (isBlocking) {
                return;
            }
        }
        print("not blocking");
        health -= damage;
        if (health <= 0) {
            GetComponent<BossEnemy>().state = BossState.DEAD;
            GetComponent<BossEnemy>().dead = true;
            _animator.SetTrigger("Death");
            GetComponent<NavMeshAgent>().isStopped = true;
            GameManager.Instance.EndGame(true);
            return;
        }
        _healthSlider.SetValue(health);
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
