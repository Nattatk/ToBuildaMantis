using System.Collections;
using UnityEngine;

public class Organ : MonoBehaviour
{
    public bool isOxygenating { get; private set; } = false;
    public string organName = "Brain";
    public int oxygenLevel = 90;
    public bool isHypoxic { get; private set; } = false;

    private void Start()
    {
        StartCoroutine("OxygenateRoutine");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Object entered: " + collision.gameObject.name);
        if (collision.gameObject.name == "TracheaMesh")
        {
            isOxygenating = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "TracheaMesh")
        {
            isOxygenating = false;
        }
     
    }

    private IEnumerator OxygenateRoutine()
    {
        yield return new WaitForSeconds(1);

        if (isOxygenating == true && oxygenLevel <= 99)
        {
            oxygenLevel++;
        }
        else if (!isOxygenating && oxygenLevel >=60)
        {
            oxygenLevel--;
        }

        //set isHypoxic bool
        if (oxygenLevel <= 60)
        {
            isHypoxic = true;
        }

        StartCoroutine(OxygenateRoutine());
    }

}
