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

    public void SetSize(Vector2 newSize)
    {
        rect.sizeDelta = newSize;
        SetCollisionVectors();
    }

    public void SetPosition(Vector2 newPos, Vector2 direction) {
        Vector2 finalPos;
        Debug.Log(newPos);

        if (direction == Vector2.up)
        {
            float up = newPos.y + ArtificialGrid.Instance.GetCellSize().y;
            finalPos = new Vector2(newPos.x, up);
        }

        if (direction == Vector2.down)
        {
            float down = newPos.y - ArtificialGrid.Instance.GetCellSize().y;
            Debug.Log(newPos.y);
            Debug.Log(down);
            finalPos = new Vector2(newPos.x, down);
        }

        if (direction == Vector2.right)
        {
            float right = newPos.x + ArtificialGrid.Instance.GetCellSize().x;
            finalPos = new Vector2(right, newPos.y);

        }

        if (direction == Vector2.left)
        {
            float left = newPos.x - ArtificialGrid.Instance.GetCellSize().x;
            finalPos = new Vector2(left, newPos.y);
        }

        rect.position = newPos;
    }

    void SetCollisionVectors()
    {
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
