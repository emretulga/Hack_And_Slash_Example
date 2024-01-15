using UnityEngine;

namespace EmreTulga.HackAndSlash_Example.Core
{
    public class EnemyController : MonoBehaviour
    {
        #region VARIABLES
        [SerializeField] private CharacterController _characterController;

        [SerializeField] private Animator animator;

        [SerializeField] private float _movementSpeed;
        
        private bool _gotHit;

        private Vector3 _appearedPosition;

        private float _hitAppearSpeed;
        #endregion

        #region FUNCTIONS
        private void Update()
        {
            FollowPlayer();
        }
        private void FollowPlayer()
        {
            if(_gotHit)
            {
                Vector3 motion = Vector3.Lerp(transform.position, _appearedPosition, Time.deltaTime * _hitAppearSpeed);
                
                _characterController.Move(motion - transform.position);
                
                return;
            }
            
            Vector3 unhandledDirection = PlayerController.Instance.transform.position - transform.position;
            Vector3 direction = new Vector3(unhandledDirection.x, 0f, unhandledDirection.z).normalized;
            Vector3 movement = _movementSpeed * Time.deltaTime * direction;

            _characterController.Move(movement);
            
            transform.forward = direction;

            animator.SetBool("running", true);
        }
        public void GetHit(Vector3 hitPosition, float appearRange, float appearSpeed, bool hitToFall)
        {
            if(_gotHit)
            {
                animator.Play("GotHit", -1, 0f);
            }

            _gotHit = true;

            Vector3 unhandledAppearDirection = (transform.position - hitPosition).normalized;
            Vector3 appearDirection = new Vector3(unhandledAppearDirection.x, 0f, unhandledAppearDirection.z);

            transform.forward = -appearDirection;

            _appearedPosition = appearRange * appearDirection + transform.position;

            _hitAppearSpeed = appearSpeed;

            animator.SetBool("gotHit", true);
            animator.SetBool("fall", hitToFall);
        }
        public void ReleaseFromHit()
        {
            _gotHit = false;
            animator.SetBool("gotHit", false);
        }
        public void Die()
        {
            Destroy(_characterController);
            Destroy(this);
        }
        #endregion
    }
}