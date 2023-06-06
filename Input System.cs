using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// This code is moving the camera while player scrolling in both direction.

public class CsFileName : MonoBehaviour
{
    [SerializeField] private InputAction buttonPressAction;
    [SerializeField] private InputAction curScreenPos;
    [SerializeField] private InputAction delta;

    private Vector2 firstPos;
    private Vector2 secondPos;

    private bool isDragging = false;
    public float moveSpeed = 1f;

    void Awake() // Enable input listener.
    {

        buttonPressAction.Enable();
        curScreenPos.Enable();
        delta.Enable();

        buttonPressAction.started += context => OnButtonPress();
        buttonPressAction.canceled += context => OnButtonRelease();
    }
    void Update()
    {
        if (isDragging) // if player is scrolling.
        {
            Vector2 deltaP = delta.ReadValue<Vector2>();
            float deltaX = deltaP.x;
            float deltaY = deltaP.y;
            float cameraX = transform.position.x;
            float cameraZ = transform.position.z;

            // Camera limits.
            float minX = 1f;
            float maxX = 19f;
            float minZ = -2f;
            float maxZ = 13f;

            // New Camera positions
            float newCameraX = cameraX - deltaX * moveSpeed * Time.deltaTime;
            float newCameraZ = cameraZ - deltaY * moveSpeed * Time.deltaTime;

            // VCheck if camera if out of limits.
            if (newCameraX < minX)
                newCameraX = minX;
            else if (newCameraX > maxX)
                newCameraX = maxX;

            if (newCameraZ < minZ)
                newCameraZ = minZ;
            else if (newCameraZ > maxZ)
                newCameraZ = maxZ;
            
            // Update Camera Position.
            transform.position = new Vector3(newCameraX, transform.position.y, newCameraZ);
        }
    }

    private void OnDestroy() //Disable input listener.
    {
        buttonPressAction.Disable();
        curScreenPos.Disable();
        delta.Disable();
    }


    private void OnButtonPress() // Check For the click.
    {
        firstPos = curScreenPos.ReadValue<Vector2>();
        isDragging = true;
    }

    private void OnButtonRelease() // Check for the realease.
    {
        isDragging = false;
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            secondPos = curScreenPos.ReadValue<Vector2>();


            Vector2 diff = secondPos - firstPos;
            if (diff.magnitude < 0.5f)
            {
                //Make an action on click only.
            }
        }
    }