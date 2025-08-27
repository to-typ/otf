using UnityEngine;

public class Lenkrad : MonoBehaviour, IInteractable
{
    public Transform player;
    public Camera firstPersonCam;
    public Camera thirdPersonCam;
    public MouseLook mouseLook;
    public PlayerMovement playerMovement;
    public AnimationStateController animationStateController;
    public AirplaneController airplaneController;

    private bool steeringActive = false;

    public void Interact()
    {

    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance < 4f)
            {
                // Spieler ist nah genug zum Interagieren
                if (Input.GetKeyDown(KeyCode.E))
                {
                    steeringActive = !steeringActive;
                    firstPersonCam.enabled = !steeringActive;
                    thirdPersonCam.enabled = steeringActive;
                    mouseLook.enabled = !steeringActive;
                    playerMovement.enabled = !steeringActive;
                    animationStateController.enabled = !steeringActive;
                    airplaneController.enabled = steeringActive;
                }
            }
        }
    }
}
