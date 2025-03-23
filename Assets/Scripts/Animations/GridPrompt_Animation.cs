using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPrompt_Animation : MonoBehaviour
{
    bool isFull = false;

    RectTransform rect;
    float targetWidth;
    float targetHeigth;
    public float inflationSpeed = 1000f;

    GameObject child;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        targetWidth = 600f;
        targetHeigth = 200f;
        child = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (isFull)
        {
            child.SetActive(true);
            return;
        }
        Inflate();
    }

    void Inflate()
    {
        if (rect.sizeDelta.x >= targetWidth && rect.sizeDelta.y >= targetHeigth)
        {
            rect.sizeDelta = new Vector2(targetWidth, targetHeigth);
            isFull = true;
        }

        if (rect.sizeDelta.x < targetWidth)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x + inflationSpeed * Time.deltaTime, rect.sizeDelta.y);
        } else
        {
            rect.sizeDelta = new Vector2(targetWidth, rect.sizeDelta.y);
        }

        if (rect.sizeDelta.y < targetHeigth)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + inflationSpeed * Time.deltaTime);
        } else
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, targetHeigth);
        }
    }

    public void ClosePrompt()
    {
        Deflate();
    }

    void Deflate()
    {
        if (rect.sizeDelta.x <= 0 && rect.sizeDelta.y <= 0)
        {
            rect.sizeDelta = new Vector2(0f, 0f);
            isFull = false;
        }

        if (rect.sizeDelta.x > 0)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x - inflationSpeed * Time.deltaTime, rect.sizeDelta.y);
        }
        else
        {
            rect.sizeDelta = new Vector2(0f, rect.sizeDelta.y);
        }

        if (rect.sizeDelta.y > 0)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y - inflationSpeed * Time.deltaTime);
        }
        else
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0f);
        }
    }
}
