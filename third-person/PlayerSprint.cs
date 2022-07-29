using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerSprint : MonoBehaviour
{
  [SerializeField] float speedMultipler = 2f;

  Player player;
  PlayerInput playerInput;
  InputAction sprintAction;

  void Awake()
  {
    player = GetComponent<Player>();
    playerInput = GetComponent<PlayerInput>();
    sprintAction = playerInput.actions["sprint"];
  }

  void OnEnable() => player.OnBeforeMove += OnBeforeMove;
  void OnDisable() => player.OnBeforeMove -= OnBeforeMove;

  void OnBeforeMove()
  {
    var sprintInput = sprintAction.ReadValue<float>();
    if (sprintInput == 0) return;
    var forwardMovementFactor = Mathf.Clamp01(
      Vector3.Dot(player.transform.forward, player.velocity.normalized)
    );
    var multipler = Mathf.Lerp(1f, speedMultipler, forwardMovementFactor);  
    player.movementSpeedMultipler *= multipler;
  }
}
