using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostBlock : MonoBehaviour {   
    public Vector3Int ghostPosition {get; private set;}
    public Vector3Int[] cells {get; private set;}
    public Tilemap tilemap {get; private set;}
    public Tile tile;
    public Board mainBoard;
    public Piece mainPiece;

    void Awake() {
        tilemap = GetComponentInChildren<Tilemap>();
        cells = new Vector3Int[4];
    }

    public void LateUpdate() {
        ClearGhost();
        Copy();
        PositionUpdate();
        SetGhost();
    }

    public void PositionUpdate() {

        Vector3Int position = mainPiece.position;

        int current = position.y;
        int bottom = -mainBoard.boardSize.y / 2 - 1;

        mainBoard.Clear(mainPiece);

        for (int row = current; row >= bottom; row--)
        {
            position.y = row;

            if (mainBoard.IsValidPosition(mainPiece, position)) {
                this.ghostPosition = position;
            } else {
                break;
            }
        }

        mainBoard.Set(mainPiece);
    }

    private void Copy() {
        for (int i = 0; i < cells.Length; i++) {
            cells[i] = mainPiece.cells[i];
        }
    }

    public void ClearGhost() {
        for (int i = 0; i <  cells.Length; i++) {
            Vector3Int tilePosition = cells[i] + ghostPosition;
            this.tilemap.SetTile(tilePosition, null);
        }
    }
    public void SetGhost() {
        for (int i = 0; i < cells.Length; i++) {
            Vector3Int tilePosition = ghostPosition + mainPiece.cells[i];
            tilemap.SetTile(tilePosition, tile);
        }
    }
}
