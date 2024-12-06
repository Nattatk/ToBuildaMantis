using System.Collections;
using UnityEngine;

public class Organ : MonoBehaviour
{
    public bool isOxygenating { get; private set; } = false;
    public string organName = "Brain";
    public int oxygenLevel = 90;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Object entered: " + collision.gameObject.name);
        if (collision.gameObject.name == "TracheaMesh")
        {
            isOxygenating = true;
            StartCoroutine("OxygenateRoutine");
        }
    }

    private IEnumerator OxygenateRoutine()
    {
        yield return new WaitForSeconds(1);

        if (isOxygenating == true && oxygenLevel <= 99)
        {
            oxygenLevel++;
        }
        StartCoroutine("OxygenateRoutine");
    }
}
