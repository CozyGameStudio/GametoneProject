using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugManager;
public class TutorialManagerForInterior : MonoBehaviour
{
    private static TutorialManagerForInterior instance;

    private Queue<int> tutorialQueue = new Queue<int>();
    private bool isTutorialActive = false;

    public GameObject presetWindow;
    public GameObject interiorWindow;
    public GameObject comfortWindow;
    public GameObject comfortBar;

    public GameObject DarkFilter;
    public GameObject mask1;
    public GameObject mask2;
    public GameObject interiorButtonFilter;
    public GameObject interiorPresetFilter;
    public GameObject interiorPresetButtonFilter;
    public GameObject interiorPanelFilter;
    public GameObject interiorPositionsFilter;
    public GameObject interiorChoiceFilter;
    public GameObject interiorBuyPanelFilter;
    public GameObject interiorBuyPanelComfortFilter;
    public GameObject interiorBuyPanelCostFilter;
    public GameObject interiorBuyPanelBuyFilter;
    public GameObject interiorFocusFilter;
    public GameObject interiorPositionButtonFilter;
    public GameObject ComfortButtonFilter;
    public GameObject ComfortFilter;


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

    private bool isInteriorButtonTouch = false;
    private bool isPresetButtonTouch = false;
    private bool isInteriorChoiceTouch = false;
    private bool isInteriorBuyTouch = false;
    private bool isInteriorPositionTouch = false;
    private bool isComfortButtonTouch = false;

    private bool isEnqueueForTutorialOne = false;



    public static TutorialManagerForInterior Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialManagerForInterior>();
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
        if (mask1 !=  null) mask1.SetActive(false);
        if (mask2 != null) mask2.SetActive(false);
        if (interiorButtonFilter != null) interiorButtonFilter.SetActive(false);
        if (interiorPresetFilter != null) interiorPresetFilter.SetActive(false);
        if (interiorPresetButtonFilter != null) interiorPresetButtonFilter.SetActive(false);
        if (interiorPanelFilter != null) interiorPanelFilter.SetActive(false);
        if (interiorPositionsFilter != null) interiorPositionsFilter.SetActive(false);
        if (interiorChoiceFilter != null) interiorChoiceFilter.SetActive(false);
        if (interiorBuyPanelFilter != null) interiorBuyPanelFilter.SetActive(false);
        if (interiorBuyPanelComfortFilter != null) interiorBuyPanelComfortFilter.SetActive(false);
        if (interiorBuyPanelCostFilter != null) interiorBuyPanelCostFilter.SetActive(false);
        if (interiorBuyPanelBuyFilter != null) interiorBuyPanelBuyFilter.SetActive(false);
        if (interiorFocusFilter != null) interiorFocusFilter.SetActive(false);
        if (interiorPositionButtonFilter != null) interiorPositionButtonFilter.SetActive(false);
        if (ComfortButtonFilter != null) ComfortButtonFilter.SetActive(false);
        if (ComfortFilter != null) ComfortFilter.SetActive(false);

        if (comfortBar != null) comfortBar.SetActive(false);

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
        if (InteriorSceneManager.Instance.comfort != 0)
        {
            comfortBar.SetActive(true);
            isEnqueueForTutorialOne = true;
            CompleteCurrentTutorial();
            yield break;
        }
        tutorialIndex = 1;

        interiorButtonFilter.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[currentDialogueIndex].position;
        dialogueTextGameObject.SetActive(true);
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isInteriorButtonTouch);

        interiorButtonFilter.SetActive(false);
        interiorPresetFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());
        
        interiorPresetFilter.SetActive(false);
        interiorPresetButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isPresetButtonTouch);

        interiorPresetButtonFilter.SetActive(false);
        interiorPanelFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        interiorPanelFilter.SetActive(false);
        interiorPositionsFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        interiorPositionsFilter.SetActive(false);
        interiorChoiceFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isInteriorChoiceTouch);

        interiorChoiceFilter.SetActive(false);
        interiorBuyPanelFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        interiorBuyPanelFilter.SetActive(false);
        interiorBuyPanelComfortFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        interiorBuyPanelComfortFilter.SetActive(false);
        interiorBuyPanelCostFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        interiorBuyPanelCostFilter.SetActive(false);
        interiorBuyPanelBuyFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isInteriorBuyTouch);

        interiorBuyPanelBuyFilter.SetActive(false);
        interiorFocusFilter.SetActive(true);
        mask1.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        mask1.SetActive(false);
        interiorFocusFilter.SetActive(false);
        interiorPositionButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isInteriorPositionTouch);

        interiorPositionButtonFilter.SetActive(false);

        dialogueBoxGameObject.SetActive(false);
        interiorButtonFilter.SetActive(false);


        CompleteCurrentTutorial();
    }

    IEnumerator StartTutorialTwo()
    {
        presetWindow.SetActive(false);
        interiorWindow.SetActive(false);
        dialogueBoxGameObject.SetActive(true);
        interiorFocusFilter.SetActive(true);
        mask2.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        interiorFocusFilter.SetActive(false);
        mask2.SetActive(false);
        comfortBar.SetActive(true);
        ComfortButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isComfortButtonTouch);
        
        
        ComfortButtonFilter.SetActive(false);
        ComfortFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        ComfortFilter.SetActive(false);
        comfortWindow.SetActive(false);
        DarkFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        DarkFilter.SetActive(false);
        dialogueBoxGameObject.SetActive(false);


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



    public void InteriorButtonTouch()
    {
        isInteriorButtonTouch = true;
    }

    public void PresetButtonTouch()
    {
        isPresetButtonTouch = true;
    }

    public void InteriorChoiceTouch()
    {
        isInteriorChoiceTouch = true;
    }

    public void InteriorBuyTouch()
    {
        isInteriorBuyTouch = true;
    }

    public void InteriorPositionTouch()
    {
        isInteriorPositionTouch = true;
    }

    public void InteriorBuySecond()
    {
        EnqueueTutorial(2);
    }

    public void ComfortButtonTouch()
    {
        isComfortButtonTouch = true;
    }
}
