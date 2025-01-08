using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;

    private Vector3 _currentVelocity;
    [SerializeField] private float smoothTime = 0.2f;
    
    protected void Update()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            target.position,
            ref _currentVelocity,
            smoothTime);
    }
}
