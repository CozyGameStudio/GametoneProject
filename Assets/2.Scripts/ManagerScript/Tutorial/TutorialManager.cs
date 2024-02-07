using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.Profiling.Memory.Experimental;
using System;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;

    private Queue<int> tutorialQueue = new Queue<int>();
    private bool isTutorialActive = false;
    public GameObject DarkFilter;
    public GameObject close;
    public GameObject open;
    public GameObject touch;
    public GameObject businessButton;
    public GameObject ProcessBarButton;
    public GameObject money;
    public GameObject mask;
    public GameObject businessFilter;
    public GameObject businessBuyPanelFilter;
    public GameObject businessCloseFilter;

    public List<string> dialogueDataList;
    public List<Transform> dialogueBoxPositionDataList;
    public Image dialogueBox;
    public TMP_Text dialogueText;
    public GameObject prefabMask;

    [Header("Mask Position")]
    public GameObject chefPosition;
    public GameObject serverPosition;
    public GameObject UIPosition;
    //[Space(10f)]

    public int tutorialIndex { get; private set; } = 1;

    private GameObject nextButtonGameObject;
    private GameObject dialogueBoxGameObject;
    private GameObject dialogueTextGameObject;

    private bool buttonPressed = false;
    private float timer = 0f;
    private bool isRotating = false;
    private int currentDialogueIndex = 0;
    
    private GameObject customer;
    
    //손님 음식 주문 관련 마스크
    private GameObject maskCustomer;
    private GameObject maskChef;
    private GameObject maskServer;
    private GameObject maskMoney;

    // 경영창 마스크
    private GameObject maskOne;
    private GameObject maskTwo;

    // UI Position
    RectTransform uiBusinessButton;
    RectTransform uiMoney;
    RectTransform uiMain;
    RectTransform uiProcessBar;

    // ButtonClick
    public Button nextButton;
    
    private bool isBusinessButtonTouch = false;
    private bool isBusinessMachineBuyTouch = false;
    private bool isBusinessCloseTouch = false;
    private bool isProcessBarButtonTouch = false;
    

    public float waitTimeForPushButton = 5f;
    public float waitTimeForStartTutorial = 1f;
    public float rotationDuration = 0.5f;

    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialManager>();
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

    void Start()
    {   
        DarkFilter.SetActive(false);
        close.SetActive(false);
        open.SetActive(false);
        touch.SetActive(false);
        businessFilter.SetActive(false);
        businessBuyPanelFilter.SetActive(false);
        businessCloseFilter.SetActive(false);
        nextButtonGameObject = nextButton.transform.gameObject;
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

        // UI GetComponent
        uiMoney = money.GetComponent<RectTransform>();
        uiBusinessButton = businessButton.GetComponent<RectTransform>();
        uiProcessBar = ProcessBarButton.GetComponent<RectTransform>();
        EnqueueTutorial(1);
    }

    public void EnqueueTutorial(int tutorialId)
    {
        tutorialQueue.Enqueue(tutorialId);
        TryStartNextTutorial();
    }

    private void TryStartNextTutorial()
    {
        if (!isTutorialActive && tutorialQueue.Count > 0)
        {
            isTutorialActive = true;
            int tutorialId = tutorialQueue.Dequeue(); // 대기열에서 튜토리얼 ID 꺼내기
            StartTutorial(tutorialId);
        }
    }

    private void StartTutorial(int tutorialId)
    {
        Debug.Log($"Starting tutorial ID: {tutorialId}");
        // 여기에 튜토리얼 시작 로직 구현, 예를 들어 튜토리얼 별로 다른 함수 호출 등

        // 예시 로직
        switch (tutorialId)
        {
            case 1:
                // 첫 번째 튜토리얼 로직
                StartCoroutine(StartTutorialOne());
                CompleteCurrentTutorial(tutorialId);
                break;
            case 2:
                // 두 번째 튜토리얼 로직
                StartCoroutine(StartTutorialTwo());
                CompleteCurrentTutorial(tutorialId);
                
                break;
            case 3:
                StartCoroutine(StartTutorialThree());
                CompleteCurrentTutorial(tutorialId);

                break;
            case 4:
                StartCoroutine(StartTutorialFour());
                CompleteCurrentTutorial(tutorialId);

                break;


        }
    }

    public void OnButtonClickForNext()
    {
        buttonPressed = true;
    }

    IEnumerator StartTutorialOne()
    {
        yield return new WaitForSeconds(waitTimeForStartTutorial);
        DarkFilter.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[currentDialogueIndex].position;
        Debug.Log(currentDialogueIndex);
        dialogueTextGameObject.SetActive(true);
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());
        DarkFilter.SetActive(false);
        businessFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        while (!isBusinessButtonTouch)
        {
            yield return null;
        }
        isBusinessButtonTouch = false;
        businessFilter.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        dialogueTextGameObject.SetActive(false);
        businessBuyPanelFilter.SetActive(true);
        Debug.Log("check1");
        while (!isBusinessMachineBuyTouch)
        {
            yield return null;
        }
        Debug.Log("check");
        businessBuyPanelFilter.SetActive(false);
        businessCloseFilter.SetActive(true);
        while (!isBusinessCloseTouch)
        {
            yield return null;
        }
        businessCloseFilter.SetActive(false);
        tutorialIndex++;
    }

    IEnumerator StartTutorialTwo()
    {
        yield return new WaitForSeconds(waitTimeForStartTutorial);
        DarkFilter.SetActive (true);
        nextButtonGameObject.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueTextGameObject.SetActive(true);
        dialogueText.text = dialogueDataList[++currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());
        dialogueBoxGameObject.SetActive(false);
        dialogueTextGameObject.SetActive(false);
        //DarkFilter.SetActive(false);
        close.SetActive(true);
        touch.SetActive(true);
        yield return StartCoroutine(PushNextButton());
        touch.SetActive(false);
        nextButton.transform.gameObject.SetActive(false);
        if (!isRotating)
        {
            StartCoroutine(StartRotation(close));
        }
        yield return new WaitForSeconds(waitTimeForStartTutorial);
        close.SetActive(false);
        open.SetActive(true);
        StartCoroutine(StartRotation(open));
        yield return new WaitForSeconds(waitTimeForStartTutorial);
        DarkFilter.SetActive(false);
        open.SetActive(false);
        tutorialIndex++;
    }

    private IEnumerator StartRotation(GameObject signObject)
    {
        isRotating = true;

        float elapsedTime = 0f;
        Transform rotatingObject = signObject.transform;
        Quaternion startRotation = rotatingObject.rotation;
        Quaternion targetRotation = rotatingObject.rotation * Quaternion.Euler(new Vector3(0f, 90f, 0f)); // 회전 각도를 설정합니다.

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationDuration);
            rotatingObject.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            // 회전 중에 다른 동작을 수행할 수도 있습니다.
            // 예를 들어, 화면에 오브젝트를 보이게 설정할 수 있습니다.
            yield return null;
        }
        isRotating = false;
    }

    IEnumerator StartTutorialThree()
    {
        maskCustomer.SetActive(true);
        DarkFilter.SetActive(true);
        while (!OrderManager.Instance.isOrderedForTutorial)
        {
            yield return null;
        }
        maskCustomer.SetActive(false);
        maskChef = Instantiate(prefabMask);
        maskChef.transform.SetParent(chefPosition.transform, false);
        dialogueBoxGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        dialogueTextGameObject.SetActive(true);
        maskChef.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Server serverscript = serverPosition.GetComponent<Server>();
        if (serverscript == null)
        {
            Debug.LogError("Server not found for Tutorial");
        }
        while (!serverscript.isPickupForTutorial)
        {
            yield return null;
        }
        dialogueBoxGameObject.SetActive(false);
        maskChef.SetActive(false);
        maskServer = Instantiate(prefabMask);
        maskServer.transform.SetParent(serverPosition.transform, false);
        maskServer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        while (!serverscript.isServedForTutorial)
        {
            yield return null;
        }
        dialogueBoxGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        maskCustomer.SetActive(true);
        maskServer.SetActive(false);
        yield return new WaitForSeconds(waitTimeForStartTutorial * 3);
        DarkFilter.SetActive(false) ;
        maskCustomer.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        tutorialIndex++;
    }

    public void GetObject(GameObject obj)
    {
        customer = obj;
        maskCustomer = Instantiate(prefabMask);
        maskCustomer.transform.SetParent(customer.transform, false);
        maskCustomer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        maskCustomer.SetActive(false);
    }

    IEnumerator StartTutorialFour()
    {
        // UIMoney focus
        DarkFilter.SetActive(true);
        maskOne = Instantiate(prefabMask);
        maskOne.transform.SetParent(UIPosition.transform, false);
        UIPosition.transform.position = ChangeRectTransformToTransform(uiMoney);
        maskOne.transform.localScale = new Vector3(0.6f, 0.15f, 0.5f);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        dialogueBoxGameObject.SetActive(true);
        yield return StartCoroutine(PushNextButton());

        // UIBusinessButton focus
        Destroy(maskOne);
        maskOne = Instantiate(prefabMask);
        maskOne.transform.SetParent(UIPosition.transform, false);
        UIPosition.transform.position = ChangeRectTransformToTransform(uiBusinessButton);
        maskOne.transform.localScale = new Vector3(0.3f, 0.3f, 0.5f);
        businessButton.SetActive(true);
        touch.SetActive(true);
        touch.transform.position = uiBusinessButton.position + new Vector3(10f, 10f, 0f);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        while (!isBusinessButtonTouch)
        {
            yield return null;
        }



        // UIBusinessButton focus
        tutorialIndex++;
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

    private Vector3 ChangeRectTransformToTransform(RectTransform uiElement)
    {
        // UI 요소의 스크린 좌표를 계산합니다.
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, uiElement.position);

        // 스크린 좌표를 월드 좌표로 변환합니다.
        // Screen Space - Overlay 모드에서는 카메라의 Z 축 위치를 고려할 필요가 없습니다.
        // 여기서 사용되는 Z 값은 카메라에서 월드 공간의 대상 오브젝트까지의 거리를 기준으로 해야 합니다.
        // 2D 게임의 경우, 일반적으로 월드 공간의 대상 오브젝트는 Z 축에서 특정한 위치에 있으므로, 이를 고려하여 설정합니다.
        // 예제에서는 간소화를 위해 0을 사용합니다.
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, Camera.main.nearClipPlane));

        // 결과적으로, Z 축 값은 월드 공간의 대상 오브젝트와 일치하도록 조정해야 할 수 있습니다.
        // 이 예제에서는 2D 게임을 가정하고 Z 값을 0으로 설정합니다.
        return new Vector3(worldPoint.x, worldPoint.y, 0);
    }

    public void BusinessButtonTouch()
    {
        if(tutorialIndex == 4 || tutorialIndex == 1)
        {
            isBusinessButtonTouch = true;
            Debug.Log("BusinessClick");
        }
    }

    public void BusinessMachineUnlock()
    {
        if(tutorialIndex == 1)
        {
            isBusinessMachineBuyTouch = true;
        }
    }

    public void BusinessClose()
    {
        if(tutorialIndex == 1)
        {
            isBusinessCloseTouch = true;
        }
    }

    public void CompleteCurrentTutorial(int n)
    {
        isTutorialActive = false;
        TryStartNextTutorial();
    }
}
