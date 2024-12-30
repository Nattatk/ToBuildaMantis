using UnityEngine;

public class MousePositionManager : MonoBehaviour
{
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane; // Adjust depth if needed
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
