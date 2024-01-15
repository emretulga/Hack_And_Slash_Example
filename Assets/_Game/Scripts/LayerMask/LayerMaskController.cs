using UnityEngine;

namespace EmreTulga.HackAndSlash_Example.LayerMasks
{
    public class LayerMaskController : MonoBehaviour
    {
        #region STATICS && CONSTS
        public static LayerMaskController Instance;
        #endregion

        #region VARIABLES
        public LayerMask EnemyLayerMask;
        #endregion

        #region FUNCTIONS
        private void Awake()
        {
            Instance = this;
        }
        #endregion
    }
}