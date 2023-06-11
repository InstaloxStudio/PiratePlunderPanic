using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class AICharacter : NetworkBehaviour, IInteractable,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler    
{
    public NavMeshAgent _agent;
    public Transform _target;
    public float _walkSpeed = 1.5f;
    public float _runSpeed = 3.5f;
    public float _dangerRadius = 10f;
    public float _wanderRadius = 5f;
    public float _attackRadius = 2f;
    public float _attackRate = 1f;
    public float _attackDamage = 10f;
    public float _attackTimer = 0f;

    private bool _idleTimerRunning;
    public float _idleTime = 3;
    private bool _isWandering;
    public float _wanderTime = 3;

    private Color _originalColor;
    private Color _highlightColor = Color.white;

    [SyncVar(hook = nameof(OnTargetChanged))]
    public NetworkIdentity _targetIdentity;

    [SyncVar(hook = nameof(OnAIStateChanged))]
    public AIState _aiState = AIState.Idle;

    [SyncVar(hook = nameof(OnHealthChanged))]
    public float _currentHealth;

    public bool _isDead = false;

    public HealthComponent _healthComponent;

    public void Awake()
    {
        _healthComponent = TryGetComponent<HealthComponent>(out HealthComponent healthComponent)
            ? healthComponent : gameObject.AddComponent<HealthComponent>();

        _agent = TryGetComponent<NavMeshAgent>(out NavMeshAgent agent)
            ? agent : gameObject.AddComponent<NavMeshAgent>();

        _agent.speed = _walkSpeed;
        _agent.stoppingDistance = _attackRadius;
        _agent.autoBraking = false;

        //randomize the starting wander time so that the AI don't all wander at the same time
        _wanderTime = Random.Range(1f, 5f);
        _idleTimerRunning = true;
        
    }

    public void Update()
    {
        if (!isServer) return;

        if (_isDead)
        {
            _aiState = AIState.Dead;
            return;
        }

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
                    _aiState = AIState.Chase;
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
        _agent.isStopped = true;
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
        if (_target == null)
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
                // attack here
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
        WanderAroundRadius(_wanderRadius, transform.position);
        yield return new WaitForSeconds(_wanderTime);
        _isWandering = false;
    }

    IEnumerator IdleTimer()
    {
        _idleTimerRunning = true;
        yield return new WaitForSeconds(_idleTime);
        _aiState = AIState.Wander;
        _idleTimerRunning = false;
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

    void OnTargetChanged(NetworkIdentity oldIdentity, NetworkIdentity newIdentity)
    {
        // handle the target change
        _target = newIdentity?.transform;
    }


    void OnAIStateChanged(AIState oldState, AIState newState)
    {
        // handle the state change
        _aiState = newState;
    }

    void OnHealthChanged(float oldHealth, float newHealth)
    {
        _currentHealth = newHealth;
        _healthComponent.currentHealth = _currentHealth;
    }

    //coroutine to check for enemies nearby every x seconds
    IEnumerator CheckForEnemies(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            Collider[] colliders = Physics.OverlapSphere(transform.position, _dangerRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    _target = collider.transform;
                    _aiState = AIState.Chase;
                    break;
                }
            }
        }
    }

    //method to return closest transform from a list of transforms
    Transform GetClosestTransform(List<Transform> transforms)
    {
        Transform closestTransform = null;
        float closestDistance = Mathf.Infinity;
        foreach (Transform transform in transforms)
        {
            float distance = Vector3.Distance(transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTransform = transform;
            }
        }
        return closestTransform;
    }

    //method to knockback this object
    public void Knockback(Vector3 direction, float force)
    {
        _agent.isStopped = true;
        _agent.velocity = direction * force;
        StartCoroutine(StopKnockback());
    }
    public IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.5f);
        _agent.isStopped = false;


    }

    public void Interact(PlayerCharacter interactor)
    {
        CmdTakeDamage(interactor.netIdentity, 10);
    }

    [Command(requiresAuthority = false)]
    public void CmdTakeDamage(NetworkIdentity id, int damage)
    {
        Debug.Log("damaged by " + id);
        _currentHealth -= damage;
        _healthComponent.TakeDamage(damage);
        if (_healthComponent.currentHealth <= 0)
        {
            _isDead = true;
            _agent.isStopped = true;
            _aiState = AIState.Dead;
        }
    }

    public void Highlight(PlayerCharacter interactor)
    {
        Debug.Log("highlighted");
    }

    public void UnHighlight(PlayerCharacter interactor)
    {
        Debug.Log("unhighlighted");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight(null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnHighlight(null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

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
