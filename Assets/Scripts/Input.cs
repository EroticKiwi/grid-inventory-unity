using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Input : MonoBehaviour
{
    public CellManager _cellManager;
    Controls _controls;
    
    bool hasPressed_horizontal = false;
    bool hasPressed_vertical = false;
    bool hasPressed_selected = false;
    bool hasPressed_cancel = false;

    private void Start()
    {
        _controls = new Controls();
        _controls.Enable();
    }

    private void Update()
    {
        ListenForInput();
    }

    void ListenForInput()
    {
        MoveHorizontal(_controls.Movement.Horizontal.ReadValue<float>());
        MoveVertical(_controls.Movement.Vertical.ReadValue<float>());
        Select(_controls.Movement.Select.ReadValue<float>());
        Cancel(_controls.Movement.Cancel.ReadValue<float>());
    }

    void MoveHorizontal(float direction)
    {
        if (direction == 0)
        {
            hasPressed_horizontal = false;
            return;
        }

        if (direction == 1 && !hasPressed_horizontal)
        {
            _cellManager.NextCell(Vector2.right);
            hasPressed_horizontal = true;
            return;
        }

        if (direction == -1 && !hasPressed_horizontal)
        {
            _cellManager.NextCell(Vector2.left);
            hasPressed_horizontal = true;
        }
    }

    void MoveVertical(float direction)
    {
        if (direction == 0)
        {
            hasPressed_vertical = false;
            return;
        }

        if (direction == 1 && !hasPressed_vertical)
        {
            _cellManager.NextCell(Vector2.up);
            hasPressed_vertical = true;
            return;
        }

        if (direction == -1 && !hasPressed_vertical)
        {
            _cellManager.NextCell(Vector2.down);
            hasPressed_vertical = true;
        }
    }

    void Select(float pressed)
    {
        if (pressed == 0)
        {
            hasPressed_selected = false;
            return;
        }

        if (pressed == 1 && !hasPressed_selected)
        {
            _cellManager.SelectCells_Input();
            hasPressed_selected = true;
        }
    }

    void Cancel(float pressed)
    {
        if (pressed == 0)
        {
            hasPressed_cancel = false;
            return;
        }

        if (pressed == 1 && !hasPressed_cancel)
        {
            _cellManager.Cancel_Input();
            hasPressed_cancel = true;
        }
    }
}