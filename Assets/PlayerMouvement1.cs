using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement1 : MonoBehaviour
{
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] Transform _root;
    [SerializeField] float _speed;
    [SerializeField] float _MovingThreshold;
    [SerializeField] Animator _animator;
    Vector3 _playerMovement;
    [SerializeField] GameObject _playerCamera;
   [SerializeField] Rigidbody rb;
    public float groundDistance = 0.5f;
    [SerializeField] InputActionReference _jumpInput;
    [SerializeField] float _jumpPower;
    [SerializeField] float _jumpMax; //saut max
    [SerializeField] float _jumpNumbercurrent; // mon nombre de saut
    [SerializeField] float _minJump;
   public bool _isJumping;
    //   [SerializeField] Transform _GroudCheck;
    [SerializeField] Transform _raycastRoot;
    [SerializeField] Vector3 raycastDirection;
    public bool _isGrounded;
    [SerializeField] Transform _rayCastR;

    void Start()
    {
        _moveInput.action.started += StartMove;
        _moveInput.action.performed += UdpateMove;
        _moveInput.action.canceled += EndMove;

       _jumpInput.action.started += StartJump;

    }


    private void Update()
    {

        if (_playerMovement.magnitude > _MovingThreshold)  // si on est ent train de bouger alors 
        {
            _animator.SetBool("Iswalking", true);
            _animator.SetFloat("Horrizontal", _playerMovement.x);
            _animator.SetFloat("Vertical", _playerMovement.z);
        }
        else
        {                 //sinon c'est qu'on bouge pas donc false
            _animator.SetBool("Iswalking", false);
        }

        if (Physics.Raycast(_raycastRoot.position, raycastDirection, out RaycastHit hit, raycastDirection.magnitude))
        {
            Debug.DrawLine(_raycastRoot.position, _raycastRoot.position + raycastDirection, Color.magenta);
            _isGrounded = true;
            _isJumping = false;
        }
        else
        {
            Debug.DrawLine(_raycastRoot.position, _raycastRoot.position + raycastDirection, Color.red);
            _isGrounded = false;
            _isJumping = true;
        }
        
        if (_isGrounded == true)
        {
            _jumpNumbercurrent = _minJump;
        }
        if (Physics.Raycast(_rayCastR.position, raycastDirection, out RaycastHit raycastHit, raycastDirection.magnitude))
        {
            Debug.DrawLine(_rayCastR.position, _rayCastR.position + raycastDirection, Color.magenta);
            _isGrounded = true;
            _isJumping = false;
        }
        else
        {
            Debug.DrawLine(_rayCastR.position, _rayCastR.position + raycastDirection, Color.red);
            _isGrounded = false;
            _isJumping = true;
        }


    }

    void FixedUpdate()
    {

        var tmp = new Vector3(_playerCamera.transform.forward.x, 0, _playerCamera.transform.forward.z);
        rb.transform.LookAt(rb.transform.position + tmp);
 
        
        var cameraDirection = _playerCamera.transform.TransformDirection(_playerMovement);
        cameraDirection.y = 0;
        rb.MovePosition(rb.transform.position + (cameraDirection * _speed * Time.fixedDeltaTime));
    }
    public void StartMove(InputAction.CallbackContext obj)
    {
     var joystick = obj.ReadValue<Vector2>();
        _playerMovement = new Vector3(joystick.x, 0, joystick.y);

    
    }
    private void UdpateMove(InputAction.CallbackContext obj)
    {
        var joystick = obj.ReadValue<Vector2>();
        _playerMovement = new Vector3(joystick.x, 0, joystick.y);
    }
    void EndMove(InputAction.CallbackContext obj)
    {
       _playerMovement = new Vector3(0, 0, 0);
    }

    private void StartJump(InputAction.CallbackContext obj)
    {
        if (_isGrounded = true && _jumpNumbercurrent < _jumpMax)
        {
            _animator.SetTrigger("Jump");
            rb.velocity = Vector3.up * _jumpPower;
        }
        _jumpNumbercurrent++;
    }

/*   bool IsGrounded()
    {
      
    }*/
}
