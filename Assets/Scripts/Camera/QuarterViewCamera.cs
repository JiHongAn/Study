using UnityEngine;

public class QuarterViewCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(10f, 10f, -10f);
    public float smoothSpeed = 0.125f;
    public float rotationSmoothSpeed = 2f;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSmoothSpeed * Time.deltaTime);
    }
}