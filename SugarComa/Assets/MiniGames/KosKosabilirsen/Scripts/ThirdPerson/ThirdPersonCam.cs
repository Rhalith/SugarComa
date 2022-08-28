using UnityEngine;

namespace Assets.MiniGames.KosKosabilirsen.Scripts.ThirdPerson
{
    public class ThirdPersonCam : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _orientation;
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _playerObject;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private PlayerInputReceiver _playerInputReceiver;

        [Header("Cameras")]
        [SerializeField] private GameObject _combatLookAtObject;
        [SerializeField] private GameObject _thirdPersonCam;
        [SerializeField] private GameObject _combatCam;

        public float rotationSpeed;

        public CameraStyle currentStyle;

        public enum CameraStyle
        {
            Basic,
            Combat
        }

        void Update()
        {
            RotatePlayer(currentStyle);
        }

        public void ChangeCamera(CameraStyle style)
        {
            _thirdPersonCam.SetActive(false);
            _combatCam.SetActive(false);
            _combatLookAtObject.SetActive(false);

            switch (style)
            {
                case CameraStyle.Basic:
                    _thirdPersonCam.SetActive(true);
                    currentStyle = CameraStyle.Basic;
                    break;
                case CameraStyle.Combat:
                    _combatLookAtObject.SetActive(true);
                    _combatCam.SetActive(true);
                    currentStyle = CameraStyle.Combat;
                    break;
            }
        }

        private void RotatePlayer(CameraStyle style)
        {
            if (style.Equals(CameraStyle.Basic))
            {
                Vector3 viewDirection = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);
                _orientation.forward = viewDirection.normalized;

                Vector3 inputDirection = _orientation.forward * _playerInputReceiver.move.y + _orientation.right * _playerInputReceiver.move.x;

                if (inputDirection != Vector3.zero)
                {
                    _playerObject.forward = Vector3.Slerp(_playerObject.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
                }
            }
            else if (style.Equals(CameraStyle.Combat))
            {
                Vector3 combatDirection = _combatLookAtObject.transform.position - new Vector3(transform.position.x, _combatLookAtObject.transform.position.y, transform.position.z);
                _orientation.forward = combatDirection.normalized;

                _playerObject.forward = combatDirection.normalized;
            }

        }
    }
}