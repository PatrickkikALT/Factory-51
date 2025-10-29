using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


public static class PoolManager {
  private static Queue<GameObject> _playerBulletPool = new();
  private static Queue<GameObject> _basicBulletPool = new();
  private static Queue<GameObject> _circleBulletPool = new();
  private static Queue<GameObject> _waveBulletPool = new();
  private static Queue<GameObject> _bossBulletPool = new();

  private const int PoolSize = 100;

  public static void Enqueue(GameObject bullet, BulletType type) {
    Queue<GameObject> bulletPool = type switch {
      BulletType.PLAYER => _playerBulletPool,
      BulletType.BASIC => _basicBulletPool,
      BulletType.CIRCLE => _circleBulletPool,
      BulletType.WAVE => _waveBulletPool,
      BulletType.BOSS => _bossBulletPool,
      _ => null
    };

    if (bulletPool == null) {
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
      BulletType.PLAYER => _playerBulletPool,
      BulletType.BASIC => _basicBulletPool,
      BulletType.CIRCLE => _circleBulletPool,
      BulletType.WAVE => _waveBulletPool,
      BulletType.BOSS => _bossBulletPool,
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