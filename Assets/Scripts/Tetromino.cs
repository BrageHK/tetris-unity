using UnityEngine;
using UnityEngine.Tilemaps;
public enum Tetromino {
    I,
    J,
    L,
    O,
    S,
    T,
    Z
}

[System.Serializable]
public struct TetrominoData {
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells {get; private set;}
    public Vector2Int[,] wallKicksI {get; private set;}
    public Vector2Int[,] wallKicks {get; private set;}
   
   
    public void Initialize() {
        cells = Data.Cells[tetromino];
        wallKicks = Data.WallKicks[tetromino];
    }
}