using UnityEngine;

[SelectionBase]
public class Cell : MonoBehaviour {
    public BuildLayer buildPlayer;
    private void OnDrawGizmos() {
        if (buildPlayer == null) return;
        Gizmos.color = new Color(0, 0, 0, 0.1f);
        Gizmos.DrawCube(transform.position, 
            new Vector3(buildPlayer.gridCellSize.x, buildPlayer.gridCellSize.y, 0));
    }
}
