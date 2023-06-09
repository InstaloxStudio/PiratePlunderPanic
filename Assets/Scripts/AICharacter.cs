using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AICharacter : NetworkBehaviour
{
    public NavMeshAgent _agent;
    public Transform _target;
    public float _walkSpeed = 1.5f;
    public float _runSpeed = 3.5f;
    public float _dangerRadius = 10f;
    public float _attackRadius = 2f;
    public float _attackRate = 1f;
    public float _attackDamage = 10f;
    public float _attackTimer = 0f;

    public AIState _aiState = AIState.Idle;
    private bool _idleTimerRunning;
    public float _idleTime=3;
    private bool _isWandering;
    public float wanderTime=3;

    public void Awake()
    {
        _agent = TryGetComponent<NavMeshAgent>(out NavMeshAgent agent) ? agent : gameObject.AddComponent<NavMeshAgent>();

        _agent.speed = _walkSpeed;
        _agent.stoppingDistance = _attackRadius;
        _agent.autoBraking = false;


    }

    public void Update()
    {
        if (!isServer) return;


        switch (_aiState)
        {
            case AIState.Idle:
                Idle();
                break;
            case AIState.Wander:
                Wander();
                break;
            case AIState.Chase:
                Chase();
                break;
            case AIState.Attack:
                Attack();
                break;
            case AIState.Dead:
                Dead();
                break;
        }

        if (_target == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _dangerRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    _target = collider.transform;
                    _aiState  = AIState.Chase;
                    break;
                }
            }
        }
        else
        {
            if (_target.CompareTag("Player"))
            {
                float distance = Vector3.Distance(transform.position, _target.position);
                if (distance <= _attackRadius)
                {
                    _agent.isStopped = true;
                    _attackTimer += Time.deltaTime;
                    if (_attackTimer >= _attackRate)
                    {
                        _attackTimer = 0f;
                        // _target.GetComponent<PlayerCharacter>().TakeDamage(_attackDamage);
                    }
                }
                else
                {
                    _agent.isStopped = false;
                    _agent.SetDestination(_target.position);
                    _agent.speed = _runSpeed;
                }
            }
            else
            {
                _target = null;
            }
        }
    }

    private void Dead()
    {
        //play death animation
        //have the body stay on the ground for a while so the player can loot it
        //destroy the body

    }

    private void Attack()
    {
        //attack the target using the attack rate
        if (_target.CompareTag("Player"))
        {
            float distance = Vector3.Distance(transform.position, _target.position);
            if (distance <= _attackRadius)
            {
                _agent.isStopped = true;
                _attackTimer += Time.deltaTime;
                if (_attackTimer >= _attackRate)
                {
                    _attackTimer = 0f;
                    // _target.GetComponent<PlayerCharacter>().TakeDamage(_attackDamage);
                    //attack the target code goes here

                }
            }
            else
            {
                _agent.isStopped = false;
                _agent.SetDestination(_target.position);
                _agent.speed = _runSpeed;
            }
        }


    }

    private void Chase()
    {
        //chase the target
        if(_target==null)
        {
            _aiState = AIState.Idle;
            return;
        }

        float distance = Vector3.Distance(transform.position, _target.position);
        if (distance <= _attackRadius)
        {
            _agent.isStopped = true;
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attackRate)
            {
                _attackTimer = 0f;
                // _target.GetComponent<PlayerCharacter>().TakeDamage(_attackDamage);
            }
        }
        else
        {
            _agent.isStopped = false;
            _agent.SetDestination(_target.position);
            _agent.speed = _runSpeed;
        }

    }

    private void Wander()
    {
        if (_agent.pathStatus == NavMeshPathStatus.PathComplete && !_isWandering)
        {
            StartCoroutine(Wandering());
        }
    }

    private void Idle()
    {
        if (!_idleTimerRunning)
        {
            StartCoroutine(IdleTimer());
        }

    }

    private IEnumerator Wandering()
    {
        _isWandering = true;
        WanderAroundRadius(_dangerRadius, transform.position);
        yield return new WaitForSeconds(wanderTime);
        _isWandering = false;
    }

    IEnumerator IdleTimer()
    {
        _idleTimerRunning = true;
        yield return new WaitForSeconds(_idleTime);
        _aiState = AIState.Wander;
        _idleTimerRunning = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _dangerRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }


    //wander
    void WanderAroundRadius(float radius, Vector3 center)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * radius;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }
}

public enum AIState
{
    Idle,
    Wander,
    Chase,
    Attack,
    Dead
}