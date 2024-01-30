using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Cooldown : MonoBehaviour
{
    private Image cooldownImage;
    private Vector3 offset;
    public RectTransform uiElement; // 이동할 UI 요소
    public Canvas canvas; // UI 요소가 포함된 캔버스
    public Vector3 setting;
    public float holdTime = 1.0f; // 1초간 눌러야 함
    private float timeHeld = 0f;
    private bool isCooldown = false; // 쿨타임 상태 추적
    public bool isBox = false;
    private void Start()
    {
        cooldownImage = GetComponent<Image>();
    }

    void Update()
    {
        if (isBox)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition + setting, canvas.worldCamera, out position);
            uiElement.localPosition = position;
            if (Input.GetMouseButtonDown(0))
            {
                timeHeld = 0.0f; // 시간 추적 시작
                StartCooldown(); // 쿨타임 시작
                isCooldown = true;
            }
            if (Input.GetMouseButton(0) && !isCooldown)
            {
                timeHeld += Time.deltaTime;


            }
            if (Input.GetMouseButtonUp(0))
            {
                if (isCooldown)
                {
                    ResetCooldown(); // 쿨타임 리셋
                }
            }

            if (isCooldown)
            {
                UpdateCooldown();
            }
        }
    }
    private void StartCooldown()
    {
        cooldownImage.fillAmount = 0;
    }

    private void UpdateCooldown()
    {
        cooldownImage.fillAmount += 1 / holdTime * Time.deltaTime;

        if (cooldownImage.fillAmount >= 1)
        {
            ResetCooldown() ;
        }
    }

    private void ResetCooldown()
    {
        cooldownImage.fillAmount = 0;
        isCooldown = false;
    }
    public void DecorateExit()
    {
        isBox = true;
    }
}
