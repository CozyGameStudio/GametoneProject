using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BusinessPanel : MonoBehaviour
{
    [Header("버튼 목록")]
    public Button machineButton;
    public Button foodButton;
    public Button characterButton;
    [Header("패널 목록")]
    public GameObject machinePanel;
    public GameObject foodPanel;
    public GameObject characterPanel;
    [Header("스프라이트")]
    public Sprite activeMachineSprite;
    public Sprite activeFoodSprite;
    public Sprite activeCharacterSprite;
    public Sprite inActiveMachineSprite;
    public Sprite inActiveFoodSprite;
    public Sprite inActiveCharacterSprite;

    private Dictionary<Button, GameObject> buttonPanelMap;
    private Dictionary<Button, Sprite> buttonActiveSpriteMap;
    private Dictionary<Button, Sprite> buttonInActiveSpriteMap;
    private List<Button> buttons;
    void Start(){
        buttonPanelMap = new Dictionary<Button, GameObject>
        {
            { machineButton, machinePanel },
            { foodButton, foodPanel },
            { characterButton, characterPanel }
        };

        buttonActiveSpriteMap = new Dictionary<Button, Sprite>
        {
            { machineButton, activeMachineSprite },
            { foodButton, activeFoodSprite },
            { characterButton, activeCharacterSprite }
        };
        buttonInActiveSpriteMap = new Dictionary<Button, Sprite>
        {
            { machineButton, inActiveMachineSprite },
            { foodButton, inActiveFoodSprite },
            { characterButton, inActiveCharacterSprite }
        };

        buttons = new List<Button> { machineButton, foodButton, characterButton };

        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClicked(button));
        }
    }
    void OnEnable(){
        OnButtonClicked(machineButton);
    }
    void OnButtonClicked(Button clickedButton)
    {
        foreach (var pair in buttonPanelMap)
        {
            pair.Value.SetActive(false); 
        }
        if (buttonPanelMap.TryGetValue(clickedButton, out var activePanel))
        {
            activePanel.SetActive(true); 
        }

        foreach (var button in buttons)
        {
            if (button == clickedButton)
            {
                if (button != null) button.GetComponent<Image>().sprite = buttonActiveSpriteMap[button]; // 활성화된 스프라이트로 변경
            }
            else
            {
                if(button!=null)button.GetComponent<Image>().sprite = buttonInActiveSpriteMap[button]; // 다른 모든 버튼을 비활성화된 스프라이트로 변경
            }
        }
    }
}
