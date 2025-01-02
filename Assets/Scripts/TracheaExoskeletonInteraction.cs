using UnityEngine;

public class TracheaExoskeletonInteraction : MonoBehaviour
{
    /*The player is drawing an insect trachea and when the trachea overlaps an organ, the organ receives oxygen. 
     * What I want next is to have an exoskeleton object. When the mouse is near the exoskeleton, an indicator 
     * shows the point where a player can begin drawing a trachea line. The point changes along the length of 
     * the exoskeleton nearest the player mouse. 
     * If the point indicator is showing, that means the player mouse is near and that they can begin drawing a trachea line. 
     * If the mouse is not near enough then the player cannot draw.*/

    public GameObject exoskeleton; //reference to exoskeleton transform
    public GameObject spiraclePointIndicator; //reference to spiracle point indicator object
    public float activationRadius; // area where trachea drawing can begin
    public LayerMask exoskeletonLayer; // Layer for raycasting against the exoskeleton

    public TracheaLineDrawer tlDrawer;

    private Vector3 outOfBounds = new Vector3(40, 40, 0);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = MousePositionManager.GetMouseWorldPosition();

        // Find the nearest point on the exoskeleton
        Vector3 nearestPoint = GetNearestPointOnExoskeleton(mouseWorldPos);

        // Calculate the distance between mouse and nearest point
        float distance = Vector3.Distance(mouseWorldPos, nearestPoint);

        // Check if within activation radius
        if (distance <= activationRadius)
        {
            tlDrawer.on = true;
            //spiraclePointIndicator.SetActive(true);
            spiraclePointIndicator.transform.position = nearestPoint;
        }
        else
        {
            tlDrawer.on = false;
            spiraclePointIndicator.transform.position = outOfBounds;
        }
    }

    Vector3 GetNearestPointOnExoskeleton(Vector3 point)
    {
        // Use physics to find the nearest point on the exoskeleton
        Ray ray = new Ray(point, Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, exoskeletonLayer))
        {
            return hit.point; // Return the hit point on the exoskeleton
        }

        return exoskeleton.transform.position; // Fallback to the exoskeleton position
    }
}
