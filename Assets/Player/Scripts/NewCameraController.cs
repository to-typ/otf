using TMPro;
using UnityEngine;
using System.Collections; // <-- Hinzugefügt für IEnumerator

public class NewCameraController : MonoBehaviour
{
    public Transform player;
    public Transform plane;

    public Camera playerCamera;
    public Camera planeCamera;

    public AirplaneController planeController;
    public PlayerMovement playerMovement;
    public AnimationStateController animationStateController;
    public MouseLook mouseLook;
    [SerializeField] TextMeshProUGUI cannotExitPlane;

    public void Start()
    {
        cannotExitPlane.enabled = false;
        planeController.enabled = false;
        playerMovement.enabled = true;
        animationStateController.enabled = true;
        mouseLook.enabled = true;

        playerCamera.enabled = true;
        planeCamera.enabled = false;
    }

    public void Update()
    {
        float distanceToPlane = Vector3.Distance(player.position, plane.position);
        if (Input.GetKeyDown(KeyCode.C) && distanceToPlane < 4f)
        {
            if (playerCamera.enabled)
            {
                playerCamera.enabled = false;
                planeCamera.enabled = true;
                planeController.enabled = true;
                playerMovement.enabled = false;
                animationStateController.enabled = false;
                mouseLook.enabled = false;

                player.gameObject.transform.parent = plane;
            }
            /*
            else if (!planeController.isAirborne)
            {
                playerCamera.enabled = true;
                planeCamera.enabled = false;
                planeController.enabled = false;
                playerMovement.enabled = true;
                animationStateController.enabled = true;
                mouseLook.enabled = true;

                player.gameObject.transform.parent = null;
                player.rotation = Quaternion.Euler(0f, plane.rotation.eulerAngles.y, 0f);
            }
            else
            {
                cannotExitPlane.enabled = true;
                StartCoroutine(HideCannotExitPlane());
            }
            */
        }
    }

    private IEnumerator HideCannotExitPlane()
    {
        yield return new WaitForSeconds(2f);
        cannotExitPlane.enabled = false;
    }
}
