using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoubleClickHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference click;
    [SerializeField] private float doubleClickTime = 0.25f;

    public static Action<Vector2> OnDoubleClick;

    private Coroutine clickCoroutine;
    private Vector2 firstClickPosition;

    private void Awake()
    {
        click.action.performed += DoubleClickPerformed;
    }

    private void DoubleClickPerformed(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();

        if (clickCoroutine != null)
        {
            // Second click arrived in time - double click
            StopCoroutine(clickCoroutine);
            clickCoroutine = null;

            OnDoubleClick?.Invoke(firstClickPosition);
        }
        else
        {
            // First click arrived
            firstClickPosition = screenPosition;
            clickCoroutine = StartCoroutine(DoubleClickTimer());
        }
    }

    private IEnumerator DoubleClickTimer()
    {
        yield return new WaitForSeconds(doubleClickTime);

        // Second click did not arrive in time
        clickCoroutine = null;
    }

    private void OnDestroy()
    {
        click.action.performed -= DoubleClickPerformed;
    }
}