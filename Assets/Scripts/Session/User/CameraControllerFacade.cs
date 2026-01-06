using UnityEngine;

namespace Session.User
{
    public class CameraControllerFacade : MonoBehaviour
    {
        [SerializeField] private Camera _ñamera;

        public Camera Camera
        {
            get
            {
                if (_ñamera == null)
                    _ñamera = GetComponent<Camera>();
                return _ñamera;
            }
        }
    }
}