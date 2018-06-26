using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	public Vector2 gridSize;
	public float nodeRadius;
	public List<Node> path;

	Node[,] grid;
	float nodeDiameter;
	Vector2Int numberOfCells;

	void Start() {
		this.nodeDiameter = this.nodeRadius * 2;
		this.numberOfCells = new Vector2Int(Mathf.RoundToInt(this.gridSize.x / this.nodeDiameter), Mathf.RoundToInt(this.gridSize.y / this.nodeDiameter));
		this.CreateGrid();
	}

	void CreateGrid() {
		this.grid = new Node[this.numberOfCells.x, this.numberOfCells.y];

		Vector3 worldBottomLeft = this.transform.position - (Vector3.right * this.gridSize.x / 2) - (Vector3.forward * this.gridSize.y / 2);

		for (int x = 0; x < this.numberOfCells.x; x++) {
			for (int y = 0; y < this.numberOfCells.y; y++) {
				Vector3 nodePosition = worldBottomLeft + Vector3.right * (x * this.nodeDiameter + this.nodeRadius) + Vector3.forward * (y * this.nodeDiameter + this.nodeRadius);
				bool isWalkable = !(Physics.CheckSphere(nodePosition, this.nodeRadius, this.unwalkableMask));
				this.grid[x,y] = new Node(isWalkable, nodePosition, new Vector2Int(x, y));
			}
		}
	}

	public Node GetNodeFromWorldPosition(Vector3 worldPosition) {
		float percentX = (worldPosition.x / this.gridSize.x) + 0.5f; // ex : if gridSize.x = 50, a worldPosition of 25 would be on the right border
		float percentY = (worldPosition.z / this.gridSize.y) + 0.5f;

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.FloorToInt(Mathf.Min(this.numberOfCells.x * percentX, this.numberOfCells.x - 1));
		int y = Mathf.FloorToInt(Mathf.Min(this.numberOfCells.y * percentY, this.numberOfCells.y - 1));

		return this.grid[x,y];
	}

	public List<Node> GetNeightbours(Node node) {
		List<Node> neighbours = new List<Node>();

		for(int x = -1; x <= 1; x++) {
			for(int y = -1; y <= 1; y++) {
				if(x == 0 && y == 0)
					continue;
				
				int checkX = node.gridIndice.x + x;
				int checkY = node.gridIndice.y + y;

				if(checkX >= 0 && checkX < this.numberOfCells.x && checkY >= 0 && checkY < this.numberOfCells.y) {
					neighbours.Add(this.grid[checkX,checkY]);
				}
			}
		}

		return neighbours;
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(this.transform.position, new Vector3(this.gridSize.x, 1, this.gridSize.y));
	
		if(this.grid != null) {
			foreach (Node node in this.grid) {
				Gizmos.color = node.isWalkable ? Color.green : Color.red;

				if(this.path != null) 
					if(this.path.Contains(node))
						Gizmos.color = Color.yellow;

				Gizmos.DrawCube(node.worldPosition, Vector3.one * (this.nodeDiameter - .05f));
			}
		}
	}
}
