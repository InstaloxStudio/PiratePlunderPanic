using Mirror;
using System;
using UnityEngine;
// PlayerController script
public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private PlayerCharacter _playerCharacter;


    public Inventory inventory = new Inventory();

    public bool canProcessInput = true;

    //event for when character is spawned
    public Action<PlayerCharacter> OnCharacterSpawned;


    // On start, if this is the local player, spawn the player character
    public override void OnStartLocalPlayer()
    {
        //rename gameobject to LocalPlayerController
        gameObject.name = "LocalPlayerController";
        CmdSpawnPlayerCharacter();
    }

    [TargetRpc]
    private void TargetAssignPlayerCharacter(NetworkConnection target, GameObject playerCharacter)
    {
        _playerCharacter = playerCharacter.GetComponent<PlayerCharacter>();
        CameraManager.instance.SetVCamFollowTarget(_playerCharacter.transform);
        CameraManager.instance.SetVCamLookAtTarget(_playerCharacter.transform);
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //check to make sure our _playerCharacter is not null
        if (_playerCharacter == null)
        {
            Debug.Log("PlayerCharacter is null");
            return;
        }

        if (canProcessInput)
            _playerCharacter.HandleInput();
    }

    public PlayerCharacter GetPlayerCharacter()
    {
        return _playerCharacter;
    }

    public void EnableInput()
    {
        canProcessInput = true;
    }

    public void DisableInput()
    {
        canProcessInput = false;
    }

    public void PosessCharacter(PlayerCharacter character)
    {
        _playerCharacter = character;
        _playerCharacter._playerController = this;
        _playerCharacter.isLocalCharacter = isLocalPlayer;
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
        TargetAssignPlayerCharacter(connectionToClient, prefab);
        OnCharacterSpawned?.Invoke(_playerCharacter);


    }

    [Command]
    public void CmdPosessCharacter(GameObject character)
    {
        var _character = character.GetComponent<PlayerCharacter>();
        _playerCharacter = _character;
        _playerCharacter._playerController = this;
        _playerCharacter.isLocalCharacter = isLocalPlayer;
        TargetAssignPlayerCharacter(connectionToClient, _playerCharacter.gameObject);
    }
}
