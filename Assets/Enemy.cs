using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    enum StateAI { PATROL, CHASE, ATTACK, FinalAtaack }

    [SerializeField] float _minDistance = 1f;
    [SerializeField] GameObject _player;
    [SerializeField] Transform[] PatrolPoint;
    [SerializeField] int startingPoint;
    [SerializeField] float _speed;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] float _gravityPush;

    float targetDistance;
    [SerializeField] float _zoneChase;
    [SerializeField] float stopchase;

    // Vector3 _playerChase;
    StateAI _state;
    Vector3 _gravity;
    int i;

    // Start is called before the first frame update
    void Start()
    {
        _agent.SetDestination(PatrolPoint[startingPoint].position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ChasePlayer = Vector3.zero;

        switch (_state)
        {
            case StateAI.PATROL:
                Patrol();

                // Transition
                if (Vector3.Distance(transform.position, _player.transform.position) <= _zoneChase)
                {
                    _state = StateAI.CHASE;
                }
            /*    else if(false)
                {
                    _state = StateAI.ATTACK;

                }*/
                break;
            case StateAI.CHASE:

                targetDistance = Vector2.Distance(transform.position, _player.transform.position);
                if(targetDistance < stopchase)
                {
                    _state = StateAI.ATTACK;
                }
                else if (targetDistance < _zoneChase && targetDistance > stopchase)
                {
                    _agent.SetDestination(_player.transform.position);
                }
                else if(targetDistance > _zoneChase)
                {
                    _state = StateAI.PATROL;
                }

                break;
            case StateAI.ATTACK:
                
                targetDistance = Vector2.Distance(transform.position, _player.transform.position);
                if (targetDistance > stopchase)
                {
                    _state = StateAI.PATROL;
                }
                break;
            
            default:
                break;
        }

        // Gravity
        //if (_chara.isGrounded)
        //{
        //    _gravity.y = 0;
        //}
        //else
        //{
        //    _gravity.y += _gravityPush;
        //}

        //_chara.Move((direction * _speed * Time.deltaTime) + (_gravity * Time.deltaTime));
        //_chara.transform.LookAt(_chara.transform.position + _agent.d);
    }

    void Patrol()
    {
        // Go to patrol
        if (_agent.remainingDistance < _minDistance)
        {
            i++;
            if (i >= PatrolPoint.Length)
            {
                i = 0; // reset index
            }
            _agent.SetDestination(PatrolPoint[i].position);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _zoneChase);
    }
}