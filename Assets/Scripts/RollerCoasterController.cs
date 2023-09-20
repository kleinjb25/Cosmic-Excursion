using UnityEngine;

public class RollerCoasterController : MonoBehaviour
{
    public GameObject[] waypoints;
    public int currentWaypoint = 0;
    public float speed = 10f;
    public float groundOffset = 0.3f;
    public float rotSpeed = 1f;
    public Rigidbody cartRigidbody;
    public Rigidbody capsuleRigidbody;
    private void Start()
    {
        // Disable physics interactions between the cart and the capsule
        if (cartRigidbody != null && capsuleRigidbody != null)
        {
            Physics.IgnoreCollision(cartRigidbody.GetComponent<Collider>(), capsuleRigidbody.GetComponent<Collider>());
        }
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 1.5f)
            currentWaypoint++;
        if (currentWaypoint >= waypoints.Length)
            currentWaypoint = 0;
        Vector3 targetPosition = waypoints[currentWaypoint].transform.position;

        // Raycast downwards to detect the ground
        RaycastHit hit;
        if (Physics.Raycast(targetPosition, -Vector3.up, out hit))
        {
            // Adjust the target position to stick to the ground
            targetPosition.y = hit.point.y + groundOffset;
        }
        Quaternion lookAtWaypoint = Quaternion.LookRotation(waypoints[currentWaypoint].transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookAtWaypoint, Time.deltaTime * rotSpeed);
        this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
