using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

	public bool isWalkable;
	public Vector3 worldPosition;
	public Vector2Int gridIndice;
	public Node parent;

	public int gCost; // distance from starting node
	public int hCost; // distance from target

	int heapIndex;

	public int fCost {
		get {
			return this.gCost + this.hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public Node(bool isWalkable, Vector3 worldPosition, Vector2Int gridIndice) {
		this.isWalkable = isWalkable;
		this.worldPosition = worldPosition;
		this.gridIndice = gridIndice;
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = this.fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = this.hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
}
