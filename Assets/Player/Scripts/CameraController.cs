using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera planeCam;
    public Camera playerCam;

    bool firstPersonMode = true;

    public AirplaneController airplaneController;
    public PlayerMovement playerMovement;

    public Transform plane;
    public Transform player;

    public float minDistanceToInteract = 5;

    public CharacterController characterController;

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
        if (!firstPersonMode)
        {
            player.position = plane.position;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            float distance = Vector3.Distance(plane.position, player.position);

            if (firstPersonMode && distance < minDistanceToInteract)
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

                player.position = plane.position + new Vector3(0, 5f, 0);
                characterController.enabled = true;
            }
        }
    }
}
