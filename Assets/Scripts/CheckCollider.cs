using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider
{
    public List<GameObject> colliders; // Things it's colliding with
    RectTransform rect;

    Vector2 startPos; // Upper left corner it will create the check area from
    Vector2 endPos; // Bottom right corner it will create the check area from

    public CheckCollider(RectTransform rect)
    {
        colliders = new List<GameObject>();
        this.rect = rect;
        SetCollisionVectors();
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

    public void SetSize(Vector2 newSize) // Potremmo usare anche solo il numero di celle piuttosto che la grandezza dell'immagine, forse è meglio
    {
        rect.sizeDelta = newSize;
        //Debug.Log(rect.sizeDelta);
        SetCollisionVectors();
    }

    public void SetInitialCheckCollider(Vector2 newSize, Vector2 newPos)
    {
        SetSize(newSize);
        SetInitialPosition(newPos);
    }

    void SetInitialPosition(Vector2 newPos)
    {
        rect.localPosition = newPos;
    }

    public void SetPosition(Vector2 newPos, Vector2 direction) {

        Vector2 finalPos = Vector2.zero;

        //Debug.Log(newPos);

        if (direction == Vector2.down || direction == Vector2.up)
        {
            float yAxis = ArtificialGrid.Instance.GetNextYPosition(newPos.y, direction); // Problema ancora qui! La y ora è troppo corta
            finalPos = new Vector2(newPos.x, yAxis);
        }

        if (direction == Vector2.left || direction == Vector2.right)
        {
            float xAxis = ArtificialGrid.Instance.GetNextXPosition(newPos.x, direction);
            finalPos = new Vector2(xAxis, newPos.y);
        }

        rect.localPosition = finalPos;

        UpdateCollisions();
    }

    public Vector2 GetPosition()
    {
        return rect.localPosition;
    }

    void SetCollisionVectors()
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        startPos = corners[2];
        endPos = corners[0];
    }

    void UpdateCollisions()
    {
        SetCollisionVectors();
        CheckCollision();
    }

    public int GetNumberOfColliders()
    {
        return colliders.Count;
    }

    public bool CheckConditions(int cellsOccupied)
    {
        if (colliders.Count < cellsOccupied)
        {
            return false;
        }

        return true;
    }
}
