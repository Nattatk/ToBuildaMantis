using UnityEngine;

public class RotationPhysics : MonoBehaviour
{
    public float gravityForce = 9.81f; // Strength of gravity in the +Z direction
    public float maxRotationAngle = 45f; // Maximum rotation angle in degrees
    private Rigidbody rb;

    void Start()
    {
        // Get or add a Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false; // Disable Unity's default gravity
    }

    void FixedUpdate()
    {
        // Apply gravity in the positive Z direction
        Vector3 force = new Vector3(0, 0, -gravityForce);
        rb.AddForce(force, ForceMode.Acceleration);

        // Constrain the rotation
        LimitRotation();
    }

    void LimitRotation()
    {
        // Get the current rotation in Euler angles
        Vector3 rotation = transform.eulerAngles;

        // Convert to -180 to 180 range for easier clamping
        rotation.x = NormalizeAngle(rotation.x);
        rotation.y = NormalizeAngle(rotation.y);

        // Clamp rotation to within ±45 degrees for x and y
        rotation.x = Mathf.Clamp(rotation.x, -maxRotationAngle, maxRotationAngle);
        rotation.y = Mathf.Clamp(rotation.y, -maxRotationAngle, maxRotationAngle);

        // Apply the constrained rotation
        transform.eulerAngles = rotation;
    }

    float NormalizeAngle(float angle)
    {
        // Normalize an angle to the range -180 to 180
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}
