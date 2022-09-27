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
    [SerializeField] Rigidbody rb;
    // [SerializeField] CharacterController _chara;


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
        // orientation
        /* if (_playerMovement.x > 0)     //right
         {
             _root.rotation = Quaternion.Euler(0, 90, 0);
         }
         else if (_playerMovement.x < 0)   //left
         {
             _root.rotation = Quaternion.Euler(0, -90, 0);
         }
         if (_playerMovement.z > 0)     //right
         {
             _root.rotation = Quaternion.Euler(0, 0, 0);
         }
         else if (_playerMovement.z < 0)   //left
         {
             _root.rotation = Quaternion.Euler(0, -180, 0);
         }*/

        var tmp = new Vector3(_playerCamera.transform.forward.x, 0, _playerCamera.transform.forward.z);
        rb.transform.LookAt(rb.transform.position + tmp);

        var cameraDirection = _playerCamera.transform.TransformDirection(_playerMovement);
        cameraDirection.y = 0;
        rb.MovePosition(rb.transform.position + (cameraDirection * _speed * Time.fixedDeltaTime));

        //var tmp = new Vector3(forwardCamera.x, 0, forwardCamera.z);
        //rb.transform.LookAt(rb.transform.position + tmp);
        // _chara.Move(transform.position + (_playerMovement * _speed * Time.fixedDeltaTime));


        /* if (_playerMovement.magnitude > _MovingThreshold)  // si on est ent train de bouger alors 
         {
             _animator.SetBool("isWalking", true);
         }
         else
         {                 //sinon c'est qu'on bouge pas donc false
             _animator.SetBool("isWalking", false);
         }*/
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
