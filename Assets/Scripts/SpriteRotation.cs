using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    /*In Unity I want a script to keep track of the velocity of the object. If the velocity reaches a certain amount in the X direction,
    then in the animator controller play animation clip #1. If the velocity is a certain amount in the -X direction, play clip #2;
    if the velocity is a certain amount in the Y direction, play clip #3; if the velocity is a certain amount in the -Y direction,
    then play clip #4. If the velocity is zero or close to zero then return to original animation state.
    Additionally, if the velocity is high in both X and Y direction, then play clip #5; high in -X and Y play clip #6;
    high in -X and -Y play clip #7; high in X and -Y play clip #8*/

    
    public Animator animator; // Reference to the Animator component
    public float momentumDurationFactor = 0.5f;
    public float velocityThreshold = 1.0f; // Threshold for "high velocity"
    public float diagonalThresholdRatio = 0.5f; 

    
    private Vector3 lastPosition;
    private Vector2 lastDirection;
    private float stateTimer = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }
    private void Update()
    {
        // Calculate velocity
        Vector3 currentPosition = transform.position;
        Vector2 velocity = (currentPosition - lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;

        // Determine the direction if the velocity exceeds the threshold
        if (velocity.magnitude > velocityThreshold)
        {
            lastDirection = velocity.normalized;
            stateTimer = velocity.magnitude * momentumDurationFactor; // Set timer proportional to speed

            // Check for diagonal movement first
            if (Mathf.Abs(lastDirection.x) > diagonalThresholdRatio && Mathf.Abs(lastDirection.y) > diagonalThresholdRatio)
            {
                // Both x and y are significant; choose diagonal direction
                if (lastDirection.x > 0 && lastDirection.y > 0) animator.Play("Up_Right");
                else if (lastDirection.x < 0 && lastDirection.y > 0) animator.Play("Up_Left");
                else if (lastDirection.x < 0 && lastDirection.y < 0) animator.Play("Down_Left");
                else if (lastDirection.x > 0 && lastDirection.y < 0) animator.Play("Down_Right");
            }
            else if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
            {
                // Horizontal movement dominates
                if (lastDirection.x > 0) animator.Play("Right");
                else animator.Play("Left");
            }
            else
            {
                // Vertical movement dominates
                if (lastDirection.y > 0) animator.Play("Up");
                else animator.Play("Down");
            }
        }
        else if (stateTimer > 0)
        {
            //Decrement the timer to maintain the state
            stateTimer -= Time.deltaTime;
        }
        else
        {
            //Reset to idle state if the timer runs out
            animator.Play("Center");
        }

    }
}
