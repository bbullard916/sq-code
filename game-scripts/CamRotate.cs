using UnityEngine;

public class CamRotate : MonoBehaviour
{
    public float rotationSpeed = 5.0f;
    public float lerpSpeed = 5.0f;

    private Quaternion targetRotation;

    void Start()
    {
        // Initialize target rotation to the current rotation
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // Check if the right mouse button is held down
        if (Input.GetMouseButton(1))
        {
            // Get the mouse movement
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Calculate the target rotation
            targetRotation *= Quaternion.Euler(-mouseY * rotationSpeed, mouseX * rotationSpeed, 0);
        }

        // Smoothly interpolate to the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
    }
}
