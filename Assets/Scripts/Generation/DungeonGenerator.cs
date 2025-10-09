using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {
  public GameObject roomPrefab;
  public GameObject hallwayPrefab;
  [Tooltip("This includes both the room and the hallway!")]
  public int dungeonSize;
  public int roomSize;

  private List<Vector3> _positions = new();
  private List<GameObject> _roomsInstance = new();
  private List<Vector3> _worms = new();

  [Header("Chests")] 
  public GameObject chestPrefab;
  public int roomsUntilChest = 5;

  private void Start() {
    GenerateDungeon();
  }

  public void GenerateDungeon() {
    ClearPositions();
    GeneratePositions();
    GenerateRooms();
  }

  private void GeneratePositions() {
    for (int i = 0; i < dungeonSize; i++) {
      if (i < 1) {
        _positions.Add(Vector3.zero);
      }
      else {
        Vector3 lastPosition = _positions[i - 1];
        List<Vector3> newPosition = new List<Vector3> {
          new(lastPosition.x + roomSize, 0, lastPosition.z),
          new(lastPosition.x - roomSize, 0, lastPosition.z),
          new(lastPosition.x, 0, lastPosition.z + roomSize),
          new(lastPosition.x, 0, lastPosition.z - roomSize)
        };

        List<Vector3> repeatPosition = newPosition.Where(t => _positions.Contains(t)).ToList();

        foreach (var t in repeatPosition) {
          newPosition.Remove(t);
        }

        if (newPosition.Count < 1) {
          newPosition.Clear();
          repeatPosition.Clear();

          for (int y = 2; y < dungeonSize; y++) {
            lastPosition = _positions[i - y];

            newPosition.Add(new Vector3(lastPosition.x + roomSize, 0, lastPosition.z));
            newPosition.Add(new Vector3(lastPosition.x - roomSize, 0, lastPosition.z));
            newPosition.Add(new Vector3(lastPosition.x, 0, lastPosition.z + roomSize));
            newPosition.Add(new Vector3(lastPosition.x, 0, lastPosition.z - roomSize));

            repeatPosition.AddRange(newPosition.Where(t => _positions.Contains(t)));

            foreach (var t in repeatPosition) {
              newPosition.Remove(t);
            }

            if (newPosition.Count > 0) {
              Vector3 finalPosition = newPosition.Random();
              _positions.Add(finalPosition);
              _worms.Add(finalPosition);
              break;
            }

            newPosition.Clear();
          }
        }
        else {
          Vector3 finalPosition = newPosition.Random();
          _positions.Add(finalPosition);
        }
      }
    }
  }

  private void GenerateRooms() {
    for (int i = 0; i < _positions.Count; i++) {
      GameObject newRoom = Instantiate(roomPrefab, _positions[i], Quaternion.identity);
      _roomsInstance.Add(newRoom);
      if (i < 1) {
        var room = newRoom.GetComponent<Room>();
        room.disabled = true;
      }
      if (i > 0) {
        Room newRoomScript, lastRoomScript;
        if (_worms.Contains(_positions[i])) {
          Vector3[] directions = {
            Vector3.right,
            Vector3.left,
            Vector3.forward,
            Vector3.back
          };

          //ewwww
          List<int> aroundPositionsIndex = directions.Select(dir => _positions[i] + dir * roomSize)
            .Select(target => _positions.IndexOf(target)).Where(index => index != -1).ToList();

          int lastPositionIndex = aroundPositionsIndex.Max();
          while (lastPositionIndex > i) {
            aroundPositionsIndex.Remove(lastPositionIndex);
            lastPositionIndex = aroundPositionsIndex.Max();
          }

          newRoomScript = _roomsInstance[i].GetComponent<Room>();
          lastRoomScript = _roomsInstance[lastPositionIndex].GetComponent<Room>();
          Vector3 direction = _positions[i] - _positions[lastPositionIndex];

          Vector3 midpoint = (_positions[i] + _positions[lastPositionIndex]) / 2f;
          Quaternion rotation = Quaternion.identity;
          if (Mathf.Abs(direction.x) > 0) rotation = Quaternion.Euler(0, 90, 0);
          InstantiateHallwayBetween(_positions[i], _positions[lastPositionIndex]);

          OpenDoors(newRoomScript, lastRoomScript, direction);
          
        }
        else {
          newRoomScript = _roomsInstance[i].GetComponent<Room>();
          lastRoomScript = _roomsInstance[i - 1].GetComponent<Room>();

          Vector3 direction = _positions[i] - _positions[i - 1];

          Vector3 midpoint = (_positions[i] + _positions[i - 1]) / 2f;
          Quaternion rotation = Quaternion.identity;
          if (Mathf.Abs(direction.x) > 0) rotation = Quaternion.Euler(0, 90, 0);
          InstantiateHallwayBetween(_positions[i], _positions[i - 1]);

          OpenDoors(newRoomScript, lastRoomScript, direction);
        }
      }

      if (i % roomsUntilChest == 0 && i != 0) {
        Transform pos = _roomsInstance[i].GetComponent<Room>().chestLocations.Random();
        Instantiate(chestPrefab, pos.position, pos.rotation);
      }
    }
  }

  private void OpenDoors(Room newRoom, Room lastRoom, Vector3 dir) {
    var doorMap = new Dictionary<Vector3, (int newDoor, int lastDoor)> {
      { Vector3.right * roomSize, (2, 1) },
      { Vector3.left * roomSize, (1, 2) },
      { Vector3.forward * roomSize, (4, 3) },
      { Vector3.back * roomSize, (3, 4) }
    };

    if (doorMap.TryGetValue(dir, out var doors)) {
      newRoom.doors[doors.newDoor].SetActive(false);
      lastRoom.doors[doors.lastDoor].SetActive(false);
    }
  }

  private void ClearPositions() {
    _positions.Clear();
    _worms.Clear();

    foreach (var t in _roomsInstance) {
      Destroy(t);
    }

    _roomsInstance.Clear();
  }
  
  private void InstantiateHallwayBetween(Vector3 a, Vector3 b) {
    Vector3 midpoint = (a + b) / 2f;
    Vector3 dir = b - a;
    Quaternion rotation = Quaternion.identity;
    
    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z)) rotation = Quaternion.Euler(0, 90, 0);
    GameObject hallway = Instantiate(hallwayPrefab, midpoint, rotation);
    
    Renderer[] rends = hallway.GetComponentsInChildren<Renderer>();
    if (rends.Length > 0) {
      Bounds combined = rends[0].bounds;
      for (int i = 1; i < rends.Length; i++) combined.Encapsulate(rends[i].bounds);

      Vector3 correction = midpoint - combined.center;
      hallway.transform.position += correction;
    }

    //yucky solution but we dont need multiple floors rn so y can always be set to 0
    Vector3 pos = hallway.transform.position;
    pos.y = 0;
    hallway.transform.position = pos;
  }

  
}