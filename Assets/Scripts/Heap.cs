using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T> {

	T[] items;
	int currentItemCount;

	public Heap(int maxHeapSize) {
		this.items = new T[maxHeapSize];
	}

	public void Add(T item) {
		item.HeapIndex = this.currentItemCount;
		this.items[this.currentItemCount] = item;
		this.SortUp(item);
		this.currentItemCount++;
	}

	public T RemoveFirst() {
		T firstItem = this.items[0];
		this.currentItemCount--;
		this.items[0] = this.items[currentItemCount];
		this.items[0].HeapIndex = 0;
		this.SortDown(this.items[0]);
		return firstItem;
	}

	public void UpdateItem(T item) {
		this.SortUp(item);
	}

	public int Count {
		get {
			return this.currentItemCount;
		}
	}

	public bool Contains(T item) {
		return Equals(this.items[item.HeapIndex], item);
	}

	void SortDown(T item) {
		while (true) {
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = item.HeapIndex * 2 + 2;
			int swapIndex = 0;

			if (childIndexLeft < this.currentItemCount) {
				swapIndex = childIndexLeft;

				if (childIndexRight < this.currentItemCount) {
					if (this.items[childIndexLeft].CompareTo(this.items[childIndexRight]) < 0) {
						swapIndex = childIndexRight;
					}
				}

				if (item.CompareTo(this.items[swapIndex]) < 0) {
					this.Swap(item, this.items[swapIndex]);
				}
				else {
					return;
				}

			}
			else {
				return;
			}

		}
	}

	void SortUp(T item) {
		int parentIndex = (item.HeapIndex - 1) / 2;
		
		while (true) {
			T parentItem = this.items[parentIndex];
			if (item.CompareTo(parentItem) > 0) {
				this.Swap(item, parentItem);
			}
			else {
				break;
			}

			parentIndex = (item.HeapIndex-1)/2;
		}
	}

	void Swap(T itemA, T itemB) {
		this.items[itemA.HeapIndex] = itemB;
		this.items[itemB.HeapIndex] = itemA;

		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}
}

public interface IHeapItem<T> : IComparable<T> {
	int HeapIndex {
		get;
		set;
	}
}
