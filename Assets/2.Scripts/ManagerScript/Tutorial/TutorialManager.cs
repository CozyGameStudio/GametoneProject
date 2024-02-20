using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using Unity.VisualScripting;

public static class EventDispatcher
{
    public static event Action<int> OnMoneyChanged;
    public static event Action OnMissionCompleted;

    public static void MoneyChanged(int currentMoney)
    {
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public static void MissionCompleted()
    {
        OnMissionCompleted?.Invoke();
        Debug.Log("mission Complete Occured");
    }
}
public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;

    private Queue<int> tutorialQueue = new Queue<int>();
    private bool isTutorialActive = false;
    public GameObject machinePanel;
    public GameObject foodPanel;
    public GameObject stageMissionPanel;

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
    
    private Transform machineBoxUpgradeButtonFilter;
    private Transform foodBoxUpgradeButtonFilter;
    
    public GameObject moneyFilter;
    public GameObject businessPanelFoodButtonFilter;
    public GameObject processBarFilter;
    public GameObject StageMissionScrollviewFilter;
    public GameObject machineFilter;
    
    private Transform StageMissionRewardFilter;
    private Transform stageMissionFilter;
    private Transform stageMissionFilter2;
    public GameObject UIDarkFilter;


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
    
    //�մ� ���� �ֹ� ���� ����ũ
    private GameObject maskCustomer;
    private GameObject maskChef;
    private GameObject maskServer;
    private GameObject maskMoney;

    // �濵â ����ũ
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
    private bool isMissionBoxRewardTouch = false;

    private bool isEnqueueForTutorialOne = false;
    private bool isEnqueueForTutorialFour = false;
    private bool isEnqueueForTutorialFive = false;
    private bool isEnqueueForTutorialSix = false;
    private bool isMissionAllCleared = false;


    public float waitTimeForPushButton = 5f;
    public float waitTimeForTutorial = 1f;
    public float rotationDuration = 0.5f;

    private Coroutine currentCoroutine;
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
    void OnDisable(){
        EventDispatcher.OnMoneyChanged -= CheckMoneyForTutorial;
        EventDispatcher.OnMissionCompleted -= CheckMissionCompletion;
    }
    void Start()
    {
        EventDispatcher.OnMoneyChanged += CheckMoneyForTutorial;
        EventDispatcher.OnMissionCompleted += CheckMissionCompletion;
        if (DarkFilter != null) DarkFilter.SetActive(false);
        if(close!=null)close.SetActive(false);
        if (open != null) open.SetActive(false);
        if (touch != null) touch.SetActive(false);
        if (businessButtonFilter != null) businessButtonFilter.SetActive(false);
        if (businessBoxFilter != null) businessBoxFilter.SetActive(false);
        if (businessCloseFilter != null) businessCloseFilter.SetActive(false);
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

        // UI GetComponent
        uiMoney = money.GetComponent<RectTransform>();
        uiBusinessButton = businessButton.GetComponent<RectTransform>();
        uiProcessBar = ProcessBarButton.GetComponent<RectTransform>();
        //일단 1번 튜토리얼 Enqueue
        EnqueueTutorial(1);
    }
    void CheckMoneyForTutorial(int currentMoney)
    {
        if(!isEnqueueForTutorialOne)return;
        if (!isEnqueueForTutorialFour && currentMoney >= 10)
        {
            EnqueueTutorial(4);
            isEnqueueForTutorialFour = true;
        }
        else if (!isEnqueueForTutorialFive && currentMoney >= 6&& DataManager.Instance.activeMachines[0].currentLevel > 1)
        {
            EnqueueTutorial(5);
            isEnqueueForTutorialFive = true;
            EventDispatcher.OnMoneyChanged -= CheckMoneyForTutorial;
        }
    }

