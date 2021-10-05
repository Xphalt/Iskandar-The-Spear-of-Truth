
/* 
 * Dominique, 30-09-2021
 * Player Controller for testing of the outline shader
 */

using UnityEngine;

public class ShaderTest : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public float moveSpeed = 2f;

    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 90.0f;
    private float rotX;

    public int rayRange = 6;
    private RaycastHit hit;
    private GameObject highlightedObject;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        highlightedObject = gameObject;
    }

    void Update()
    {
        // Mouse
        float y = Input.GetAxis("Mouse X") * turnSpeed;
        rotX += Input.GetAxis("Mouse Y") * turnSpeed;
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);
        transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);

        // Move
        Vector3 dir = new Vector3(0, 0, 0);
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        // Shader
#if DEBUG // Dominique 30-09-2021, Raycast debugging
        Vector3 forward = transform.TransformDirection(Vector3.forward) * rayRange;
        Debug.DrawRay(transform.position, forward, Color.red);
#endif // DEBUG
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayRange))
        {
            // Dominique 30-09-2021, If the object hit isn't the one currently highlighted then clear the old object's highlight and set the new one's to show
            if (highlightedObject != hit.collider.gameObject)
            {
                ShaderHandler.instance.SetOutlineColor(highlightedObject, Color.clear);
                highlightedObject = hit.collider.gameObject;
                ShaderHandler.instance.SetOutlineColor(highlightedObject, Color.cyan);
            }
        }
        // Dominique 30-09-2021, If we don't hit an object clear the highlight
        else if (highlightedObject != gameObject)
        {
            ShaderHandler.instance.SetOutlineColor(highlightedObject, Color.clear);
            highlightedObject = gameObject;
        }
    }
}