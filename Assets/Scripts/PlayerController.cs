using Mirror;
using UnityEngine;
// PlayerController script
public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private PlayerCharacter _playerCharacter;

    // On start, if this is the local player, spawn the player character
    public override void OnStartLocalPlayer()
    {
        //rename gameobject to LocalPlayerController
        gameObject.name = "LocalPlayerController";


        CmdSpawnPlayerCharacter();
    }

    // Command to spawn the player character
    [Command]
    private void CmdSpawnPlayerCharacter()
    {
        //Instantiate the player character and set it as a child of this controller
        var prefab = Instantiate(NetworkManager.singleton.spawnPrefabs[0], transform.position, Quaternion.identity);




        _playerCharacter = prefab.GetComponent<PlayerCharacter>();
        _playerCharacter._playerController = this;
        _playerCharacter.isLocalCharacter = isLocalPlayer;

        // Spawn the prefab on the server
        NetworkServer.Spawn(prefab, connectionToClient);
        //make sure to assign the player character to the client that spawned it
       // TargetAssignPlayerCharacter(connectionToClient, prefab);
    }

    [TargetRpc]
    private void TargetAssignPlayerCharacter(NetworkConnection target, GameObject playerCharacter)
    {
        _playerCharacter = playerCharacter.GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // Handle input and send commands to the player character here...
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //check to make sure our _playerCharacter is not null
        if (_playerCharacter == null)
        {
            Debug.LogError("PlayerCharacter is null");
            return;
        }

        _playerCharacter.HandleInput(horizontal, vertical);
    }

    public PlayerCharacter GetPlayerCharacter()
    {
        return _playerCharacter;
    }
}
