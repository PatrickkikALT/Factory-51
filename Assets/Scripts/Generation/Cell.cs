using UnityEngine;

public class Cell : MonoBehaviour {
  public bool collapsed;
  public Tile[] tiles;

  public void CreateCell(bool state, Tile[] tileOpt) {
    collapsed = state;
    tiles = tileOpt;
  }

  public void RecreateCell(Tile[] tileOpt) {
    tiles = tileOpt;
  }
}