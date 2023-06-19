using Mirror;
using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacter : NetworkBehaviour
{
    public CharacterController _characterController;
    public float _speed = 5f;
    public float turnSpeed = 1f;
    public PlayerController _playerController;
    public bool isLocalCharacter = false;

    public NavMeshAgent agent;

    public Action onArrived;

    private Vector3 destination;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        gameObject.name = "LocalPlayerCharacter";
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                // Arrived
                onArrived?.Invoke();
                onArrived = null; // Clear out the action once it's been invoked
            }
        }
    }

    public virtual void HandleInput()
    {
        //left click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //check if we hit an interactable object
                if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
                {
                   // Vector3 nearestPoint = GetNearestPointOnNavMesh(hit.point);
                   // PositionMarker.Instance.SetNewPosition(nearestPoint);
                    interactable.Interact(this);
                }
            }
        }

        //right click
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //Debug.Log("Hit: " + hit.collider.gameObject.name);
                //check if we hit the ground
                if (hit.collider.CompareTag("Floor"))
                {
                    //set position marker
                    PositionMarker.Instance.SetNewPosition(hit.point);
                    //check if hit is on a navmesh
                    var nearestPoint = GetNearestPointOnNavMesh(hit.point);
                    agent.SetDestination(nearestPoint);
                }




            }
        }
    }

    public void SetDestination(Vector3 destination, Action onArrived = null)
    {
        var nearestPoint = GetNearestPointOnNavMesh(destination);
        agent.SetDestination(nearestPoint);
        this.onArrived = onArrived;
        this.destination = nearestPoint;
        //spawn a sphere at the nearest point
       // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
       // sphere.transform.position = nearestPoint;
    }


    //get closest point on navmesh to a given point
    public Vector3 GetNearestPointOnNavMesh(Vector3 position)
    {
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(position, out navMeshHit, 1f, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }
        else
        {
            if (NavMesh.SamplePosition(position, out navMeshHit, 100f, NavMesh.AllAreas))
            {
                return navMeshHit.position;
            }
        }
        return position;
    }

    public PlayerController GetPlayerController()
    {
        return _playerController;
    }

    public void SetPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
    }
}
