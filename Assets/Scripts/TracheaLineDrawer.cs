using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class TracheaLineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer; // Assign in the Inspector
    public float pointSpacing = 0.1f; // Minimum distance between points for smoothness
    public float lineSpeedFactor = 0.1f; // Factor to scale down the mouse velocity to control line speed
    public float maxSpeed = 5f;
    public float smoothingFactor = 0.05f;
    public MoveOrgan moveOrgan;

    private List<Vector3> points = new List<Vector3>();
    private Vector3 lastMousePosition;
    private Vector3 currentLinePosition;
    private bool isDrawing = false;
    private AudioSource tracheaAS;
    

    private bool IsMouseInWindow()
    {
        return Input.mousePosition.x >= 0 &&
            Input.mousePosition.y >= 0 &&
            Input.mousePosition.x <= Screen.width &&
            Input.mousePosition.y <= Screen.height;
    }

    private void Start()
    {
        tracheaAS = GetComponent<AudioSource>();
        // Ensure LineRenderer is assigned
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Start the line position at the initial mouse position
        currentLinePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentLinePosition.z = 0;

    }

    private void Update()
    {
        //Get the current mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; //Ensure line is in 2D space

        // Detect mouse hold and initiate drawing
        if (Input.GetMouseButton(0) && IsMouseInWindow() && !moveOrgan.isHolding)
        {
            if (!isDrawing)
            {
                // Start drawing when the mouse is first pressed
                points.Clear();
                lineRenderer.positionCount = 0;
                currentLinePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentLinePosition.z = 0;
                AddPoint(currentLinePosition); //Add the initial point where the mouse begins to draw
                isDrawing = true;
                Cursor.visible = false;
            }

            // Calculate mouse velocity (how much the mouse moved since last frame)
            Vector3 mouseDelta = mousePosition - lastMousePosition;

            // Calculate the speed of the line (capped at max speed)
            float speed = mouseDelta.magnitude / Time.deltaTime;
            speed = Mathf.Min(speed, maxSpeed); // Clamp speed to the max speed

            //Normalize the direction and scale by the lineSpeedFactor
            Vector3 lineMoveDirection = mouseDelta.normalized * speed * lineSpeedFactor;

            // Update the last mouse position
            lastMousePosition = mousePosition;

            // Smoothly interpolate the line's position towards the direction of movement
            currentLinePosition = Vector3.Lerp(currentLinePosition, currentLinePosition + lineMoveDirection, smoothingFactor);

            // Add the new point only if it is sufficiently far enough from the last point
            if (points.Count == 0 || Vector3.Distance(currentLinePosition, points[points.Count - 1]) >= pointSpacing)
            {
                // Initialize the first point at the mouse position
                AddPoint(currentLinePosition);
            }
            
        }
        // Optionally, clear line when the mouse button is released
        else if (Input.GetMouseButtonUp(0))
        {
            //points.Clear();
            isDrawing = false;
            Cursor.visible = true;
        }
    }

    private void AddPoint(Vector3 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
        var randomNumber = Random.Range(1, 9);
        if (randomNumber == 2)
        {
            tracheaAS.Play();
        }
       
    }

    private void ResetLine()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }

}
