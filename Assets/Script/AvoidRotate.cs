using UnityEngine;

public class FollowPlayerNoRotate : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;

        // Do NOT rotate with the player
        // Rotation stays exactly as you set it in the editor
    }
}
