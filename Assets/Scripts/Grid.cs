using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;

	Node[,] grid;
	float nodeDiameter;
	Vector2Int gridSize;

	void Start() {
		this.nodeDiameter = this.nodeRadius * 2;
		this.gridSize = new Vector2Int(Mathf.RoundToInt(this.gridWorldSize.x / this.nodeDiameter), Mathf.RoundToInt(this.gridWorldSize.y / this.nodeDiameter));
		this.CreateGrid();
	}

	void CreateGrid() {
		this.grid = new Node[this.gridSize.x, this.gridSize.y];

		Vector3 worldBottomLeft = this.transform.position - (Vector3.right * this.gridWorldSize.x / 2) - (Vector3.forward * this.gridWorldSize.y / 2);

		for (int x = 0; x < this.gridSize.x; x++) {
			for (int y = 0; y < this.gridSize.y; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * this.nodeDiameter + this.nodeRadius) + Vector3.forward * (y * this.nodeDiameter + this.nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, this.nodeRadius, this.unwalkableMask));
				this.grid[x,y] = new Node(walkable, worldPoint);
			}
		}
	}

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + this.gridWorldSize.x / 2) / this.gridWorldSize.x;
		float percentY = (worldPosition.z + this.gridWorldSize.y / 2) / this.gridWorldSize.y;
	
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((this.gridSize.x - 1) * percentX);
		int y = Mathf.RoundToInt((this.gridSize.y - 1) * percentY);

		return this.grid[x,y];
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(this.transform.position, new Vector3(this.gridWorldSize.x, 1, this.gridWorldSize.y));
	
		if(this.grid != null) {
			foreach (Node node in this.grid) {
				Gizmos.color = node.walkable ? Color.green : Color.red;
				Gizmos.DrawCube(node.worldPosition, Vector3.one * (this.nodeDiameter - .05f));
			}
		}
	}
}
