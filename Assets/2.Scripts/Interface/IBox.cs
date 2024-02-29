using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBox<T>
{
    void InitBox(T data);
    void ButtonClick();
}
