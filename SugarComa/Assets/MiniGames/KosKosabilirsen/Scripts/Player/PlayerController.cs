using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.MiniGames.KosKosabilirsen.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        //[Space(10)]
        //[Tooltip("The height the player can jump")]
        //public float JumpHeight = 1.2f;

        //[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        //public float Gravity = -9.81f;

        //[Space(10)]
        //[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        //public float JumpTimeout = 0.50f;

        //[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        //public float FallTimeout = 0.15f;

        //[Header("Player Grounded")]
        //[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        //public bool Grounded = true;

        //[Tooltip("Useful for rough ground")]
        //public float GroundedOffset = -0.14f;

        //[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        //public float GroundedRadius = 0.28f;

        //[Tooltip("What layers the character uses as ground")]
        //public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerSpecs _playerSpec;
        //[SerializeField] private Animator _animator;
        //[SerializeField] private CharacterController _controller;
        [SerializeField] private PlayerAnimation _playerAnimation;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private PlayerInputReceiver _input;
        [SerializeField] private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool IsCurrentDeviceMouse
        {
            get
            {
                return _playerInput.currentControlScheme == "KeyboardMouse";
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            //_controller = GetComponent<CharacterController>();
            //_input = GetComponent<PlayerInputReceiver>();
            //_playerInput = GetComponent<PlayerInput>();


            // reset our timeouts on start
            //_jumpTimeoutDelta = JumpTimeout;
            //_fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            //JumpAndGravity();
            //GroundedCheck();
            Move();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        // Did not delete because of hooking action.
        //private void GroundedCheck()
        //{
        //    // set sphere position, with offset
        //    Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
        //        transform.position.z);
        //    Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
        //        QueryTriggerInteraction.Ignore);

        //    // update animator if using character
        //    //TODO Landing anim
        //}

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _playerSpec.IsSlow ? _playerSpec.PlayerSlowSpeed : _playerSpec.PlayerMoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_rigidbody.velocity.x, 0.0f, _rigidbody.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * _playerSpec.SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * _playerSpec.SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    _playerSpec.PlayerRotationSpeed);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            transform.Translate(targetDirection.normalized * _speed * Time.deltaTime * 10f +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime, Space.World);

            // update animator if using character
            AnimateCharacter(_speed);
        }

        private void AnimateCharacter(float speed)
        {
            if(speed > 0)
            {
                _playerAnimation.StartRunning();
            }
            else
            {
                _playerAnimation.StopRunning();
            }
        }
        //Will change to hook action
        //private void JumpAndGravity()
        //{
        //    if (Grounded)
        //    {
        //        // reset the fall timeout timer
        //        _fallTimeoutDelta = FallTimeout;

        //        // update animator if using character


        //        // stop our velocity dropping infinitely when grounded
        //        if (_verticalVelocity < 0.0f)
        //        {
        //            _verticalVelocity = -2f;
        //        }

        //        // Jump
        //        if (_input.jump && _jumpTimeoutDelta <= 0.0f)
        //        {
        //            // the square root of H * -2 * G = how much velocity needed to reach desired height
        //            _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

        //        }

        //        // jump timeout
        //        if (_jumpTimeoutDelta >= 0.0f)
        //        {
        //            _jumpTimeoutDelta -= Time.deltaTime;
        //        }
        //    }
        //    else
        //    {
        //        // reset the jump timeout timer
        //        _jumpTimeoutDelta = JumpTimeout;

        //        // fall timeout
        //        if (_fallTimeoutDelta >= 0.0f)
        //        {
        //            _fallTimeoutDelta -= Time.deltaTime;
        //        }
        //        else
        //        {
        //            // update animator if using character
        //        }

        //        // if we are not grounded, do not jump
        //        _input.jump = false;
        //    }

        //    // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        //    if (_verticalVelocity < _terminalVelocity)
        //    {
        //        _verticalVelocity += Gravity * Time.deltaTime;
        //    }
        //}

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        //private void OnDrawGizmosSelected()
        //{
        //    Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        //    Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        //    if (Grounded) Gizmos.color = transparentGreen;
        //    else Gizmos.color = transparentRed;

        //    // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        //    Gizmos.DrawSphere(
        //        new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
        //        GroundedRadius);
        //}

        //TODO IF SOUND ADD
        //private void OnFootstep(AnimationEvent animationEvent)
        //{
        //    if (animationEvent.animatorClipInfo.weight > 0.5f)
        //    {
        //        if (FootstepAudioClips.Length > 0)
        //        {
        //            var index = Random.Range(0, FootstepAudioClips.Length);
        //            AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
        //        }
        //    }
        //}

        //private void OnLand(AnimationEvent animationEvent)
        //{
        //    if (animationEvent.animatorClipInfo.weight > 0.5f)
        //    {
        //        AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        //    }
        //}
    }
}