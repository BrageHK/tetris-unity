using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour {
    public TetrominoData[] tetrominoes;
    public Piece activePiece {get; private set;}
    public Tilemap tilemap {get; private set;}
    public int Score {get; private set;}
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);
    public Vector2Int boardSize = new Vector2Int(10, 20);
    
    public RectInt Bounds {
        get {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake() {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < tetrominoes.Length; i++) {
            tetrominoes[i].Initialize();
        }
    }

    private void Start() {
        SpawnTetromino();
    }

    public void SpawnTetromino() {
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];    

        activePiece.Initialize(this, spawnPosition, data);

        if(IsValidPosition(activePiece, spawnPosition)) {
            Set(this.activePiece);
        } else {
            GameOver();
        }
        
    }

    public void Set(Piece piece) {
        for (int i = 0; i <  piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece) {
        for (int i = 0; i <  piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece ,Vector3Int posToCheck) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int cellPosition = piece.cells[i] + posToCheck;

            if (this.tilemap.HasTile(cellPosition)) {
                return false;
            }

            if (!Bounds.Contains((Vector2Int)cellPosition)) {
                return false;
            }
        }
        return true;
    }

    public void GameOver() {
        Debug.Log("Game over!");
        //tilemap.ClearAllTiles();
    }

    public void ClearLines() {
        RectInt bounds = Bounds;

        int row = bounds.yMin;

        while (row < bounds.yMax) {
            if(IsLineFull(row)) {
                ClearLine(row);
            } else {
                row++;
            }
        }

        for (int y = bounds.yMin; y < bounds.yMax; y++) {

            
        }
    } 

    public void ClearLine(int line) {
        RectInt bounds = Bounds;

        if (!IsLineFull(line)) {
            return;
        }

        //Removes a line. 
        for (int i = bounds.xMin; i < bounds.xMax; i++){
            Vector3Int position = new Vector3Int(i, line, 0);
            tilemap.SetTile(position, null);
        }

        //Moves every tile above the "line" tile down one step.
        while (Bounds.yMax > line) {

            for (int x = bounds.xMin; x < bounds.xMax; x++) {

                Vector3Int position = new Vector3Int(x, line + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(x, line, 0);
                tilemap.SetTile(position, above);
            }

            line++;
        }
    }

    public bool IsLineFull(int line) {
        RectInt bounds = Bounds;

        for (int i = bounds.xMin; i < bounds.xMax; i++) {
            if (!tilemap.HasTile(new Vector3Int(i, line, 0))) {
                return false;
            }
        }

        return true;
    }
}
