using System;
using UnityEngine;

public class BuildLayer : MonoBehaviour {
    public Vector2 gridSize;
    public GameObject[,] cells;
    public Vector2 gridCellSize;
    public void BuildCells() {
        cells = new GameObject[(int)gridSize.x, (int)gridSize.y];
        for (int x = 0; x < gridSize.x; x++) {
            for (int y = 0; y < gridSize.y; y++) {
                Vector3 pos = CalculatePos(x, y);

                GameObject cell = new GameObject();
                cell.transform.parent = transform;
                cell.transform.position = pos;
                cell.name = $"Cell(x:{x}, y:{y})";

                Cell cellScript = cell.AddComponent<Cell>();
                cellScript.buildPlayer = this;

                cells[x, y] = cell;
            }
        }
    }
    public void ModifyCells(GameObject cell, Transform transform, Vector2 cellSize) {
        cell.name = cell.name.Replace("Cell(", "").Replace(")", "");
        string[] parts = cell.name.Split(new[] { "x:", ", y:" }, StringSplitOptions.RemoveEmptyEntries);

        int x = int.Parse(parts[0]);
        int y = int.Parse(parts[1]);

        Vector3 pos = new Vector3(transform.position.x + (x * cellSize.x), transform.position.y + (y * cellSize.y), 0);

        cell.transform.parent = transform;
        cell.transform.position = pos;
        Cell cellScript = cell.GetComponent<Cell>();
        cellScript.buildPlayer = this;

        cell.name = $"Cell_{x}_{y}";
    }
    private void OnDrawGizmos() {
        for (int x = 0; x < gridSize.x; x++) {
            for (int y = 0; y < gridSize.y; y++) {
                Vector3 pos = CalculatePos(x, y);
                Gizmos.DrawWireCube(pos, new Vector3(gridCellSize.x, gridCellSize.y, 0));
            }
        }
    }
    private Vector3 CalculatePos(int x, int y) {
        return new Vector3(transform.position.x + (x * gridCellSize.x), transform.position.y + (y * gridCellSize.y), 0);
    }
}
