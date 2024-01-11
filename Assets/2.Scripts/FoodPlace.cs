using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPlace : MonoBehaviour
{
    // Define a delegate
    public delegate void ChildHandler(Transform child);

    // Create an event based on the delegate
    public event ChildHandler OnChildAdded;
    public event ChildHandler OnChildRemoved;
    public bool IsAvailable => gameObject.transform.childCount==0;
    public void AddChild(GameObject child)
    {
        child.transform.SetParent(transform);
        OnChildAdded?.Invoke(child.transform);
    }
    public void RemoveChild(GameObject child){
        if (child.transform.parent == transform)
        {
            // 자식 객체의 부모 설정을 해제
            child.transform.SetParent(null);
            // 자식 객체가 제거됨을 알리는 이벤트 발생
            OnChildRemoved?.Invoke(child.transform);
        }
    }
}
