using System;
using System.Collections.Generic;

public class Deque<T>
{
    private LinkedList<T> list = new LinkedList<T>();

    public void AddFront(T item)
    {
        list.AddFirst(item);
    }

    public void AddRear(T item)
    {
        list.AddLast(item);
    }

    public T RemoveFront()
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Deque is empty");
        }
        T value = list.First.Value;
        list.RemoveFirst();
        return value;
    }

    public T RemoveRear()
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Deque is empty");
        }
        T value = list.Last.Value;
        list.RemoveLast();
        return value;
    }

    public int Count
    {
        get { return list.Count; }
    }
    public T Peek()
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Deque is empty");
        }
        return list.First.Value;
    }
    public bool IsDequeEmpty()
    {
        return list.Count==0;
    }
}
