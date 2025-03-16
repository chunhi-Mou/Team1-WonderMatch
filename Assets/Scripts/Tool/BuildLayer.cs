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
