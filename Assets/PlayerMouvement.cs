using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement : MonoBehaviour
{
    enum StatePlayer { IDLE, Run, Walk, Crounch}
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

    [SerializeField] InputActionReference _jumpInput;

    [SerializeField] float _jumpPower;
    [SerializeField] float _jumpMax; //saut max
    [SerializeField] float _jumpNumbercurrent; // mon nombre de saut
    [SerializeField] float _minJump;
    [SerializeField] bool _isJumping;

    [SerializeField] InputActionReference _crounchInput;

    
    [SerializeField] GameObject _sword;
    [SerializeField] GameObject _positionBack;
    [SerializeField] GameObject _handPosition;

    public bool _equiped;
    [SerializeField] InputActionReference _ArmeWeapon;

    [SerializeField] InputActionReference _SprintInput;

    [SerializeField] float _sprint;
    Vector3 _gravity;


    StatePlayer _playerState;
    void Start()
    {
        _playerState = StatePlayer.IDLE;
        _moveInput.action.started += StartMove;
        _moveInput.action.performed += UdpateMove;
        _moveInput.action.canceled += EndMove;

        _jumpInput.action.started += StartJump;;

        _crounchInput.action.started += StartCrounch;
        _crounchInput.action.performed += UdpateCrounch;
        _crounchInput.action.canceled += EndCrounch;
        _equiped = false;

        _ArmeWeapon.action.started += StartWeapon;


        _SprintInput.action.started += Action_started;
        _SprintInput.action.performed += uptadeSprint;

    }


    private void Update()
    {
        _animator.SetBool("isRunning", _playerState == StatePlayer.Run);
        _animator.SetBool("Iswalking", _playerState == StatePlayer.Walk);
        _animator.SetBool("Crounch", _playerState == StatePlayer.Crounch);

        if (_playerMovement.magnitude > _MovingThreshold)  // si on est ent train de bouger alors 
        {
            _animator.SetFloat("Horrizontal", _playerMovement.x);
            _animator.SetFloat("Vertical", _playerMovement.z);
        }
    }

    private void LateUpdate()
    {
        if (_equiped)
        {
            _animator.SetBool("SwordIddle", true);
            _sword.transform.position = _handPosition.transform.position;
            _sword.transform.rotation = _handPosition.transform.rotation;
        }
        else
        {
            _animator.SetBool("SwordIddle", false);
            _sword.transform.position = _positionBack.transform.position;
            _sword.transform.rotation = _positionBack.transform.rotation;
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
        //_gravity = new Vector3(0, _gravityPush, 0);

        if (_chara.isGrounded)
        {
            if(_isJumping)
            {
        
            }
            else
            {
                 _gravity.y = 0;
                _jumpNumbercurrent = 0;
            }
            //_gravityPush = -7;
        }
        else
        {
            _gravity.y += _gravityPush;
        }
        if (_isJumping && _chara.isGrounded)
        {
            _isJumping = false;
            _animator.SetTrigger("Landing");
            
        }

        //if (_isJumping)
        //{
        //    _gravityPush = -2;
        //}


        _chara.Move((cameraDirection * _speed * Time.fixedDeltaTime) + (_gravity * Time.deltaTime)) ;
        if (_playerState == StatePlayer.Run)
        {
            _chara.Move((cameraDirection * _sprint * Time.fixedDeltaTime) + (_gravity * Time.deltaTime));
        }

    }
    public void StartMove(InputAction.CallbackContext obj)
    {
        _playerState = StatePlayer.Walk;
        var joystick = obj.ReadValue<Vector2>();
        _playerMovement = new Vector3(joystick.x, 0, joystick.y);

    
    }
    private void UdpateMove(InputAction.CallbackContext obj)
    {
        Debug.Log("Update Move");
        var joystick = obj.ReadValue<Vector2>();
        _playerMovement = new Vector3(joystick.x, 0, joystick.y);
        if(_playerState == StatePlayer.Run || _playerState == StatePlayer.Crounch)
        {
            // nothing
        }
        else
        {
            _playerState = StatePlayer.Walk;
        }
    }
    void EndMove(InputAction.CallbackContext obj)
    {
        _playerState = StatePlayer.IDLE;
       _playerMovement = new Vector3(0, 0, 0);
    }
    private void StartJump(InputAction.CallbackContext obj)
    {
        if (_chara.isGrounded && _jumpNumbercurrent < _jumpMax)
        {
            _gravity = new Vector3(0, _jumpPower, 0);
            _animator.SetTrigger("Jump");
           //_chara.Move (_gravity * Time.fixedDeltaTime);
            _isJumping = true;
            _jumpNumbercurrent++;
        }
    }
    public void StartCrounch(InputAction.CallbackContext obj)
    {
        _playerState = StatePlayer.Crounch;
    }
    private void UdpateCrounch(InputAction.CallbackContext obj)
    {
        _playerState = StatePlayer.Crounch;
    }
    void EndCrounch(InputAction.CallbackContext obj)
    {
    }
    
    public void StartWeapon(InputAction.CallbackContext obj)
    {
        if (_equiped)
        {
            _animator.SetTrigger("_unEquiped");
            _equiped = false;
        }
        else
        {
            _animator.SetTrigger("Equiped");
            _equiped = true;
        }
        
    }
    private void Action_started(InputAction.CallbackContext obj)
    {

    }
    private void uptadeSprint(InputAction.CallbackContext obj)
    {
        Debug.Log("Update Sprint");
        _playerState = StatePlayer.Run;
    }
}
