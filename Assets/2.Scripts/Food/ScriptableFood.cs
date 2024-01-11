using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "GametoneProject/Food", order = 0)]
public class ScriptableFood : ScriptableObject
{
    [SerializeField]
    private string foodName;
    public string FoodName { get { return foodName; } }
    [SerializeField]
    private int money;
    public int Money { get { return money; } }
    [SerializeField]
    private int cookTime;
    public int CookTime { get { return cookTime; } }
    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } }
    [SerializeField]
    private GameObject foodPrefab;
    public GameObject FoodPrefab { get { return foodPrefab; } }

}

