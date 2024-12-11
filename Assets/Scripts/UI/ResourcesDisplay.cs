using UnityEngine;
using TMPro;

public class ResourcesDisplay : MonoBehaviour
{
    public TextMeshProUGUI resourcesText;
    public ResourceManager resources;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resourcesText.text = "Resources";
    }

    // Update is called once per frame
    void Update()
    {
        resourcesText.text = $"Resources\nChitin: {resources.rChitin}";
    }
}
