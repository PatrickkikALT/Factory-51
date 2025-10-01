using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public enum Direction {
  UP,
  DOWN
}

public class MapGeneration : MonoBehaviour {
  public int mapSize;
  [SerializeField] private List<GameObject> tiles;
  private List<Vector3> tilePositions = new List<Vector3>();
  public int gridSize = 10;
  private GameObject _temp;
  public float xPos, zPos, newXPos, newYPos;
  private Vector3 _tempVector;
  public GameObject tile;
  private Direction tempDir = Direction.UP;

  private void Start() {
    GenerateMap();
  }

  private void GenerateMap() {
    for (int i = 0; i < mapSize; i++) {
      if (tiles.Count == 0) {
        _temp = Instantiate(tile, Vector3.zero, Quaternion.identity);
        tiles.Add(_temp);
        tilePositions.Add(_temp.transform.position);
      }
      else {
        ChooseDirection();
        _temp = Instantiate(tile, _tempVector, Quaternion.identity);
        tiles.Add(_temp);
        tilePositions.Add(_tempVector);
      }
    }
  }

  private void ChooseDirection() {
    bool valid = false;
    while (!valid) {
      Direction dir = tempDir.RandomEnum<Direction>();
      switch (dir) {
        case Direction.UP: {
          var position = _temp.transform.position;
          xPos = position.x + gridSize;
          zPos = position.z;
          break;
        }
        case Direction.DOWN: {
          var position = _temp.transform.position;
          xPos = position.x;
          zPos = position.z + -gridSize;
          break;
        }
        default:
          throw new ArgumentOutOfRangeException();
      }

      _tempVector = new Vector3(xPos, 0, zPos);
      valid = tilePositions.All(pos => _tempVector != pos);
    }
  }
}