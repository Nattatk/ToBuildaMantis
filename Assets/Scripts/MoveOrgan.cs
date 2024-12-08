using UnityEngine;
using UnityEngine.InputSystem;

public class MoveOrgan : MonoBehaviour
{

    public bool isHolding { get; set; }
    public InputActionReference click;

    private Vector3 mousePosition;
    private Transform pickedUpTransform;
    private float originalZ;


    void Start()
    {
        pickedUpTransform = null;
    }

    private void OnEnable()
    {
        click.action.Enable();
        click.action.started += OnLeftMouseDown;
        click.action.canceled += OnLeftMouseRelease;
    }

    private void OnDisable()
    {
        //Unsubscribe to avoid memory leaks
        click.action.started -= OnLeftMouseDown;
        click.action.canceled -= OnLeftMouseRelease;
        click.action.Disable();
    }

    private void OnLeftMouseDown(InputAction.CallbackContext context)
    {
        Debug.Log("Mouse pressed and holding");
        OnHoldStart();
    }

    private void OnLeftMouseRelease(InputAction.CallbackContext context)
    {
        
        Debug.Log("Mouse released");
        OnHoldEnd();
    }

    void Update()
    {
        // Get the mouse position in world space
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure z-coordinate matches your 2D plane

        // While holding the mouse, move the picked-up object
        if (isHolding && pickedUpTransform != null)
        {
            pickedUpTransform.position = new Vector3(mousePosition.x, mousePosition.y, -1);
        }

        
    }

    private void OnHoldStart()
    {
        // Run once when button is pressed

        // Check for raycast hits
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            
                // Try to pick up the object
                Organ organInfo = hit.collider.GetComponent<Organ>();
                if (organInfo != null)
                {
                    isHolding = true;
                    pickedUpTransform = hit.collider.transform;

                    // Store the original Z position
                    originalZ = pickedUpTransform.position.z;

                    // Move the object closer to the camera
                    pickedUpTransform.position = new Vector3(
                        pickedUpTransform.position.x,
                        pickedUpTransform.position.y,
                        -1 // Or any "picked up" z-value closer to the camera
                    );

                    Debug.Log("Picked up the organ: " + organInfo.name);
                }
            
        }
    }

    private void DuringHold()
    {

    }

    private void OnHoldEnd()
    {
        
        
            isHolding = false;

            // Reset the Z position to the original
            if (pickedUpTransform != null)
            {
                pickedUpTransform.position = new Vector3(
                    pickedUpTransform.position.x,
                    pickedUpTransform.position.y,
                    originalZ
                );

                Debug.Log("Dropped the organ.");
                pickedUpTransform = null; // Clear the reference
            }
        
    }
}
