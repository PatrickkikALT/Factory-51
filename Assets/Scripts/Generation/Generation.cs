using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generation : MonoBehaviour {
  public static Generation Instance;
  public int maxXZ;
  public float offsetX;
  public float offsetZ;
  public Tile[] tileObjects;
  public List<Cell> gridList = new();
  public Cell cell;
  public Tile blankTile;
  public List<GameObject> dungeons = new();
  public List<Transform> dungeonPositions = new();
  private int iterations;

  private void Start() {
    Instance = this;
    InitializeGrid();
  }

  private void InitializeGrid() {
    for (var x = 0; x < maxXZ; x++)
    for (var z = 0; z < maxXZ; z++) {
      var newCell = Instantiate(cell, new Vector3(x * offsetX, 0, z * offsetZ), Quaternion.identity);
      newCell.CreateCell(false, tileObjects);
      gridList.Add(newCell);
    }

    StartCoroutine(CheckEntropy());
  }

  private IEnumerator CheckEntropy() {
    List<Cell> tempGrid = new(gridList);
    tempGrid.RemoveAll(c => c.collapsed);
    tempGrid.Sort((a, b) => a.tiles.Length - b.tiles.Length);
    var arrLength = tempGrid[0].tiles.Length;
    var stopIndex = 0;
    for (var i = 1; i < tempGrid.Count; i++) {
      stopIndex = i;
      break;
    }

    if (stopIndex > 0) tempGrid.RemoveRange(stopIndex, tempGrid.Count - stopIndex);

    yield return new WaitForSeconds(0.01f);
    CollapseCell(tempGrid);
  }

  private void CollapseCell(List<Cell> tempGrid) {
    var randIndex = Random.Range(0, tempGrid.Count);
    var colCell = tempGrid[randIndex];
    colCell.collapsed = true;
    var sTile = blankTile;
    try {
      sTile = colCell.tiles[Random.Range(0, colCell.tiles.Length - 1)];
    }
    catch {
      throw new ArgumentOutOfRangeException();
    }

    colCell.tiles = new[] { sTile };
    var fTile = colCell.tiles[0];
    var newTile = Instantiate(fTile, colCell.transform.position, Quaternion.Euler(Vector3.zero));
    dungeons.Add(newTile.gameObject);
    dungeonPositions.Add(newTile.transform);
    newTile.transform.parent = transform;
    newTile.dungeonId = fTile.dungeonId;
    UpdateGeneration();
  }

  private void UpdateGeneration() {
    List<Cell> newCells = new(gridList);
    for (var x = 0; x < maxXZ; x++)
    for (var y = 0; y < maxXZ; y++) {
      var index = x + y * maxXZ;
      if (gridList[index].collapsed) {
        newCells[index] = gridList[index];
      }
      else {
        List<Tile> options = new();
        foreach (var v in tileObjects) options.Add(v);

        if (y > 0) {
          var up = gridList[x + (y - 1) * maxXZ];
          List<Tile> validOptions = new();
          validOptions = up.tiles.Select(possible => Array.FindIndex(tileObjects, obj => obj == possible)).Select(valOp => tileObjects[valOp].up).Aggregate(validOptions, (current, valid) => current.Concat(valid).ToList());

          CheckValidity(options, validOptions);
        }

        if (x < maxXZ - 1) {
          var right = gridList[x + 1 + y * maxXZ];
          List<Tile> validOptions = new();
          validOptions = right.tiles.Select(possible => Array.FindIndex(tileObjects, obj => obj == possible)).Select(valOp => tileObjects[valOp].right).Aggregate(validOptions, (current, valid) => current.Concat(valid).ToList());

          CheckValidity(options, validOptions);
        }

        if (y < maxXZ - 1) {
          var down = gridList[x + (y + 1) * maxXZ];
          List<Tile> validOptions = new();
          validOptions = down.tiles.Select(possible => Array.FindIndex(tileObjects, obj => obj == possible)).Select(valOp => tileObjects[valOp].down).Aggregate(validOptions, (current, valid) => current.Concat(valid).ToList());

          CheckValidity(options, validOptions);
        }

        if (x > 0) {
          var left = gridList[x - 1 + y * maxXZ];
          List<Tile> validOptions = new();
          validOptions = left.tiles.Select(possible => Array.FindIndex(tileObjects, obj => obj == possible)).Select(valOp => tileObjects[valOp].left).Aggregate(validOptions, (current, valid) => current.Concat(valid).ToList());

          CheckValidity(options, validOptions);
        }

        var newTiles = new Tile[options.Count];
        for (var i = 0; i < options.Count; i++) newTiles[i] = options[i];

        newCells[index].RecreateCell(newTiles);
      }
    }

    gridList = newCells;
    iterations++;
    if (iterations < maxXZ * maxXZ) StartCoroutine(CheckEntropy());
  }

  private void CheckValidity(List<Tile> optionList, List<Tile> validOption) {
    for (var x = optionList.Count - 1; x >= 0; x--) {
      var element = optionList[x];
      if (!validOption.Contains(element)) optionList.RemoveAt(x);
    }
  }
}