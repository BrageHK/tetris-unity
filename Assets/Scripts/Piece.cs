using UnityEngine;
using UnityEngine.InputSystem;

public class Piece : MonoBehaviour {
    public Board board {get; private set;}
    public TetrominoData data {get; private set;}
    public Vector3Int position {get; private set;}
    public Vector3Int[] cells {get; private set;}
    public int rotationIndex {get; private set;}

    private float time = 0;
    private float fallTime = 0.75f;
    
    public void Initialize(Board board, Vector3Int position, TetrominoData data) {
        this.position = position;
        this.board = board;
        this.data = data;
        this.rotationIndex = 0;

        if(cells == null) {
            cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update() {
        board.Clear(this);

        time += Time.deltaTime;
        if(time > fallTime) {
            if(!board.IsValidPosition(this, position + Vector3Int.down)) {
                Lock();
            } else {
                Move(Vector2Int.down);
            }
    
            time = 0;
        }

        board.Set(this);
    }

    private void OnMove(InputValue value) {
        Vector2Int direction = Vector2Int.CeilToInt(value.Get<Vector2>());
        board.Clear(this);
        if(direction.x > 0.6f) {
            Move(Vector2Int.right);
        } else if(direction.x < -0.6f) {
            Move(Vector2Int.left);
        } else if(direction.y < -0.6f) {
            Move(Vector2Int.down);
            Vector3Int newPosition = position;
            newPosition.y -= 1;

            if(board.IsValidPosition(this, newPosition)) {
                time = 0;
            }
            
        }
        board.Set(this);
    }

    private bool Move(Vector2Int direction) {
        Vector3Int newPosition = position;
        newPosition.x += direction.x;
        newPosition.y += direction.y;

        bool valid = board.IsValidPosition(this, newPosition);

        if(valid) {
            position = newPosition;
        }

        return valid;
    }

    private void Lock() {
        board.Set(this);
        board.ClearLines();
        board.SpawnTetromino();
    }

    private void OnRotateLeft() {
        Rotate(-1);
    }

    private void OnRotateRight() {
        Rotate(1);
    }

    private void OnDrop() {
        board.Clear(this);
        while (Move(Vector2Int.down)) {
            continue;
        }

        Lock();
    }

    private void Rotate(int direction) {
        board.Clear(this);

        int originalRotation = rotationIndex;

        ApplyRotationMatrix(direction);

        rotationIndex = Wrap(0, 4, rotationIndex + direction);

        if (!WallKickTest(rotationIndex,direction)) {
            rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);

        }
    }

    private void ApplyRotationMatrix(int direction) {
        rotationIndex = Wrap(0,4,rotationIndex + direction);

        for (int i = 0; i < cells.Length; i++) {
            Vector3 cell = this.cells[i];

            int x, y;

            switch(this.data.tetromino) {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    //This wrap funtion is not a complete wrap function, but it works well for Tetris.
    private int Wrap(int min, int max, int value) {
        if (value < min) {
            return max - (min - value) % (max - min);
        } else {
            return min + (value - min) % (max - min);
        }
    }

    private bool WallKickTest(int rotationIndex, int rotationDirection) {
        int index = WallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < data.wallKicks.GetLength(1); i++) {
            Vector2Int translation = data.wallKicks[index, i];

            if (Move(translation)) {
                return true;
            }
        }

        return false;
    }

    private int WallKickIndex(int rotationIndex, int rotationDirection) {

        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0) {
            wallKickIndex--;
        }

        return Wrap(0, data.wallKicks.GetLength(0), wallKickIndex);

    }
}