using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

	public bool isWalkable;
	public Vector3 worldPosition;
	public Vector2Int gridIndice;
	public Node parent;

	public int gCost; // distance from starting node
	public int hCost; // distance from target

	public int fCost {
		get {
			return this.gCost + this.hCost;
		}
	}

	public Node(bool isWalkable, Vector3 worldPosition, Vector2Int gridIndice) {
		this.isWalkable = isWalkable;
		this.worldPosition = worldPosition;
		this.gridIndice = gridIndice;
	}
}
