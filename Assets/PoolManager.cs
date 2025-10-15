using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


public static class PoolManager {
  private static Queue<GameObject> playerBulletPool = new();
  private static Queue<GameObject> basicBulletPool = new();
  private static Queue<GameObject> circleBulletPool = new();
  private static Queue<GameObject> waveBulletPool = new();
  private static Queue<GameObject> bossBulletPool = new();

  private const int PoolSize = 100;

  public static void Enqueue(GameObject bullet, BulletType type) {
    Queue<GameObject> bulletPool = type switch {
      BulletType.PLAYER => playerBulletPool,
      BulletType.BASIC => basicBulletPool,
      BulletType.CIRCLE => circleBulletPool,
      BulletType.WAVE => waveBulletPool,
      BulletType.BOSS => bossBulletPool,
      _ => null
    };

    if (bulletPool == null) {
      Debug.LogWarning($"Unknown bullet type: {type}");
      Object.Destroy(bullet);
      return;
    }

    if (bulletPool.Count < PoolSize) {
      bullet.SetActive(false);
      bulletPool.Enqueue(bullet);
    }
    else {
      Object.Destroy(bullet);
    }
  }

  public static bool TryDequeue(BulletType type, out GameObject bullet) {
    Queue<GameObject> bulletPool = type switch {
      BulletType.PLAYER => playerBulletPool,
      BulletType.BASIC => basicBulletPool,
      BulletType.CIRCLE => circleBulletPool,
      BulletType.WAVE => waveBulletPool,
      BulletType.BOSS => bossBulletPool,
      _ => null
    };

    if (bulletPool == null || bulletPool.Count == 0) {
      bullet = null;
      return false;
    }

    var obj = bulletPool.Dequeue();

    if (!obj) {
      bullet = null;
      return false;
    }

    obj.SetActive(true);
    bullet = obj;
    return true;
  }
}