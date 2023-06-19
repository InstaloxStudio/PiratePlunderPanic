using Mirror;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BaseChoppableObject : BaseInteractableObject
{
    [SyncVar(hook = nameof(OnChopCountChanged))]
    private int _chopCount = 0;
    public int _chopCountMax = 3;
    public float _chopTime = 1f;
    public float _chopTimeMax = 1f;

    public float _chopDistance = 2f;

    public Texture2D _mainTexture;

    private void Awake()
    {
        //get child object's material and set its texture to the main texture
        _material=GetComponentInChildren<MeshRenderer>().material;
        _originalColor=_material.color;
        _material.mainTexture = _mainTexture;

    }

    [Command(requiresAuthority = false)]
    public void CmdChop(NetworkIdentity identity)
    {
        PlayerCharacter interactor = identity.GetComponent<PlayerCharacter>();

       // Debug.Log(interactor.name + " chopping " + gameObject.name);
        _chopCount++;

        if (_chopCount >= _chopCountMax)
        {
            _chopCount = 0;
            _chopTime = 0;
            NetworkServer.Destroy(gameObject);
        }
        else
        {
            _chopTime = 0;
        }
    }

    public override void Interact(PlayerCharacter interactor)
    {
        base.Interact(interactor);

        //check distance to player
        float distance = Vector3.Distance(interactor.transform.position, transform.position);
        if (distance <= _chopDistance)
        {
            CmdChop(interactor.netIdentity);
        }
        else
            interactor.SetDestination(GetRandomPositionOnTheFarSideOfObject(), () => CmdChop(interactor.netIdentity));
    }

    // This function is called automatically on all clients when chopCount is changed on the server.
    private void OnChopCountChanged(int oldChopCount, int newChopCount)
    {
      //  Debug.Log("Chop count changed from " + oldChopCount + " to " + newChopCount);
        //set sprite color based on chop count

    }


    public Vector3 GetRandomPositionInRadius(float radius)
    {
        Vector3 randomPosition = Random.insideUnitSphere * radius;
        randomPosition += transform.position;
        //set the y position to the y position of the object
        randomPosition.y = transform.position.y;

        return randomPosition;
    }

    public Vector3 GetRandomPositionOnTheFarSideOfObject()
    {
        //get the camera from the camera manager
        Camera camera = CameraManager.instance.currentCamera;

        //get the direction from the camera to the object
        Vector3 direction = (transform.position - camera.transform.position).normalized;

        // Calculate the desired position by adding an offset along the opposite direction of the hit normal
        Vector3 desiredPosition = transform.position - -transform.forward * 1f;

        PositionMarker.Instance.SetNewPosition(desiredPosition);

        return desiredPosition;
    }
}
