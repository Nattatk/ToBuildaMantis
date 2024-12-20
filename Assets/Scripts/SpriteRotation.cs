using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    /*In Unity I want a script to keep track of the velocity of the object. If the velocity reaches a certain amount in the X direction,
    then in the animator controller play animation clip #1. If the velocity is a certain amount in the -X direction, play clip #2;
    if the velocity is a certain amount in the Y direction, play clip #3; if the velocity is a certain amount in the -Y direction,
    then play clip #4. If the velocity is zero or close to zero then return to original animation state.
    Additionally, if the velocity is high in both X and Y direction, then play clip #5; high in -X and Y play clip #6;
    high in -X and -Y play clip #7; high in X and -Y play clip #8*/

    public Rigidbody2D rb; // Reference to the Rigidbody2D component
    public Animator animator; // Reference to the Animator component
    public float velocityThreshold = 1.0f; // Threshold for "high velocity"
    public float zeroThreshold = 0.1f; // Threshold to consider velocity as zero
    private Vector3 lastPosition;
    private Vector2 velocity;

    private void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }
    private void Update()
    {
        Vector3 currentPosition = transform.position;
        velocity = (currentPosition - lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;

        // Check for close-to-zero velocity
        if (velocity.magnitude < zeroThreshold)
        {
            animator.Play("Center"); // Default clip
            return;
        }

        bool highX = Mathf.Abs(velocity.x) > velocityThreshold;
        bool highY = Mathf.Abs(velocity.y) > velocityThreshold;

        // Determine animation clip based on velocity direction
        if (highX && highY)
        {
            if (velocity.x > 0 && velocity.y > 0) animator.Play("Up_Right"); // High X and Y
            else if (velocity.x < 0 && velocity.y > 0) animator.Play("Up_Left"); // High -X and Y
            else if (velocity.x < 0 && velocity.y < 0) animator.Play("Down_Left"); // High -X and -Y
            else if (velocity.x > 0 && velocity.y < 0) animator.Play("Down_Right"); // High X and -Y
            Debug.Log("highX and Y");
        }
        else if (highX)
        {
            if (velocity.x > 0) animator.Play("Right"); // High X
            else animator.Play("Left"); // High -X
            Debug.Log("high X");
        }
        else if (highY)
        {
            if (velocity.y > 0) animator.Play("Up"); // High Y
            else animator.Play("Down"); // High -Y

            Debug.Log("high Y");
        }
    }
}
