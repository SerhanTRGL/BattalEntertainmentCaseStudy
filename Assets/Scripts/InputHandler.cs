using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ClickType
{
    Single,
    Double
}
public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference click;
    [SerializeField] private float doubleClickTime = 0.25f;

    public static Action<Vector2, ClickType> OnClick;

    private Coroutine clickCoroutine;
    private void Awake() {
        click.action.performed += ClickPerformed;
    }

    private void ClickPerformed(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        if(clickCoroutine != null)
        {
            // Second click arrived in time
            StopCoroutine(clickCoroutine);
            clickCoroutine = null;

            OnClick?.Invoke(screenPosition, ClickType.Double);
        }
        else
        {
            // First click, wait for the second if it comes
            clickCoroutine = StartCoroutine(SingleClickCoroutine(screenPosition));
        }
    }

    private IEnumerator SingleClickCoroutine(Vector2 screenPosition)
    {
        yield return new WaitForSeconds(doubleClickTime);
        
        OnClick?.Invoke(screenPosition, ClickType.Single);
        clickCoroutine = null;
    }

    void OnDestroy()
    {
        click.action.performed -= ClickPerformed;
    }
}
