using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugManager;
public class TutorialManagerForStageFour : MonoBehaviour
{
    private static TutorialManagerForStageFour instance;

    private Queue<int> tutorialQueue = new Queue<int>();
    private bool isTutorialActive = false;

    public GameObject mainPanel;

    public GameObject DarkFilter;
    

    public List<string> dialogueDataList;
    public List<Transform> dialogueBoxPositionDataList;
    public Image dialogueBox;
    public TMP_Text dialogueText;

    private GameObject nextButtonGameObject;
    private GameObject dialogueBoxGameObject;
    private GameObject dialogueTextGameObject;

    // ButtonClick
    public Button nextButton;

    private float timer = 0f;
    public float waitTimeForPushButton = 5f;

    public int tutorialIndex { get; private set; } = 0;
    private int currentDialogueIndex = 0;

    private bool buttonPressed = false;

    private bool isEnqueueForTutorialOne = false;

    public static TutorialManagerForStageFour Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialManagerForStageFour>();
            }
            return instance;
        }
    }
    public void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (DarkFilter != null) DarkFilter.SetActive(false);
        
        if (nextButton != null) nextButtonGameObject = nextButton.transform.gameObject;
        if (nextButtonGameObject != null)
        {
            nextButtonGameObject.SetActive(false);
        }
        
        dialogueBoxGameObject = dialogueBox.transform.gameObject;
        if (dialogueBoxGameObject != null)
        {
            dialogueBoxGameObject.SetActive(false);
        }
        dialogueTextGameObject = dialogueText.transform.gameObject;
        if (dialogueTextGameObject != null)
        {
            dialogueTextGameObject.SetActive(false);
        }
        EnqueueTutorial(1);
    }

    public void EnqueueTutorial(int tutorialId)
    {
        Debug.Log("Enqueue" + tutorialId);
        tutorialQueue.Enqueue(tutorialId);
        TryStartNextTutorial();
    }

    private void TryStartNextTutorial()
    {
        if (!isTutorialActive && tutorialQueue.Count > 0)
        {
            isTutorialActive = true;
            Debug.Log("queue" + tutorialQueue.Count);
            int tutorialId = tutorialQueue.Dequeue();
            StartTutorial(tutorialId);
        }
    }

    void StartTutorial(int tutorialId)
    {
        Debug.Log($"Starting tutorial ID: {tutorialId}");
        switch (tutorialId)
        {
            case 1:
                StartCoroutine(StartTutorialOne());
                break;
        }
    }

    public void OnButtonClickForNext()
    {
        buttonPressed = true;
    }


    public void CompleteCurrentTutorial()
    {
        isTutorialActive = false;
        TryStartNextTutorial();
    }

    IEnumerator StartTutorialOne()
    {

        //이미 해금된 장비가 있다면 1,2번 튜토리얼은 진행하지 않음
        if (DataManager.Instance.activeMachines.Count != 0)
        {
            isEnqueueForTutorialOne = true;
            CompleteCurrentTutorial();
            yield break;
        }
        tutorialIndex = 1;

        mainPanel.SetActive(true);
        DarkFilter.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[currentDialogueIndex].position;
        dialogueTextGameObject.SetActive(true);
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());
        DarkFilter.SetActive(false);

        //businessButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        //yield return new WaitUntil(() => isBusinessButtonTouch);

        

        CompleteCurrentTutorial();
    }

    IEnumerator PushNextButton()
    {
        nextButton.transform.gameObject.SetActive(true);
        while (!buttonPressed && timer < waitTimeForPushButton)
        {
            yield return null;
            timer += Time.deltaTime;

        }
        nextButton.transform.gameObject.SetActive(false);
        buttonPressed = false;
        timer = 0;
    }
}
