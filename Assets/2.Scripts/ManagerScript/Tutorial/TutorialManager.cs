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
using JetBrains.Annotations;

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
    public GameObject businessButtonFilter;
    public GameObject businessBoxFilter;
    public GameObject businessCloseFilter;
    public GameObject businessPanelFilter;
    public GameObject machineBoxUpgradeCostFilter;
    public GameObject BoxUpgradeButtonFilter;
    public GameObject moneyFilter;
    public GameObject businessPanelFoodButtonFilter;
    public GameObject processBarFilter;

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

    public int tutorialIndex { get; private set; } = 0;

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
    private bool isMachineUpgradeTouch = false;
    private bool isFoodUpgradeTouch = false;
    private bool isBusinessPanelFoodTouch = false;

    private bool isEnqueueForTutorialOne = false;
    private bool isEnqueueForTutorialFour = false;
    private bool isEnqueueForTutorialFive = false;
    private bool isEnqueueForTutorialSix = false;
    private bool isMissionAllCleared = true;


    public float waitTimeForPushButton = 5f;
    public float waitTimeForTutorial = 1f;
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
        businessButtonFilter.SetActive(false);
        businessBoxFilter.SetActive(false);
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
        StartCoroutine(IsEnqueueTutorial());
        EnqueueTutorial(1);
    }

    IEnumerator IsEnqueueTutorial()
    {
        
        while (true)
        {
            if (BusinessGameManager.Instance.money >= 20 && !isEnqueueForTutorialFour && tutorialIndex >= 3)
            {
                isEnqueueForTutorialFour = true;
                EnqueueTutorial(4);
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        while(true)
        {
            if (BusinessGameManager.Instance.money >= 10 && !isEnqueueForTutorialFive && tutorialIndex >= 4)
            {
                isEnqueueForTutorialFive = true;
                EnqueueTutorial(5);
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        while (true)
        {
            foreach(var mission in StageMissionManager.Instance.missions)
            {
                if (!mission.isCleared)
                {
                    isMissionAllCleared = false;
                    break;
                }
            }
            if (isMissionAllCleared && !isEnqueueForTutorialSix && tutorialIndex >= 5)
            {
                isEnqueueForTutorialFive = true;
                EnqueueTutorial(6);
                break;
            }
            yield return null;
        }
    }

    public void EnqueueTutorial(int tutorialId)
    {
        Debug.Log("인큐" + tutorialId);
        tutorialQueue.Enqueue(tutorialId);
        TryStartNextTutorial();
    }

    private void TryStartNextTutorial()
    {
        if (!isTutorialActive && tutorialQueue.Count > 0)
        {
            isTutorialActive = true;
            Debug.Log("큐" + tutorialQueue.Count);
            int tutorialId = tutorialQueue.Dequeue(); // 대기열에서 튜토리얼 ID 꺼내기
            StartCoroutine(StartTutorial(tutorialId));
        }
    }

    IEnumerator StartTutorial(int tutorialId)
    {
        Debug.Log($"Starting tutorial ID: {tutorialId}");
        // 여기에 튜토리얼 시작 로직 구현, 예를 들어 튜토리얼 별로 다른 함수 호출 등
        
        // 예시 로직
        switch (tutorialId)
        {
            case 1:
                // 첫 번째 튜토리얼 로직
                yield return StartCoroutine(StartTutorialOne());
                CompleteCurrentTutorial(tutorialId);
                break;
            case 2:
                // 두 번째 튜토리얼 로직
                yield return StartCoroutine(StartTutorialTwo());
                CompleteCurrentTutorial(tutorialId);
                break;
            case 3:
                yield return StartCoroutine(StartTutorialThree());
                CompleteCurrentTutorial(tutorialId);
                break;
            case 4:
                yield return StartCoroutine(StartTutorialFour());
                CompleteCurrentTutorial(tutorialId);
                break;
            case 5:
                yield return StartCoroutine(StartTutorialFive());
                CompleteCurrentTutorial(tutorialId);
                break;
            case 6:
                yield return StartCoroutine(StartTutorialSix());
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
        tutorialIndex++;
        BusinessGameManager.Instance.ChangeSpeed(0.1f);
        waitTimeForPushButton *= 0.1f;
        waitTimeForTutorial *= 0.1f;
        rotationDuration *= 0.1f;
        yield return new WaitForSeconds(waitTimeForTutorial);
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
        while (!isBusinessButtonTouch)
        {
            yield return null;
        }
        isBusinessButtonTouch = false;
        businessButtonFilter.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        dialogueTextGameObject.SetActive(false);
        businessBoxFilter.SetActive(true);
        Debug.Log("check1");
        while (!isBusinessMachineBuyTouch)
        {
            yield return null;
        }
        Debug.Log("check");
        businessBoxFilter.SetActive(false);
        businessCloseFilter.SetActive(true);
        while (!isBusinessCloseTouch)
        {
            yield return null;
        }
        isBusinessCloseTouch = false;
        businessCloseFilter.SetActive(false);
        EnqueueTutorial(tutorialIndex + 1);
    }

    IEnumerator StartTutorialTwo()
    {
        tutorialIndex++;
        yield return new WaitForSeconds(waitTimeForTutorial);
        DarkFilter.SetActive (true);
        nextButtonGameObject.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        Debug.Log(currentDialogueIndex);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueTextGameObject.SetActive(true);
        dialogueText.text = dialogueDataList[currentDialogueIndex];
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
        yield return new WaitForSeconds(waitTimeForTutorial);
        close.SetActive(false);
        open.SetActive(true);
        StartCoroutine(StartRotation(open));
        yield return new WaitForSeconds(waitTimeForTutorial);
        DarkFilter.SetActive(false);
        open.SetActive(false);

        BusinessGameManager.Instance.ChangeSpeed(10f);
        waitTimeForPushButton *= 10f;
        waitTimeForTutorial *= 10f;
        rotationDuration *= 10f;
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
        tutorialIndex++;
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
        Destroy(maskChef);
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
        yield return new WaitForSeconds(waitTimeForTutorial * 3);
        DarkFilter.SetActive(false) ;
        maskCustomer.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
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
        tutorialIndex++;
        // UIMoney focus
        moneyFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        dialogueBoxGameObject.SetActive(true);
        yield return StartCoroutine(PushNextButton());

        // UIBusinessButton focus
        moneyFilter.SetActive(false);
        businessButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        while (!isBusinessButtonTouch)
        {
            yield return null;
        }
        isBusinessButtonTouch = false;

        // UIBusinessPanel focus
        businessButtonFilter.SetActive(false);
        businessPanelFilter.SetActive(true);
        yield return new WaitForSeconds(waitTimeForTutorial);

        // UIMachineBox focus
        businessPanelFilter.SetActive(false);
        businessBoxFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        // UIMachineBoxUpgradeCost focus
        businessBoxFilter.SetActive(false);
        machineBoxUpgradeCostFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        // UIMachineBoxUpgradeButton focus
        machineBoxUpgradeCostFilter.SetActive(false);
        BoxUpgradeButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        while (!isMachineUpgradeTouch)
        {
            yield return null;
        }

        // UIBusinessBox focus
        BoxUpgradeButtonFilter.SetActive(false);
        businessBoxFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        // UIBusinessPanelClose focus
        dialogueBoxGameObject.SetActive(false);
        businessBoxFilter.SetActive(false);
        businessCloseFilter.SetActive(true);
        while (!isBusinessCloseTouch)
        {
            yield return null;
        }
        businessCloseFilter.SetActive(false);
        isBusinessCloseTouch = false;
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

    IEnumerator StartTutorialFive()
    {
        tutorialIndex++;
        // UIMoney focus
        moneyFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        dialogueBoxGameObject.SetActive(true);
        yield return StartCoroutine(PushNextButton());

        // UIBusinessButton focus
        moneyFilter.SetActive(false);
        businessButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        while (!isBusinessButtonTouch)
        {
            yield return null;
        }
        isBusinessButtonTouch = false;

        // UIBusinessPanelFoodButoon focus
        businessButtonFilter.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        businessPanelFoodButtonFilter.SetActive(true);
        while (!isBusinessPanelFoodTouch)
        {
            yield return null;
        }
        isBusinessButtonTouch = false;

        // UIBusinessPanel focus
        businessPanelFoodButtonFilter.SetActive(false);
        businessPanelFilter.SetActive(true);
        yield return new WaitForSeconds(waitTimeForTutorial);

        // UIMachineBox focus
        businessPanelFilter.SetActive(false);
        businessBoxFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        // UIFoodBoxUpgradeButton focus
        businessBoxFilter.SetActive(false);
        BoxUpgradeButtonFilter.SetActive(true);
        while (!isFoodUpgradeTouch)
        {
            yield return null;
        }

        // UIMachineBox focus
        BoxUpgradeButtonFilter.SetActive(false);
        businessBoxFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        // UIBusinessPanelClose focus
        dialogueBoxGameObject.SetActive(false);
        businessBoxFilter.SetActive(false);
        businessCloseFilter.SetActive(true);
        while (!isBusinessCloseTouch)
        {
            yield return null;
        }
        businessCloseFilter.SetActive(false);
        isBusinessCloseTouch = false;
        isTutorialActive = false;
    }

    IEnumerator StartTutorialSix()
    {
        yield return null;

        isTutorialActive = false;
    }


    public void BusinessButtonTouch()
    {
        if(tutorialIndex == 4 || tutorialIndex == 1 || tutorialIndex == 5)
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
        if(tutorialIndex == 1 || tutorialIndex == 4 || tutorialIndex == 5)
        {
            isBusinessCloseTouch = true;
        }
    }

    public void MachineUpgrade()
    {
        if (tutorialIndex == 4)
        {
            isMachineUpgradeTouch = true;
        }
    }

    public void FoodUpgrade()
    {
        if (tutorialIndex == 5)
        {
            isFoodUpgradeTouch = true;
        }
    }

    public void BusinessPanelFood()
    {
        if (tutorialIndex == 5)
        {
            isBusinessPanelFoodTouch = true;
        }
    }

    public void CompleteCurrentTutorial(int n)
    {
        isTutorialActive = false;
        TryStartNextTutorial();
    }
}
