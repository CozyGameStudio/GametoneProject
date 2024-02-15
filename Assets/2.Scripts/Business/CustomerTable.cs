using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTable : MonoBehaviour
{
    private List<Sprite> foodBubbleSprites=new List<Sprite>();
    private Coroutine bubbleCoroutine;
    public SpriteRenderer foodBubbleSprite;
    public Transform servePosition;
    void Start(){
        Sprite[] sprites=Resources.LoadAll<Sprite>("FoodBubbles");
        foreach (var sprite in sprites)
        {
            foodBubbleSprites.Add(sprite);
        }
    }
    public void SetBubble(Food food, float duration)
    {
        foodBubbleSprite.gameObject.SetActive(true);
        foodBubbleSprite.sprite = foodBubbleSprites.Find(data => data.name.Contains(food.foodData.foodName));
        Debug.Log(foodBubbleSprite.sprite);
        if (bubbleCoroutine != null)
        {
            StopCoroutine(bubbleCoroutine);
        }

        bubbleCoroutine = StartCoroutine(DisableBubbleAfterTime(duration));
    }

    private IEnumerator DisableBubbleAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        foodBubbleSprite.gameObject.SetActive(false);
    }
}
