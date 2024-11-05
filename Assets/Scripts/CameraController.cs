using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 5f;
    public float rotationIncrement = 90f;

    float targetRotY = 0;
    float currentRotY = 0;

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");
        Vector3 forward = Quaternion.Euler(0, targetRotY, 0) * Vector3.forward;
        Vector3 right = Quaternion.Euler(0, targetRotY, 0) * Vector3.right;

        Vector3 movement = (forward * yMove + right * xMove).normalized;
        transform.position += movement * cameraSpeed * Time.deltaTime;
    }

    void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetRotY += rotationIncrement;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            targetRotY -= rotationIncrement;
        }

        currentRotY = Mathf.LerpAngle(currentRotY, targetRotY, 20 * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, currentRotY, 0f);
    }
}
