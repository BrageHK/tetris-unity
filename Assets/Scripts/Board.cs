using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class Board : MonoBehaviour {
    public TetrominoData[] tetrominoes;
    public Piece activePiece {get; private set;}
    public Tilemap tilemap {get; private set;}
    public int score {get; private set;}
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);
    public Vector2Int boardSize = new Vector2Int(10, 20);
    private bool lastScoreWasDifficult;
    public int level, combo, totalClearedLines;
    public int lastLevel {get; private set;}
    public TextMeshProUGUI scoreText, levelText, comboText, linesClearedText, speedText;
    
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
        lastScoreWasDifficult = false;
        totalClearedLines = 0;
        level = 0;
        combo = 0;
        lastLevel = 0;
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
        //Show score
        //Show menu

        tilemap.ClearAllTiles();
        ResetScore();
        ResetLevel();
    }

    public void ClearLines() {
        RectInt bounds = Bounds;

        int row = bounds.yMin;
        int linesCleared = 0;

        while (row < bounds.yMax) {
            if(IsLineFull(row)) {
                ClearLine(row);
                linesCleared++;
            } else {
                row++;
            }
        }

        CountScore(linesCleared);
        UpdateLevel(linesCleared);
    } 

    public void CountScore(int linesCleared) {
        switch(linesCleared) {
            case 1:
                AddScore(100);
                break;
            case 2:
                AddScore(300);
                break;
            case 3:
                AddScore(500);
                break;
            case 4:
                Tetris();
                break;
            default:
                break;
        }

        UpdateScore();
        
    }

    private void AddScore(int score) {
        lastScoreWasDifficult = false;
        this.score += score * (level + 1);
    }

    private void Tetris() {
        if(lastScoreWasDifficult) {
            score += Mathf.CeilToInt(800 * (level + 1) * 1.5f);
        } else {
            score += 800 * (level + 1);
        }

        lastScoreWasDifficult = true;
    }

    public void UpdateScore() {
        scoreText.text = "Score: " + score.ToString();
    }

    public void ResetScore() {
        score = 0;
        UpdateScore();
    }

    public void ResetLevel() {
        level = 0;
        levelText.text = "Level: " + level.ToString();
    }

    public void UpdateLevel(int linesCleared) {
        totalClearedLines += linesCleared;
        UpdateClearedLines();
        level = Mathf.FloorToInt(totalClearedLines/ 5);
        levelText.text = "Level: " + level.ToString();
    }

    public void UpdateClearedLines() {
        linesClearedText.text = "Lines Cleared: " + totalClearedLines.ToString();
    }

    public void SoftDropAddPoint() {
        score +=1;
    }

    public void HardDropAddPoint() {
        score +=2;
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

    public void EqualLevels() {
        lastLevel = level;
    }

    public void SetSpeedText(string speed) {
        this.speedText.text = speed;
    }
}
