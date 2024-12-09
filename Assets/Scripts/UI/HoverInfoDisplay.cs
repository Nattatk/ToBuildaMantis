using UnityEngine;
using TMPro;

public class HoverInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    // Update is called once per frame
    private void Update()
    {
        // Raycast from the mouse position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            // Check if the object has the ObjectInfo script
            Organ organInfo = hit.collider.GetComponent<Organ>();
            if (organInfo != null)
            {
                // Update the InfoText with the object's information
                infoText.text = $"Type: {organInfo.organName}\nIs Oxygenating: {organInfo.isOxygenating}\nOxygen Level: {organInfo.oxygenLevel}%" +
                    $"\nHypoxic: {organInfo.isHypoxic}";
            }
            else
            {
                // Clear the text if the object doesn't have the script
                infoText.text = "";
            }
        }
        else
        {
            // Clear the text if no object is hovered over
            infoText.text = "";
        }
    }
}
