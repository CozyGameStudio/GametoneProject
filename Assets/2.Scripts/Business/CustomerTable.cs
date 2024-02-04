using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerTable : MonoBehaviour
{
    public GameObject foodBubble;
    private Coroutine bubbleCoroutine; 

    public void SetBubble(Food food, float duration)
    {
        foodBubble.SetActive(true);
        SpriteRenderer foodSprite = foodBubble.transform.GetChild(0).GetComponent<SpriteRenderer>();
        foodSprite.sprite = food.foodData.foodIcon;

        if (bubbleCoroutine != null)
        {
            StopCoroutine(bubbleCoroutine);
        }

        bubbleCoroutine = StartCoroutine(DisableBubbleAfterTime(duration));
    }

    private IEnumerator DisableBubbleAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        foodBubble.SetActive(false);
    }
}
