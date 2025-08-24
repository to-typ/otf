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
    public GameObject planeCollider;

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
            player.rotation = plane.rotation;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            float distance = Vector3.Distance(plane.position, player.position);

            if (firstPersonMode && distance < minDistanceToInteract)
            {
                foreach (var item in planeCollider.GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(item, characterController, true);
                }

                playerMovement.enabled = false;
                airplaneController.enabled = true;

                playerCam.enabled = false;
                planeCam.enabled = true;
                firstPersonMode = false;
            }
            else if (!firstPersonMode)
            {
                airplaneController.enabled = false;
                playerMovement.enabled = true;

                planeCam.enabled = false;
                playerCam.enabled = true;
                firstPersonMode = true;

                player.position = plane.position + new Vector3(0, 5f, 0);
                player.rotation = Quaternion.Euler(0f, 0f, 0f);

                foreach (var item in planeCollider.GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(item, characterController, false);
                }
            }
        }
    }
}
