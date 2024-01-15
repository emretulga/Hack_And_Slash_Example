using UnityEngine;

namespace EmreTulga.HackAndSlash_Example.Core
{
    public class PlayerController : MonoBehaviour
    {
        #region STATICS && CONSTS
        public static PlayerController Instance;

        public const float CHARACTER_ROTATING_SPEED = 20f;
        public const float CHECK_LANDING_RAY_DISTANCE = 0.2f;
        public const float GRAVITY_SCALER = 0.1f;
        public const float GRAVITY_FALLING_SCALER = 0.02f;

        public const float JUMP_FORCE_ACCELERATION = 0.01f;
        #endregion

        #region VARIABLES
        [SerializeField] private Camera _currentCamera;

        [SerializeField] private CharacterController _characterController;

        [SerializeField] private Animator _characterAnimator;

        [SerializeField] private float _movementSpeed, _jumpForce;

        [SerializeField] private Combo[] _mouse0Combos;
        
        private int _comboCounter = 0;

        private Vector3 _gravityForce = Vector3.zero;
        private Vector3 _appearedPosition;

        private bool _isJumped, _isFighting;
        #endregion

        #region FUNCTIONS
        private void Awake()
        {
            Instance = this;
        }
        private void Update()
        {
            ComboDecision();
            Fight();
            Movement();
            Gravity();
            Jump();
        }
        private void Movement()
        {
            if(_isFighting)
            {
                Vector3 motion = Vector3.Lerp(transform.position, _appearedPosition, Time.deltaTime * _mouse0Combos[_comboCounter].appearspeed);
                
                _characterController.Move(motion - transform.position);
                
                return;
            }

            Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            Vector3 cameraFixedForward = new Vector3(_currentCamera.transform.forward.x, 0f, _currentCamera.transform.forward.z);
            Vector3 cameraFixedRight = new Vector3(_currentCamera.transform.right.x, 0f, _currentCamera.transform.right.z); 
            Vector3 horizontalRightMovement =  movementInput.x * _movementSpeed * Time.deltaTime * cameraFixedRight;
            Vector3 horizontalForwardMovement =  movementInput.z * _movementSpeed * Time.deltaTime * cameraFixedForward;
            Vector3 movement = horizontalForwardMovement + horizontalRightMovement;

            _characterController.Move(movement);

            Vector3 normalizedMovement = Vector3.Normalize(movement);

            bool isMoving = normalizedMovement.magnitude != 0f;

            if(!isMoving)
            {
                _characterAnimator.SetBool("isRunning", false);

                return;
            }

            _characterAnimator.SetBool("isRunning", true);

            ChangeCharacterForwardSmoothly(normalizedMovement);
        }
        private void ChangeCharacterForwardSmoothly(Vector3 direction)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * CHARACTER_ROTATING_SPEED);
        }

        public void ChangeCharacterForwardWithCamera()
        {
            Vector3 cameraFixedForward = new Vector3(_currentCamera.transform.forward.x, 0f, _currentCamera.transform.forward.z);

            ChangeCharacterForward(cameraFixedForward);
        }

        public void ChangeCharacterForward(Vector3 direction)
        {
            transform.forward = direction;
        }

        private void Gravity()
        {
            Vector3 checkLandingOrigin = new Vector3(0f, 0.1f, 0f);

            RaycastHit hit;

            bool isLanding = !Physics.Raycast(transform.position + checkLandingOrigin, transform.TransformDirection(Vector3.down), out hit, CHECK_LANDING_RAY_DISTANCE);
            
            _characterAnimator.SetBool("isFalling", isLanding);

            if(_characterController.isGrounded)
            {
                _gravityForce = Vector3.zero;
                _isJumped = false;
                return;
            }

            if(_isJumped)
            {
                _gravityForce -= Time.deltaTime * Physics.gravity;

                _isJumped = _gravityForce.y < _jumpForce;
            }
            else
            {
                float gravityScaler = GRAVITY_SCALER;

                bool isFalling = _gravityForce.y <= 0f;

                if(isFalling)
                {
                    gravityScaler = GRAVITY_FALLING_SCALER;
                }


                _gravityForce += Physics.gravity * Time.deltaTime * gravityScaler;
            }

            _characterController.Move(_gravityForce);
        }
        private void Jump()
        {
            if(_isFighting)
            {
                return;
            }

            if(_isJumped || !Input.GetKeyDown(KeyCode.Space) || !_characterController.isGrounded)
            {
                return;
            }

            _gravityForce = _jumpForce * JUMP_FORCE_ACCELERATION * -Physics.gravity;

            _isJumped = true;
        }
        private void Fight()
        {
            bool hasLeastOneCombo = _mouse0Combos.Length > 0;

            if(_isFighting || !Input.GetMouseButtonDown(0) || !hasLeastOneCombo)
            {
                return;
            }

            ChangeCharacterForwardWithCamera();

            Appear();

            _isFighting = true;

            _comboCounter = 0;

            _characterAnimator.SetInteger("comboID", _mouse0Combos[_comboCounter].comboid);
            _characterAnimator.SetBool("isFighting", true);
        }
        private void ComboDecision()
        {
            bool isComboExist = _comboCounter + 1 < _mouse0Combos.Length;
            
            if(!_isFighting || !Input.GetMouseButtonDown(0) || !isComboExist || _mouse0Combos[_comboCounter].comboid != _characterAnimator.GetInteger("comboID"))
            {
                return;
            }

            _characterAnimator.SetInteger("comboID", _mouse0Combos[_comboCounter + 1].comboid);
        }
        public void IncreaseComboCounter()
        {
            _comboCounter++;
        }
        public void StopFighting()
        {
            _isFighting = false;

            _comboCounter = 0;

            _characterAnimator.SetBool("isFighting", false);
            _characterAnimator.SetInteger("comboID", _mouse0Combos[_comboCounter].comboid);
        }
        public void Appear()
        {
            _appearedPosition = _mouse0Combos[_comboCounter].appearrange * transform.forward + transform.position;
        }
        public void HitEnemy(EnemyController enemy)
        {
            Vector3 hitPosition = transform.position;

            float appearRange = _mouse0Combos[_comboCounter].appearrange;
            float appearSpeed = _mouse0Combos[_comboCounter].appearspeed;

            bool hitToFall = _mouse0Combos[_comboCounter].hitToFall;

            enemy.GetHit(hitPosition, appearRange, appearSpeed, hitToFall);
        }
        public Transform GetHitTransform()
        {
            return _mouse0Combos[_comboCounter].hitCenter;
        }
        public float GetHitRange()
        {
            return _mouse0Combos[_comboCounter].hitrange;
        }
        #endregion
    }
}