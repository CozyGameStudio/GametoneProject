using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteriorBox : MonoBehaviour, IBox<ScriptableInterior>
{
    ScriptableInterior Interior;

    public Image InteriorImage;

    public void InitBox(ScriptableInterior scriptableInterior)
    {
        Interior = scriptableInterior;
        if(InteriorImage != null)
        {
            InteriorImage.sprite = Interior.icon;
        }
        else
        {
            Debug.LogError("Cannot find Interior Image");
        }
    }

    public void ButtonClick()
    {
        List<Position> positions = InteriorManager.Instance.positions;
        foreach (var obj in positions)
        {
            if (obj.position.positionNumber == Interior.position)
            {
                SpriteRenderer spriteRenderer = obj.gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = Interior.icon;
                }
            }
        }

    }
        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