    void CheckMissionCompletion()
    {
        //장비 업그레이드에 대한 체크
        foreach(Machine machine in DataManager.Instance.machines){
            //업그레이드가 완료되면 업그레이드 비용은 0원이기 때문
            Debug.Log(machine.currentUpgradeMoney);
            if (!machine.currentUpgradeMoney.Equals(0))return;
            
        }
        //누적 판매량에 대한 체크
        if(StageMissionManager.Instance.accumulatedSales<150){
            Debug.Log(StageMissionManager.Instance.accumulatedSales);
            return;
        }
        if (!isEnqueueForTutorialSix)
        {
            EnqueueTutorial(6);
            isEnqueueForTutorialSix = true;
            EventDispatcher.OnMissionCompleted -= CheckMissionCompletion;
        }
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
                // ù ��° Ʃ�丮�� ����
                StartCoroutine(StartTutorialOne());
                break;
            case 2:
                StartCoroutine(StartTutorialTwo());
                break;
            case 3:
                StartCoroutine(StartTutorialThree());
                break;
            case 4:
                StartCoroutine(StartTutorialFour());
                break;
            case 5:
                StartCoroutine(StartTutorialFive());
                break;
            case 6:
                StartCoroutine(StartTutorialSix());
                break;
        }
    }

    public void OnButtonClickForNext()
    {
        buttonPressed = true;
    }

    IEnumerator StartTutorialOne()
    {
        
        //이미 해금된 장비가 있다면 1,2번 튜토리얼은 진행하지 않음
        if (DataManager.Instance.activeMachines.Count != 0){
            isEnqueueForTutorialOne=true;
            CompleteCurrentTutorial();
            yield break;
        }
        tutorialIndex = 1;
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
        yield return new WaitUntil(()=>isBusinessButtonTouch);
        isBusinessButtonTouch = false;
        businessButtonFilter.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        dialogueTextGameObject.SetActive(false);
        businessBoxFilter.SetActive(true);
        Debug.Log("check1");
        yield return new WaitUntil(() => isBusinessMachineBuyTouch);
       
        Debug.Log("check");
        businessBoxFilter.SetActive(false);
        businessCloseFilter.SetActive(true);
        yield return new WaitUntil(() => isBusinessCloseTouch);
        isBusinessCloseTouch = false;
        
        businessCloseFilter.SetActive(false);
        machineFilter.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        dialogueTextGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        machineFilter.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        CompleteCurrentTutorial();
        EnqueueTutorial(2);
    }

    IEnumerator StartTutorialTwo()
    {
        tutorialIndex = 2;
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
        isEnqueueForTutorialOne = true;
        CompleteCurrentTutorial();
    }

    private IEnumerator StartRotation(GameObject signObject)
    {
        isRotating = true;

        float elapsedTime = 0f;
        Transform rotatingObject = signObject.transform;
        Quaternion startRotation = rotatingObject.rotation;
        Quaternion targetRotation = rotatingObject.rotation * Quaternion.Euler(new Vector3(0f, 90f, 0f)); // ȸ�� ������ �����մϴ�.

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationDuration);
            rotatingObject.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            // ȸ�� �߿� �ٸ� ������ ������ ���� �ֽ��ϴ�.
            // ���� ���, ȭ�鿡 ������Ʈ�� ���̰� ������ �� �ֽ��ϴ�.
            yield return null;
        }
        isRotating = false;
    }

    IEnumerator StartTutorialThree()
    {
        //3번은 트리거에서 호출됨.
        
        if(!StageMissionManager.Instance.accumulatedCustomer.Equals(0)){
            CompleteCurrentTutorial();
            yield break;
        }
        currentDialogueIndex = 3;
        tutorialIndex = 3;
        maskCustomer.SetActive(true);
        DarkFilter.SetActive(true);
        yield return new WaitUntil(()=> OrderManager.Instance.isOrderedForTutorial);
        maskCustomer.SetActive(false);
        maskChef = Instantiate(prefabMask);
        maskChef.transform.SetParent(chefPosition.transform, false);
        dialogueBoxGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        dialogueTextGameObject.SetActive(true);
        maskChef.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Server serverscript = serverPosition.GetComponentInChildren<Server>();
        if (serverscript == null)
        {
            Debug.LogError("Server not found for Tutorial");
        }
        yield return new WaitUntil(() => serverscript.isPickupForTutorial);
        dialogueBoxGameObject.SetActive(false);
        maskChef.SetActive(false);
        Destroy(maskChef);
        maskServer = Instantiate(prefabMask);
        maskServer.transform.SetParent(serverPosition.transform, false);
        maskServer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        yield return new WaitUntil(() => serverscript.isServedForTutorial);
        dialogueBoxGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        maskCustomer.SetActive(true);
        maskServer.SetActive(false);
        yield return new WaitForSeconds(waitTimeForTutorial * 3);
        DarkFilter.SetActive(false) ;
        maskCustomer.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        CompleteCurrentTutorial();
        
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
        //4번은 장비 업그레이드가 가능할때 진행
        //이미 되어있으면 멈춤
        if(DataManager.Instance.activeMachines[0].currentLevel!=1){
            CompleteCurrentTutorial();
            yield break;
        }
        currentDialogueIndex = 5;
        tutorialIndex = 4;
        // UIMoney focus
        moneyFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueTextGameObject.SetActive(true);
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
        GameObject machineBox = machinePanel.transform.GetChild(0).gameObject;
        machineBoxUpgradeButtonFilter = machineBox.transform.Find("BoxUpgradeButtonFilter");
        machineBoxUpgradeButtonFilter.gameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isMachineUpgradeTouch);

        // UIBusinessBox focus
        machineBoxUpgradeButtonFilter.gameObject.SetActive(false);
        businessBoxFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        // UIBusinessPanelClose focus
        dialogueBoxGameObject.SetActive(false);
        businessBoxFilter.SetActive(false);
        businessCloseFilter.SetActive(true);
        yield return new WaitUntil(() => isBusinessCloseTouch);
        businessCloseFilter.SetActive(false);
        isBusinessCloseTouch = false;
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

    private Vector3 ChangeRectTransformToTransform(RectTransform uiElement)
    {
        // UI ����� ��ũ�� ��ǥ�� ����մϴ�.
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, uiElement.position);

        // ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ�մϴ�.
        // Screen Space - Overlay ��忡���� ī�޶��� Z �� ��ġ�� ������ �ʿ䰡 �����ϴ�.
        // ���⼭ ���Ǵ� Z ���� ī�޶󿡼� ���� ������ ��� ������Ʈ������ �Ÿ��� �������� �ؾ� �մϴ�.
        // 2D ������ ���, �Ϲ������� ���� ������ ��� ������Ʈ�� Z �࿡�� Ư���� ��ġ�� �����Ƿ�, �̸� �����Ͽ� �����մϴ�.
        // ���������� ����ȭ�� ���� 0�� ����մϴ�.
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, Camera.main.nearClipPlane));

        // ���������, Z �� ���� ���� ������ ��� ������Ʈ�� ��ġ�ϵ��� �����ؾ� �� �� �ֽ��ϴ�.
        // �� ���������� 2D ������ �����ϰ� Z ���� 0���� �����մϴ�.
        return new Vector3(worldPoint.x, worldPoint.y, 0);
    }

    IEnumerator StartTutorialFive()
    {
        if (DataManager.Instance.activeFoods[0].currentLevel != 1)
        {
            CompleteCurrentTutorial();
            yield break;
        }
        currentDialogueIndex=11;
        tutorialIndex = 5;
        // UIMoney focus
        moneyFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueTextGameObject.SetActive(true);
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        dialogueBoxGameObject.SetActive(true);
        yield return StartCoroutine(PushNextButton());

        // UIBusinessButton focus
        moneyFilter.SetActive(false);
        businessButtonFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isBusinessButtonTouch);
        isBusinessButtonTouch = false;

        // UIBusinessPanelFoodButoon focus
        businessButtonFilter.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        businessPanelFoodButtonFilter.SetActive(true);
        yield return new WaitUntil(() => isBusinessPanelFoodTouch);
        isBusinessButtonTouch = false;

        // UIBusinessPanel focus
        businessPanelFoodButtonFilter.SetActive(false);
        businessPanelFilter.SetActive(true);
        yield return new WaitForSeconds(waitTimeForTutorial);

        // UIMachineBox focus
        businessPanelFilter.SetActive(false);
        //businessBoxFilter.SetActive(true);
        businessBoxFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        // UIFoodBoxUpgradeButton focus
        businessPanelFilter.SetActive(false);
        businessBoxFilter.SetActive(false);
        GameObject foodBox = foodPanel.transform.GetChild(0).gameObject;
        foodBoxUpgradeButtonFilter = foodBox.transform.Find("BoxUpgradeButtonFilter");
        foodBoxUpgradeButtonFilter.gameObject.SetActive(true);
        yield return new WaitUntil(() =>isFoodUpgradeTouch);

        // UIMachineBox focus
        foodBoxUpgradeButtonFilter.gameObject.SetActive(false);
        businessBoxFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        // UIBusinessPanelClose focus
        dialogueBoxGameObject.SetActive(false);
        businessBoxFilter.SetActive(false);
        businessCloseFilter.SetActive(true);
        yield return new WaitUntil(() =>isBusinessCloseTouch);
        businessCloseFilter.SetActive(false);
        isBusinessCloseTouch = false;
        CompleteCurrentTutorial();
    }

    IEnumerator StartTutorialSix()
    {
        tutorialIndex=6;
        currentDialogueIndex = 15;
        ProcessBarButton.GetComponent<RectTransform>().DOAnchorPos(new Vector2(83, -50), 2f);
        // UIDarkFilter
        UIDarkFilter.SetActive(false);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        Debug.Log(dialogueDataList[currentDialogueIndex]);
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        dialogueTextGameObject.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        yield return StartCoroutine(PushNextButton());

        // UIProcessBar focus
        processBarFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        // UIProcessBar Touch
        dialogueBoxGameObject.SetActive(false);
        yield return new WaitUntil(() => isProcessBarButtonTouch);

        // UIStageMissionScrollview Touch
        processBarFilter.SetActive(false);
        StageMissionScrollviewFilter.SetActive(true);
        dialogueBoxGameObject.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        // UIMissionRewardButton Touch
        StageMissionScrollviewFilter.SetActive(false);
        GameObject missionBox = stageMissionPanel.transform.GetChild(1).gameObject;
        StageMissionRewardFilter = missionBox.transform.Find("StageMissionRewardFilter");
        StageMissionRewardFilter.gameObject.SetActive(true);
        
        missionBox = stageMissionPanel.transform.GetChild(2).gameObject;
        stageMissionFilter = missionBox.transform.Find("StageMissionFilter");
        stageMissionFilter.gameObject.SetActive(true);

        missionBox = stageMissionPanel.transform.GetChild(3).gameObject;
        stageMissionFilter2 = missionBox.transform.Find("StageMissionFilter");
        stageMissionFilter2.gameObject.SetActive(true);

        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitUntil(() => isMissionBoxRewardTouch);

        // UIProcessBar Touch
        StageMissionRewardFilter.gameObject.SetActive(false);
        stageMissionFilter.gameObject.SetActive(false);
        stageMissionFilter2.gameObject.SetActive(false);

        processBarFilter.SetActive(true);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return StartCoroutine(PushNextButton());

        processBarFilter.SetActive(false);
        dialogueBoxGameObject.SetActive(false);
        yield return new WaitUntil(() => StageMissionManager.Instance.stageProgress==100);
        dialogueBoxGameObject.transform.position = dialogueBoxPositionDataList[++currentDialogueIndex].position;
        dialogueText.text = dialogueDataList[currentDialogueIndex];
        yield return new WaitForSeconds(1);
        StageMissionManager.Instance.TriggerStageCleared();
        CompleteCurrentTutorial();
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
        if (tutorialIndex == 1)
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

    public void ProcessBar()
    {
        if (tutorialIndex == 6)
        {
            isProcessBarButtonTouch = true;
        }
    }

    public void MissionBoxReward()
    {
        if (tutorialIndex == 6)
        {
            isMissionBoxRewardTouch = true;
        }
    }

    public void CompleteCurrentTutorial()
    {
        isTutorialActive = false;
        TryStartNextTutorial();
    }
}
