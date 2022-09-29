using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement : MonoBehaviour
{
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] Transform _root;
    [SerializeField] float _speed;
    [SerializeField] float _MovingThreshold;
    [SerializeField] Animator _animator;
    Vector3 _playerMovement;
    [SerializeField] GameObject _playerCamera;
   // [SerializeField] Rigidbody rb;
    [SerializeField] CharacterController _chara;
    [SerializeField] float _gravityPush;


    void Start()
    {
        _moveInput.action.started += StartMove;
        _moveInput.action.performed += UdpateMove;
        _moveInput.action.canceled += EndMove;
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



    }

    void FixedUpdate()
    {
        // Orientation
        var tmp = new Vector3(_playerCamera.transform.forward.x, 0, _playerCamera.transform.forward.z);
      //  rb.transform.LookAt(rb.transform.position + tmp);
        _chara.transform.LookAt(_chara.transform.position + tmp); 
       

        // Move
        var cameraDirection = _playerCamera.transform.TransformDirection(_playerMovement);
        cameraDirection.y = 0;

        // Gravité
        var gravity = new Vector3(0, _gravityPush, 0);
        if (_chara.isGrounded)
        {
            gravity.y = 0;
        }




        // rb.MovePosition(rb.transform.position + (cameraDirection * _speed * Time.fixedDeltaTime));
        _chara.Move((cameraDirection * _speed * Time.fixedDeltaTime) + (gravity * Time.deltaTime)) ;
        
        //var tmp = new Vector3(forwardCamera.x, 0, forwardCamera.z);
        //rb.transform.LookAt(rb.transform.position + tmp);

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
}
