using UnityEngine;

public class HammerController : MonoBehaviour
{
    public float hammerSpeed = 10f; 
    public float hammerDownTime = 0.1f; 
    private bool isHammerActive = false;
    private Vector3 targetPosition; 

    void Update()
    {
        if (isHammerActive)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * hammerSpeed);
            Invoke("HideHammer", hammerDownTime);
        }
    }

    public void ShowHammerAtPosition(Vector3 position)
    {
        float hammerOffsetY = 1.0f; 
        targetPosition = new Vector3(position.x, position.y + hammerOffsetY, position.z);
        transform.position = new Vector3(position.x, position.y + 2.0f, position.z);

        isHammerActive = true;
    }

    void HideHammer()
    {
        isHammerActive = false;
        gameObject.SetActive(false);
    }
}
