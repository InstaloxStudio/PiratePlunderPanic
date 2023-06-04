using Mirror;
using UnityEngine;


public class PlayerCharacter : NetworkBehaviour
{
    public Animator _animator;
    public CharacterController _characterController;
    public float _speed = 5f;
    public float turnSpeed = 10f;
    public PlayerController _playerController;
    public bool isLocalCharacter = false;
    public bool useAnimator = false;
    private void Start()
    {
        if (useAnimator)
            _animator = GetComponent<Animator>();

        _characterController = GetComponent<CharacterController>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        gameObject.name = "LocalPlayerCharacter";
    }

    public void HandleInput(float horizontal, float vertical)
    {
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement = Vector3.ClampMagnitude(movement, 1f);
        movement = transform.TransformDirection(movement);
        movement *= _speed;

        _characterController.Move(movement * Time.deltaTime);

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
            if (useAnimator)
                _animator.SetBool("isRunning", true);
        }
        else
        {
            if (useAnimator)
                _animator.SetBool("isRunning", false);
        }
    }
}