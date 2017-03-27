using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T> {

    T[] items;
    int cur_amount;

    public Heap(int max_size)
    {
        items = new T[max_size];
    }

    public void Add(T item)
    {
        item.HeapIndex = cur_amount;
        items[cur_amount] = item;
        SortUp(item);
        cur_amount++;
    }

    public T RemoveFirst()
    {
        T first = items[0];
        cur_amount--;
        items[0] = items[cur_amount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return first;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return cur_amount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while(true)
        {
            int child_left = item.HeapIndex * 2 + 1;
            int child_right = item.HeapIndex * 2 + 2;
            int index = 0;

            if (child_left < cur_amount)
            {
                index = child_left;

                if (child_right < cur_amount)
                {
                    if (items[child_left].CompareTo(items[child_right]) < 0)
                    {
                        index = child_right;
                    }
                }

                if (item.CompareTo(items[index]) < 0)
                {
                    Swap(item, items[index]);
                }
                else
                    return;
            }
            else
                return;
        }
    }

    void SortUp(T item)
    {
        int index = (item.HeapIndex - 1) / 2;

        while(true)
        {
            T parent = items[index];
            if (item.CompareTo(parent) > 0)
            {
                Swap(item, parent);
            }
            else
                break;

            index = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T item_one, T item_two)
    {
        items[item_one.HeapIndex] = item_two;
        items[item_two.HeapIndex] = item_one;

        int index = item_one.HeapIndex;

        item_one.HeapIndex = item_two.HeapIndex;
        item_two.HeapIndex = index;
    }

}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
