using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CamFollow : MonoBehaviour
{ 
    public Transform treeTop;
    Vector3 offset = new Vector3(0, -6, -10);

    // mouse
    bool isDraggingMouse = false;
    Vector3 mouseStartWorld;

    // touch
    bool isPinching = false;
    Vector3 touchStartWorld;
    float prevTouchDist = 0f;

    float timer = 0f;

    public CanvasGroup mainMenuGroup;
    public CanvasGroup interactiveUIGroup;
    public CanvasGroup panUIGroup;
    public GameObject dots;

    public float panMultiplier = 1.5f;
    public float zoomSpeed = 0.01f;
    public float zoomMin = 5f;
    public float zoomMax = 20f;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleMouse();
        HandleTouch();
        AutoReturn();
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mouseStartWorld = GetWorld(Input.mousePosition);
            isDraggingMouse = true;
            timer = 0;
            ShowMainMenu(false);
        }

        if (Input.GetMouseButtonUp(1))
        {
            isDraggingMouse = false;
            ShowMainMenu(true);
        }

        if (isDraggingMouse)
        {
            Vector3 currentWorld = GetWorld(Input.mousePosition);
            Vector3 delta = currentWorld - mouseStartWorld;
            transform.position -= delta;
        }
    }

    void HandleTouch()
    {
        // Only allow exactly 2 touches
        if (Input.touchCount != 2)
        {
            isPinching = false;
            return;
        }

        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        Vector2 mid = (t0.position + t1.position) * 0.5f;
        Vector3 currentWorld = GetWorld(mid);

        float currentDist = (t0.position - t1.position).magnitude;

        // Reset baseline on first valid frame OR when touches begin
        if (!isPinching || t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
        {
            touchStartWorld = currentWorld;
            prevTouchDist = currentDist;
            isPinching = true;

            timer = 0;
            ShowMainMenu(false);
            return;
        }

        // Pan
        Vector3 delta = currentWorld - touchStartWorld;
        transform.position -= delta * panMultiplier;
        touchStartWorld = currentWorld;

        // Zoom
        float distDelta = currentDist - prevTouchDist;
        cam.orthographicSize -= distDelta * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, zoomMin, zoomMax);
        prevTouchDist = currentDist;
    }

    Vector3 GetWorld(Vector2 screenPos)
    {
        float z = Mathf.Abs(cam.transform.position.z);
        return cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));
    }

    void AutoReturn()
    {
        if (!isDraggingMouse && !isPinching)
        {
            timer += Time.deltaTime;

            if (timer > 2f)
            {
                Vector3 target = treeTop.position + offset;
                transform.position -= (transform.position - target) / 1.1f * Time.deltaTime;
            }
        }
    }

    void ShowMainMenu(bool _default)
    {
        if (_default)
        {
            mainMenuGroup.alpha = 1;
            interactiveUIGroup.alpha = 1;
            panUIGroup.alpha = 0;
            dots.SetActive(false);
        }
        else
        {
            mainMenuGroup.alpha = 0;
            interactiveUIGroup.alpha = 0;
            panUIGroup.alpha = 0.1f;
            dots.SetActive(true);
        }
    }
}