using Mirror;
using UnityEngine;

public class BaseChoppableObject : BaseInteractableObject
{
    [SyncVar(hook = nameof(OnChopCountChanged))]
    private int _chopCount = 0;
    public int _chopCountMax = 3;
    public float _chopTime = 1f;
    public float _chopTimeMax = 1f;

    public float _chopDistance = 2f;

    [Command(requiresAuthority = false)]
    public void CmdChop(NetworkIdentity identity)
    {
        PlayerCharacter interactor = identity.GetComponent<PlayerCharacter>();


        Debug.Log(interactor.name + " chopping " + gameObject.name);
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
            interactor.SetDestination(transform.position, () => CmdChop(interactor.netIdentity));
    }

    // This function is called automatically on all clients when chopCount is changed on the server.
    private void OnChopCountChanged(int oldChopCount, int newChopCount)
    {
        Debug.Log("Chop count changed from " + oldChopCount + " to " + newChopCount);
        //set sprite color based on chop count

    }

}
