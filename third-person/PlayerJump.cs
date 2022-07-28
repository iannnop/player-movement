using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerJump : MonoBehaviour
{
  [SerializeField] float jumpSpeed = 5f;
  [SerializeField] float jumpPressBufferTime = .05f;
  [SerializeField] float jumpGroundGraceTime = .2f;
  [SerializeField] int maxJumps = 1;

  Player player;

  bool tryingToJump;
  float lastJumpPressTime;
  float lastGroundedTime;
  int jumps;

  void Awake()
  {
    player = GetComponent<Player>();
  }


  void OnEnable()
  {
    player.OnBeforeMove += OnBeforeMove;
    player.OnGroundStateChange += OnGroundStateChange;
  }

  void OnDisable()
  {
    player.OnBeforeMove -= OnBeforeMove;
    player.OnGroundStateChange -= OnGroundStateChange;
  }

  void OnJump()
  {
    tryingToJump = true;
    lastJumpPressTime = Time.time;
  }

  void OnBeforeMove()
  {
    if (player.IsGrounded) jumps = 0;

    var wasTryingToJump = Time.time - lastJumpPressTime < jumpPressBufferTime;
    var wasGrounded = Time.time - lastGroundedTime < jumpGroundGraceTime;

    var isOrWasTryingToJump = tryingToJump || (wasTryingToJump && player.IsGrounded);
    var isOrWasGrounded = player.IsGrounded || wasGrounded;
    var jumpAllowed = jumps < maxJumps;

    if (
      jumpAllowed && isOrWasTryingToJump && isOrWasGrounded
      || jumpAllowed && tryingToJump
    )
    {
      player.velocity.y += jumpSpeed;
      jumps++;
    }

    tryingToJump = false;
  }

  void OnGroundStateChange(bool isGrounded)
  {
    if (!isGrounded) lastGroundedTime = Time.time;
  }
}