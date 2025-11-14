using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // character
    public Transform pivot;        // camera pivot/angle point
    public float followSpeed = 5f;
    public float rotateSpeed = 6f;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetPivot(Transform newPivot)
    {
        pivot = newPivot;
    }

    void LateUpdate()
    {
        if (target == null || pivot == null) return;

        // Position camera at pivot
        transform.position = Vector3.Lerp(
            transform.position,
            pivot.position,
            Time.deltaTime * followSpeed
        );

        // Rotate camera toward pivot rotation
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            pivot.rotation,
            Time.deltaTime * rotateSpeed
        );
    }
}

// using UnityEngine;

// public class CameraFollow : MonoBehaviour
// {
//     public Transform target;
//     public Vector3 offset = new Vector3(-5f, 3f, 0f);
//     public float smoothTime = 3f; // How long it takes to reach the target
// 	// Quaternion tilt = Quaternion.Euler(22f, 0f, 0f); // 20 degrees down, no yaw, no roll

//     private Vector3 velocity = Vector3.zero;

// 	void FixedUpdate()
// 	{
// 		if (target == null) return;

// 		Vector3 desiredPosition = target.position + offset;
// 		transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothTime * Time.fixedDeltaTime);
// 		// transform.rotation = Quaternion.LookRotation(target.forward, Vector3.up);
// 		transform.LookAt(target);
// 	}

//     public void SetTarget(Transform newTarget)
//     {
//         target = newTarget;
//     }
// }
