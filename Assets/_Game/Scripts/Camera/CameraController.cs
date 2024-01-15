using UnityEngine;

namespace EmreTulga.HackAndSlash_Example.Core
{
    public class CameraController : MonoBehaviour
    {
        #region STATICS & CONSTS
        public const float MOUSE_X_SENSE = 10f;
        public const float MOUSE_Y_SENSE = 0.1f;
        public const float VERTICAL_MIN_ROTATION_RATIO = 0f;
        public const float VERTICAL_MAX_ROTATION_RATIO = 1f;
        public const float VERTICAL_ROTATION_RATIO_RANGE = 10f;
        #endregion

        #region VARIABLES
        [SerializeField] private Transform _followObjectTransform;

        [SerializeField] private float _followSpeed;

        private Vector3 _followOffset, _rotationOffset;

        private float _verticalRotationRatio = VERTICAL_MAX_ROTATION_RATIO / 2f;
        #endregion

        #region FUNCTIONS
        private void Awake()
        {
            _rotationOffset = transform.eulerAngles;

            SetDistanceWithFollowObject();
        }
        private void Start()
        {
            LockAndHideCursor();
        }
        private void LateUpdate()
        {
            FollowObject();
            RotateCamera();
        }
        private void SetDistanceWithFollowObject()
        {
            _followOffset = transform.position - _followObjectTransform.position;
        }
        private void FollowObject()
        {
            Vector3 followingPosition = _followObjectTransform.position + _followOffset;

            transform.position = followingPosition;
        }
        private void RotateCamera()
        {
            float inputMouseX = Input.GetAxis("Mouse X") * MOUSE_X_SENSE;
            float inputMouseY = Input.GetAxis("Mouse Y") * MOUSE_Y_SENSE;

            transform.RotateAround(_followObjectTransform.position, Vector3.up, inputMouseX);

            float unclampedRatio = _verticalRotationRatio - inputMouseY;
            _verticalRotationRatio = Mathf.Clamp(unclampedRatio, VERTICAL_MIN_ROTATION_RATIO, VERTICAL_MAX_ROTATION_RATIO);

            transform.eulerAngles = new Vector3(_rotationOffset.x + _verticalRotationRatio * VERTICAL_ROTATION_RATIO_RANGE, transform.eulerAngles.y, transform.eulerAngles.z);

            SetDistanceWithFollowObject();
        }
        public void LockAndHideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        public void UnlockAndRevealCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        #endregion
    }
}