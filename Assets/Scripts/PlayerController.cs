using Mirror;
using UnityEngine;
// PlayerController script
public class PlayerController : NetworkBehaviour
{
    [SyncVar]
    public GameObject _playerCharacter;

    // On start, if this is the local player, spawn the player character
    public override void OnStartLocalPlayer()
    {
        CmdSpawnPlayerCharacter();
    }

    // Command to spawn the player character
    [Command]
    private void CmdSpawnPlayerCharacter()
    {
        // Instantiate the player character and set it as a child of this controller
        _playerCharacter = Instantiate(NetworkManager.singleton.spawnPrefabs[0], transform.position, Quaternion.identity);
        NetworkServer.Spawn(_playerCharacter, connectionToClient);

        // Set the player character on all clients
        RpcSetPlayerCharacter(_playerCharacter);
    }

    // ClientRpc to set the player character on all clients
    [ClientRpc]
    private void RpcSetPlayerCharacter(GameObject playerCharacter)
    {
        _playerCharacter = playerCharacter;
    }

    private void Update()
    {
        if (!isLocalPlayer || _playerCharacter == null)
            return;

        // Handle input and send commands to the player character here...
    }

    public GameObject GetPlayerCharacter()
    {
        return _playerCharacter;
    }
}
