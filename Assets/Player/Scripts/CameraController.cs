using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera planeCam;
    public Camera playerCam;

    bool firstPersonMode = true;

    public AirplaneController airplaneController;
    public PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        airplaneController.enabled = false;
        planeCam.enabled = false;
        playerMovement.enabled = true;
        playerCam.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (firstPersonMode)
            {
                playerMovement.enabled = false;
                airplaneController.enabled = true;
                
                playerCam.enabled = false;
                planeCam.enabled = true;
                firstPersonMode = false;
            }
            else
            {
                airplaneController.enabled = false;
                playerMovement.enabled = true;

                planeCam.enabled = false;
                playerCam.enabled = true;
                firstPersonMode = true;
            }
        }
    }
}
