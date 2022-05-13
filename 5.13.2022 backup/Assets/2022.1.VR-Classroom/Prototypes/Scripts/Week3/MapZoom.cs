using UnityEngine;
using UnityEngine.InputSystem;

public class UIZoomImage : MonoBehaviour
{
    private Vector3 initialScale;

    [SerializeField]
    private float zoomSpeed = 0.1f;
    [SerializeField]
    private float maxZoom = 10f;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    public void Update()
    {
        float scrollAmount = Mouse.current.scroll.ReadValue().y;
        Vector3 delta = Vector3.one * (scrollAmount * zoomSpeed);
        Vector3 desiredScale = transform.localScale + delta;

        desiredScale = ClampDesiredScale(desiredScale);

        transform.localScale = desiredScale;
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(initialScale, desiredScale);
        desiredScale = Vector3.Min(initialScale * maxZoom, desiredScale);
        return desiredScale;
    }
}