using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float angularSpeed = 360f;
    private Camera _mainCamera;
    private CharacterController _characterController;
    //private Animator _animator;
    
    protected void Awake()
    {
        _mainCamera = Camera.main;
        _characterController = GetComponent<CharacterController>();
        //_animator = GetComponentInChildren<Animator>();
    }
    
    protected void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            var targetDirection = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");
            _characterController.Move(targetDirection.normalized * (speed * Time.deltaTime));
            //_animator.SetBool("IsMoving", true);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(targetDirection),
                angularSpeed * Time.deltaTime);
        }
        else
        {
            //_animator.SetBool("IsMoving", false);
        }
        
    }
}
