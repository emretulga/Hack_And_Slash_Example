using UnityEngine;

namespace EmreTulga.HackAndSlash_Example.Core
{
    public class EnemyAnimatorController : MonoBehaviour
    {
        #region VARIABLES
        [SerializeField] private EnemyController enemy;
        #endregion

        #region FUNCTIONS
        public void ReturnFromHit()
        {
            enemy.ReleaseFromHit();
        }
        public void Die()
        {
            enemy.Die();
        }
        #endregion
    }
}