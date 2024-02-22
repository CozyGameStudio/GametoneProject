using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugManager;

public class TutorialManagerForStageTwo : MonoBehaviour
{
    private static TutorialManagerForStageTwo instance;

    private Queue<int> tutorialQueue = new Queue<int>();
    private bool isTutorialActive = false;

    public GameObject money;

    public GameObject businessButton;
    public GameObject characterCollectionButton;

    public GameObject DarkFilter;
    public GameObject businessButtonFilter;
    public GameObject businessPanelCharacterButtonFilter;
    public GameObject characterHappyFilter;
    public GameObject businessPanelFilter;
    public GameObject characterLevelFilter;
    public GameObject characterCostFilter;
    public GameObject characterUpgradeFilter;
    public GameObject businessCloseFilter;

    public GameObject characterCollectionButtonFilter;
    public GameObject characterCollectionFilter;
    public GameObject characterCollectionChoiceFilter;
    public GameObject characterStatFilter;
    public GameObject characterStoryBuyFilter;
    public GameObject characterStoryButtonFilter;

    public List<string> dialogueDataList;
    public List<Transform> dialogueBoxPositionDataList;
    public Image dialogueBox;
    public TMP_Text dialogueText;

    private GameObject nextButtonGameObject;
    private GameObject dialogueBoxGameObject;
    private GameObject dialogueTextGameObject;

    // ButtonClick
    public Button nextButton;

    private bool isBusinessButtonTouch = false;
    private bool isBusinessPanelEmploymentButtonTouch = false;
    private bool isCharacterButtonTouch = false;
    private bool isCharacterUpgradeButtonTouch = false;
    private bool isBusinessCloseTouch = false;
    private bool isCharacterCollectionButtonTouch = false;
    private bool isCharacterCollectionChoiceTouch = false;
    private bool isCharacterStoryLockButtonTouch = false;
    private bool isCharacterStoryBuyButtonTouch = false;
    private bool isCharacterStoryButtonTouch = false;

    // UI Position
    RectTransform uiBusinessButton;
    RectTransform uiMoney;

    private float timer = 0f;
    public float waitTimeForPushButton = 5f;

    public int tutorialIndex { get; private set; } = 0;
    private int currentDialogueIndex = 0;

    private bool buttonPressed = false;

    private bool isEnqueueForTutorialOne = false;

    public static TutorialManagerForStageTwo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialManagerForStageTwo>();
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
        if (DarkFilter != null) DarkFilter.SetActive(false);
        if (businessButtonFilter != null) businessButtonFilter.SetActive(false);
        if(businessPanelCharacterButtonFilter != null) businessPanelCharacterButtonFilter.SetActive(false);
        if (businessPanelFilter != null) businessPanelFilter.SetActive(false);
        if (characterHappyFilter != null) characterHappyFilter.SetActive(false);
        if (businessCloseFilter != null) businessCloseFilter.SetActive(false);
        if (characterLevelFilter != null) characterLevelFilter.SetActive(false);
        if (characterCostFilter != null) characterCostFilter.SetActive(false);
        if (characterUpgradeFilter != null) characterUpgradeFilter.SetActive(false);
        if (characterCollectionButtonFilter != null) characterCollectionButtonFilter.SetActive(false);
        if (characterCollectionFilter != null) characterCollectionFilter.SetActive(false);
        if (characterCollectionChoiceFilter != null) characterCollectionChoiceFilter.SetActive(false);
        if (characterStoryButtonFilter != null) characterStoryButtonFilter.SetActive(false);
        if (characterStatFilter != null) characterStatFilter.SetActive(false);
        if (characterStoryBuyFilter != null) characterStoryBuyFilter.SetActive(false);

        if (nextButton != null) nextButtonGameObject = nextButton.transform.gameObject;
        if (nextButtonGameObject != null)
        {
            nextButtonGameObject.SetActive(false);
        }
        if (characterCollectionButton != null)
        {
            characterCollectionButton.SetActive(false);
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
        uiMoney = money.GetComponent<RectTransform>();
        uiBusinessButton = businessButton.GetComponent<RectTransform>();

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
            case 2:
                StartCoroutine(StartTutorialTwo());
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
            characterCollectionButton.SetActive(true);
            CompleteCurrentTutorial();
            yield break;
        }
        tutorialIndex = 1;


