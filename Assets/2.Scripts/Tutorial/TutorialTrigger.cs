using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public int tutorialId = 3;
    private bool isTrigger = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Customer")&&!isTrigger)
        {
            Debug.Log("Trigger");
            isTrigger = true;
            TutorialManager.Instance.GetObject(other.gameObject);
            TutorialManager.Instance.EnqueueTutorial(tutorialId);
        }
    }
}
