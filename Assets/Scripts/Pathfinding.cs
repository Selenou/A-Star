using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

	public Transform seeker, target;

	Grid grid;

	void Awake() {
		this.grid = GetComponent<Grid>();
	}

	void Update(){
		this.FindPath(this.seeker.position, this.target.position);
	}

	void FindPath(Vector3 startPosition, Vector3 targetPosition) {
		Node startNode = this.grid.GetNodeFromWorldPosition(startPosition);
		Node targetNode = this.grid.GetNodeFromWorldPosition(targetPosition);

		Heap<Node> openSet = new Heap<Node>(this.grid.MaxSize);
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while(openSet.Count > 0) {
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);

			if(currentNode == targetNode) {
				this.RetracePath(startNode, targetNode);
				return;
			}

			foreach(Node neighbour in this.grid.GetNeightbours(currentNode)) {
				if(!neighbour.isWalkable || closedSet.Contains(neighbour)) 
					continue;
				
				int newCostToNeighbour = currentNode.gCost + this.GetDistance(currentNode, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = this.GetDistance(neighbour, targetNode);
					neighbour.parent = currentNode;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
					else 
						openSet.UpdateItem(neighbour);
				}
				
			}
		}
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		this.grid.path = path;
	}

	int GetDistance(Node nodeA, Node nodeB) {
		int distanceX = Mathf.Abs(nodeA.gridIndice.x - nodeB.gridIndice.x);
		int distanceY = Mathf.Abs(nodeA.gridIndice.y - nodeB.gridIndice.y);

		// 14 = (sqrt of 2) * 10 for diagonal move (pythagore)
		if(distanceX > distanceY)
			return 14 * distanceY + 10 * (distanceX - distanceY);

		return 14 * distanceX + 10 * (distanceY - distanceX);
	}
}
