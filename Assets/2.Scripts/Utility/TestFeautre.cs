using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFeautre : MonoBehaviour
{
    //this code is just for test.
    //In Build, it should be removed.
    public void AddMoney(int money){
        BusinessGameManager.Instance.ChangeSpeed(money);
    }
    public void ChangeSpeed(float multiplier){
            BusinessGameManager.Instance.ChangeSpeed(multiplier);
    }
    public void LocalLoad(){
        DataLoadManager.Instance.LoadLocalData();
    }
    public void OnlineLoad(){
        DataLoadManager.Instance.LoadOnlineData();
    }
}