        DarkFilter.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[currentDialogueIndex].position;
        Debug.Log(currentDialogueIndex);
        dialogueTextGameObject.SetActive(true);
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());
        DarkFilter.SetActive(false);
        businessButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isBusinessButtonTouch);

        businessButtonFilter.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        dialogueTextGameObject.SetActive(false);
        businessPanelCharacterButtonFilter.SetActive(true);
        yield return new WaitUntil(() => isBusinessPanelEmploymentButtonTouch);

        businessPanelCharacterButtonFilter.SetActive(false);
        businessPanelFilter.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        dialogueTextGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        businessPanelFilter.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        dialogueTextGameObject.SetActive(false);
        characterHappyFilter.SetActive(true);
        yield return new WaitUntil(() => isCharacterButtonTouch);
        characterHappyFilter.SetActive(false);

        businessPanelFilter.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        dialogueTextGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        businessPanelFilter.SetActive(false);
        characterLevelFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        characterLevelFilter.SetActive(false);
        characterCostFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        dialogueBoxGameObject.SetActive(false);
        dialogueTextGameObject.SetActive(false);
        characterCostFilter.SetActive(false);
        characterUpgradeFilter.SetActive(true);
        yield return new WaitUntil(() => isCharacterUpgradeButtonTouch);

        dialogueBoxGameObject.SetActive(true);
        dialogueTextGameObject.SetActive(true);
        characterUpgradeFilter.SetActive(false);
        businessPanelFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        dialogueBoxGameObject.SetActive(false);
        dialogueTextGameObject.SetActive(false);
        businessPanelFilter.SetActive(false);
        businessCloseFilter.SetActive(true);
        yield return new WaitUntil(() => isBusinessCloseTouch);

        businessCloseFilter.SetActive(false);

        CompleteCurrentTutorial();
        EnqueueTutorial(2);
    }

    IEnumerator StartTutorialTwo()
    {
        tutorialIndex = 2;

        characterCollectionButton.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        dialogueTextGameObject.SetActive(true);
        characterCollectionButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isCharacterCollectionButtonTouch);

        characterCollectionButtonFilter.SetActive(false);
        characterCollectionFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        characterCollectionFilter.SetActive(false);
        characterCollectionChoiceFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isCharacterCollectionChoiceTouch);

        dialogueBoxGameObject.SetActive(false);
        dialogueTextGameObject.SetActive(false);
        characterCollectionFilter.SetActive(true);
        yield return StartCoroutine(PushNextButton());

        dialogueBoxGameObject.SetActive(true);
        dialogueTextGameObject.SetActive(true);
        characterCollectionFilter.SetActive(false);
        characterStatFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        characterStatFilter.SetActive(false);
        characterStoryButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isCharacterStoryLockButtonTouch);

        characterStoryButtonFilter.SetActive(false);
        characterStoryBuyFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isCharacterStoryBuyButtonTouch);

        characterStoryBuyFilter.SetActive(false);
        characterStoryButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isCharacterStoryButtonTouch);

        characterStoryButtonFilter.SetActive(false);
        characterCollectionFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        characterCollectionFilter.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        dialogueTextGameObject.SetActive(false);

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

    public void BusinessButtonTouch()
    {
        if (tutorialIndex == 1)
        {
            isBusinessButtonTouch = true;
            Debug.Log("BusinessClick");
        }
    }

    public void BusinessPanelEmploymentButtonTouch()
    {
        if (tutorialIndex == 1)
        {
            isBusinessPanelEmploymentButtonTouch = true;
            Debug.Log("BusinessClick");
        }
    }

    public void CharacterButtonTouch()
    {
        if (tutorialIndex == 1)
        {
            isCharacterButtonTouch = true;
            Debug.Log("BusinessClick");
        }
    }

    public void CharacterUpgradeButtonTouch()
    {
        if (tutorialIndex == 1)
        {
            isCharacterUpgradeButtonTouch = true;
        }
    }

    public void BusinessClose()
    {
        if (tutorialIndex == 1)
        {
            isBusinessCloseTouch = true;
        }
    }

    public void CharacterCollectionButtonTouch()
    {
        if (tutorialIndex == 2)
        {
            isCharacterCollectionButtonTouch = true;
        }
    }

    public void CharacterCollectionChoiceTouch()
    {
        if (tutorialIndex == 2)
        {
            isCharacterCollectionChoiceTouch = true;
        }
    }

    public void CharacterStoryLockButtonTouch()
    {
        if (tutorialIndex == 2)
        {
            isCharacterStoryLockButtonTouch = true;
        }
    }

    public void CharacterStoryBuyButtonTouch()
    {
        if (tutorialIndex == 2)
        {
            isCharacterStoryBuyButtonTouch = true;
        }
    }

    public void CharacterStoryButtonTouch()
    {
        if (tutorialIndex == 2)
        {
            isCharacterStoryButtonTouch = true;
        }
    }



}
