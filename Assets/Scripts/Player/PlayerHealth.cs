using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth {
  [Header("Dev")] 
  public bool immortal;
  
  [Header("UI")]
  public HealthSliderHelper healthSlider;

  public TMP_Text fpsText;

  private void Start() {
    MaxHealth = health;
    healthSlider.SetValue(health);
  }

  public override void TakeDamage(int amount) {
    if (immortal) return;
    health -= amount;
    healthSlider.SetValue(health);
    if (health <= 0) {
      GameManager.Instance.player.GetComponent<Animator>().SetTrigger("Die");
      GameManager.Instance.player.GetComponent<Movement>().dead = true;
      GameManager.Instance.EndGame(false);
    }
  }
  
  public override void Heal(int amount) {
    base.Heal(amount);
    healthSlider.SetValue(health);
    health = Mathf.Clamp(health, 0, MaxHealth);
  }

  public void OnDevTool() {
    immortal = !immortal;
    fpsText.enabled = immortal;
  }
}