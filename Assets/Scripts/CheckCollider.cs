using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{
    public List<GameObject> colliders;
    RectTransform rect;

    Vector2 startPos;
    Vector2 endPos;

    private void Awake()
    {
        colliders = new List<GameObject>();
        rect = GetComponent<RectTransform>();
        SetCollisionVectors();
    }

    private void Update()
    {
        CheckCollision();
    }

    void CheckCollision()
    {
        colliders = ArtificialGrid.Instance.CheckForCollisions(startPos, endPos);
    }
    
    public GameObject[] CheckCells()
    {
        GameObject[] objs = new GameObject[colliders.Count];
        for (int i = 0; i < colliders.Count; i++)
        {
            objs[i] = colliders[i].gameObject;
        }
        return objs;
    }

    void SetCollisionVectors()
    {
        RectTransform rect = transform.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        startPos = corners[2];
        endPos = corners[0];
    }

    public void UpdateCollisions()
    {
        SetCollisionVectors();
        CheckCollision();
    }
}
