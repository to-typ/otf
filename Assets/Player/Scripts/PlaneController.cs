using UnityEngine;
using TMPro;

public class PlaneController : MonoBehaviour
{
    public float throttleIncrement = 0.1f;
    public float maxThrust = 200f;
    public float responsiveness = 10f;
    public float lift = 135f;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    private float responseModifier
    {
        get {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    Rigidbody rb;
    [SerializeField] TextMeshProUGUI hud;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Horizontal");
        pitch = Input.GetAxis("Vertical");
        yaw = -Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space)) 
        {
            throttle += throttleIncrement;
        }
        else if (Input.GetKey(KeyCode.LeftControl)) 
        {
            throttle -= throttleIncrement;
        }
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }

    private void Update()
    {
        HandleInputs();
        UpdateHud();
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(-transform.forward * roll * responseModifier);

        //Lift
        rb.AddForce(Vector3.up * rb.linearVelocity.magnitude * lift);
    }

    private void UpdateHud()
    {
        hud.text = "Throttle: " + throttle.ToString("F0") + "%\n";
        hud.text += "Airspeed: " + (rb.linearVelocity.magnitude * 3.6f).ToString("F0") + " km/h\n";
        hud.text += "Altitude: " + transform.position.y.ToString("F0") + " m";
    }
}
