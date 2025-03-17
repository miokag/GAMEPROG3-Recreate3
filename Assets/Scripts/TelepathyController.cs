using UnityEngine;

public class TelepathyController : MonoBehaviour
{
    public float pullForce = 10f;
    public float throwForce = 20f;
    public float maxGrabDistance = 10f;
    public Transform holdPosition;
    public LayerMask grabbableLayer;

    private GameObject grabbedObject;
    private bool isHoldingObject = false;

    void Update()
    {
        // Visualize the raycast
        Vector3 raycastOrigin = Camera.main.transform.position;
        Vector3 raycastDirection = Camera.main.transform.forward;
        Debug.DrawRay(raycastOrigin, raycastDirection * maxGrabDistance, Color.green);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHoldingObject)
            {
                TryGrabObject();
            }
            else
            {
                ThrowObject();
            }
        }

        if (isHoldingObject && grabbedObject != null)
        {
            // Move the object to the hold position
            grabbedObject.transform.position = holdPosition.position;
            grabbedObject.transform.rotation = holdPosition.rotation;
        }
    }

    void TryGrabObject()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = Camera.main.transform.position;
        Vector3 raycastDirection = Camera.main.transform.forward;

        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, maxGrabDistance, grabbableLayer))
        {
            Debug.Log("Object detected: " + hit.collider.name);
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("Object grabbed: " + hit.collider.name);
                grabbedObject = hit.collider.gameObject;

                rb.isKinematic = true; 
                rb.detectCollisions = false; 

                grabbedObject.transform.SetParent(holdPosition);

                isHoldingObject = true;
            }
        }
        else
        {
            Debug.Log("No grabbable object detected.");
        }
    }

    void ThrowObject()
    {
        if (grabbedObject != null)
        {
            Debug.Log("Object thrown: " + grabbedObject.name);
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();

            rb.isKinematic = false;
            rb.detectCollisions = true;

            grabbedObject.transform.SetParent(null);

            rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

            grabbedObject = null;
            isHoldingObject = false;
        }
    }
}