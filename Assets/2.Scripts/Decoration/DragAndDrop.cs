using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private GameObject objectToDrag;
    private bool isDragging = false;
    private Vector3 offset;
    private float timeToHold = 1.0f; // 1�ʰ� ������ ��
    private float timeHeld = 0.0f; // ���� �ð� ����

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            timeHeld = 0.0f; // �ð� ���� ����
        }

        if (Input.GetMouseButton(0))
        {
            timeHeld += Time.deltaTime; // ���콺�� ������ �ִ� �ð� ����

            if (timeHeld >= timeToHold && !isDragging)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject.tag == "Draggable")
                {
                    isDragging = true;
                    objectToDrag = hit.collider.gameObject;
                    offset = objectToDrag.transform.position - mouseWorldPos;
                    offset.z = 0;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            objectToDrag.transform.position = mouseWorldPos + offset;
        }
    }
}
