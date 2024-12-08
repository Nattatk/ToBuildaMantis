using UnityEngine;

public class MoveOrgan : MonoBehaviour
{
    /*When a player clicks and holds on an organ, the organ is lifted closer to the camera (as if it is picked up). The player, while holding,
      can move the mouse and the object will follow. When the mouse button releases, the organ falls to the original z coordinate.
    */

    private Vector3 mousePosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            if (Input.GetMouseButton(0))
            {
                // Check if the object has the organ info script
                Organ organInfo = hit.collider.GetComponent<Organ>();
                if (organInfo != null)
                {
                    Debug.Log("Mouse has picked up the organ " + organInfo.name);
                    Vector3 organVector3 = hit.collider.gameObject.transform.position;
                    organVector3.x = mousePosition.x;
                    organVector3.y = mousePosition.y; 
                    Debug.Log("organVector3.x = " +organVector3.x + " and mousePosition.x = " +mousePosition.x);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    
                }
            }
            
        }
        else
        {
            
        }
    }

    private void MoveOrganWithMouse()
    {
        
    }
}
