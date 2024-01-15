using EmreTulga.HackAndSlash_Example.LayerMasks;
using UnityEngine;

namespace EmreTulga.HackAndSlash_Example.Core
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        #region VARIABLES
        private Animator playerAnimator;
        #endregion

        #region FUNCTIONS
        private void Awake()
        {
            playerAnimator = GetComponent<Animator>();

            if(!playerAnimator)
            {
                Destroy(this);
            }
        }
        public void StartOfCombo()
        {
            PlayerController.Instance.IncreaseComboCounter();
            PlayerController.Instance.ChangeCharacterForwardWithCamera();
            PlayerController.Instance.Appear();
        }
        public void HitCombo()
        {
            Vector3 hitPosition = PlayerController.Instance.GetHitTransform().position;

            float hitRange = PlayerController.Instance.GetHitRange();

            LayerMask hitLayer = LayerMaskController.Instance.EnemyLayerMask;

            Collider[] hitEnemies = Physics.OverlapSphere(hitPosition, hitRange, hitLayer);

            foreach (Collider hitEnemy in hitEnemies)
            {
                EnemyController enemy = hitEnemy.GetComponent<EnemyController>();

                PlayerController.Instance.HitEnemy(enemy);
            }
        }
        public void EndOfCombo()
        {
            PlayerController.Instance.StopFighting();
        }
        #endregion
    }
}