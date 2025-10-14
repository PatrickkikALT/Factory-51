using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {
  [SerializeField] private Transform parent;
  [Header("Room Prefabs")] public GameObject[] baseRoomPrefabs;
  public GameObject[] windowRoomPrefabs;
  public GameObject[] xRoomPrefabs;
  public GameObject bossRoom;

  [Header("Hallway Prefabs")] public GameObject baseHallwayPrefab;
  public GameObject baseToWindowHallwayPrefab;
  public GameObject windowHallwayPrefab;
  public GameObject windowToXHallwayPrefab;
  public GameObject xHallwayPrefab;
  public GameObject bossHallwayPrefab;

  [Header("Dungeon Settings")] 
  public int dungeonSize;
  [Tooltip("This includes both the room and the hallway!")]
  public int roomSize;

  private List<Vector3> _positions = new();
  private List<GameObject> _roomsInstance = new();
  private List<Vector3> _worms = new();

  [Header("Chests")] public GameObject chestPrefab;
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
      if (i == 0) {
        _positions.Add(Vector3.zero);
        continue;
      }

      Vector3 lastPosition = _positions[i - 1];
      List<Vector3> newPosition = new List<Vector3> {
        new(lastPosition.x + roomSize, 0, lastPosition.z),
        new(lastPosition.x - roomSize, 0, lastPosition.z),
        new(lastPosition.x, 0, lastPosition.z + roomSize),
        new(lastPosition.x, 0, lastPosition.z - roomSize)
      };

      List<Vector3> repeatPosition = newPosition.Where(t => _positions.Contains(t)).ToList();
      foreach (var t in repeatPosition) newPosition.Remove(t);

      if (newPosition.Count < 1) {
        newPosition.Clear();
        repeatPosition.Clear();

        for (int y = 2; y < dungeonSize; y++) {
          lastPosition = _positions[i - y];
          newPosition.AddRange(new[] {
            new Vector3(lastPosition.x + roomSize, 0, lastPosition.z),
            new Vector3(lastPosition.x - roomSize, 0, lastPosition.z),
            new Vector3(lastPosition.x, 0, lastPosition.z + roomSize),
            new Vector3(lastPosition.x, 0, lastPosition.z - roomSize)
          });

          repeatPosition.AddRange(newPosition.Where(t => _positions.Contains(t)));
          foreach (var t in repeatPosition) newPosition.Remove(t);

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

    Vector3 lastRoomPos = _positions[_positions.Count - 1];
    Vector3 secondLastPos = _positions[_positions.Count - 2];
    Vector3 dir = (lastRoomPos - secondLastPos).normalized;
    
    Vector3 bossRoomPos = Vector3.zero;
    float distanceMultiplier = 2f;
    bool foundSpot = false;

    for (int attempt = 0; attempt < 15; attempt++) {
      bossRoomPos = lastRoomPos + dir * (roomSize * distanceMultiplier - 4);

      if (!_positions.Contains(bossRoomPos)) {
        foundSpot = true;
        break;
      }

      distanceMultiplier += 0.5f;
    }
    
    if (!foundSpot) {
      bossRoomPos = lastRoomPos + dir * (roomSize * 2);
    }

    _positions.Add(bossRoomPos);

  }

  private void GenerateRooms() {
    int baseEnd = dungeonSize / 3;
    int windowEnd = 2 * dungeonSize / 3;

    for (int i = 0; i < _positions.Count; i++) {
      bool isBossRoom = i == _positions.Count - 1;
      GameObject newRoom;
      if (isBossRoom) {
        newRoom = Instantiate(bossRoom, _positions[i], Quaternion.identity);
        newRoom.GetComponent<Room>().bossRoom = true;
      }
      else {
        GameObject[] currentRoomSet = i < baseEnd ? baseRoomPrefabs :
          i < windowEnd ? windowRoomPrefabs :
          xRoomPrefabs;
        newRoom = Instantiate(currentRoomSet.Random(), _positions[i], Quaternion.identity);
      }

      newRoom.GetComponent<Room>().id = i;
      newRoom.transform.parent = parent;

      _roomsInstance.Add(newRoom);

      if (i == 0) {
        var room = newRoom.GetComponent<Room>();
        room.disabled = true;
      }

      if (i > 0) {
        Room newRoomScript = _roomsInstance[i].GetComponent<Room>();
        Room lastRoomScript = _roomsInstance[i - 1].GetComponent<Room>();
        Vector3 prevPos = _positions[i - 1];
        Vector3 currPos = _positions[i];
        Vector3 direction = currPos - prevPos;

        GameObject hallwayPrefab = isBossRoom
          ? bossHallwayPrefab
          : GetHallwayForTransition(i - 1, i, baseEnd, windowEnd);

        InstantiateHallwayBetween(prevPos, currPos, hallwayPrefab, isBossRoom);
        try {
          OpenDoors(newRoomScript, lastRoomScript, direction);
        }
        catch {
          Debug.Log($"Failed at index {i}, tried new room {_roomsInstance[i]} and old room {_roomsInstance[i - 1]}");
        }
      }

      if (!isBossRoom && i % roomsUntilChest == 0 && i != 0) {
        Transform pos = _roomsInstance[i].GetComponent<Room>().chestLocations.Random();
        Instantiate(chestPrefab, pos.position, pos.rotation);
      }
    }
    parent.GetComponent<NavMeshSurface>().BuildNavMesh();
  }

  private GameObject GetHallwayForTransition(int lastIndex, int currentIndex, int baseEnd, int windowEnd) {
    bool lastWasBase = lastIndex < baseEnd;
    bool lastWasWindow = lastIndex >= baseEnd && lastIndex < windowEnd;
    bool lastWasX = lastIndex >= windowEnd;

    bool currIsBase = currentIndex < baseEnd;
    bool currIsWindow = currentIndex >= baseEnd && currentIndex < windowEnd;
    bool currIsX = currentIndex >= windowEnd;

    if (lastWasBase && currIsWindow || lastWasWindow && currIsBase) return baseToWindowHallwayPrefab;
    if (lastWasWindow && currIsX || lastWasX && currIsWindow) return windowToXHallwayPrefab;
    if (currIsBase) return baseHallwayPrefab;
    if (currIsWindow) return windowHallwayPrefab;
    if (currIsX) return xHallwayPrefab;

    return baseHallwayPrefab;
  }

  private void OpenDoors(Room newRoom, Room lastRoom, Vector3 dir) {
    Vector3 normalizedDir = Vector3.zero;

    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
      normalizedDir = dir.x > 0 ? Vector3.right : Vector3.left;
    else
      normalizedDir = dir.z > 0 ? Vector3.forward : Vector3.back;

    var doorMap = new Dictionary<Vector3, (int newDoor, int lastDoor)> {
      { Vector3.right, (2, 1) },
      { Vector3.left, (1, 2) },
      { Vector3.forward, (4, 3) },
      { Vector3.back, (3, 4) }
    };

    if (doorMap.TryGetValue(normalizedDir, out var doors)) {
      newRoom.doors[doors.newDoor].SetActive(false);
      lastRoom.doors[doors.lastDoor].SetActive(false);
    }
  }


  private void ClearPositions() {
    _positions.Clear();
    _worms.Clear();

    foreach (var t in _roomsInstance) Destroy(t);
    _roomsInstance.Clear();
  }

  private void InstantiateHallwayBetween(Vector3 a, Vector3 b, GameObject hallwayPrefab, bool bossConnection = false) {
    Vector3 dir = (b - a).normalized;
    Quaternion rotation = Mathf.Abs(dir.x) > Mathf.Abs(dir.z) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;

    Vector3 midpoint = (a + b) / 2f;

    if (bossConnection) {
      float offset = 5f; 
      midpoint -= dir * offset;
    }

    GameObject hallway = Instantiate(hallwayPrefab, midpoint, rotation);
    
    Renderer[] finalRends = hallway.GetComponentsInChildren<Renderer>();
    if (finalRends.Length > 0) {
      Bounds combined = finalRends[0].bounds;
      for (int i = 1; i < finalRends.Length; i++) combined.Encapsulate(finalRends[i].bounds);
      Vector3 correction = midpoint - combined.center;
      hallway.transform.position += correction;
    }
    hallway.transform.parent = parent;

    //yucky solution but we dont need multiple floors rn so y can always be set to 0
    Vector3 pos = hallway.transform.position;
    pos.y = 0;
    hallway.transform.position = pos;
  }

}