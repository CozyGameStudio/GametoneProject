using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Cooldown : MonoBehaviour
{
    private Image cooldownImage;
    private Vector3 offset;
    public RectTransform uiElement; // �̵��� UI ���
    public Canvas canvas; // UI ��Ұ� ���Ե� ĵ����
    public Vector3 setting;
    public float holdTime = 1.0f; // 1�ʰ� ������ ��
    private float timeHeld = 0f;
    private bool isCooldown = false; // ��Ÿ�� ���� ����
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
                timeHeld = 0.0f; // �ð� ���� ����
                StartCooldown(); // ��Ÿ�� ����
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
                    ResetCooldown(); // ��Ÿ�� ����
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
