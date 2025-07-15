using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

namespace Sources.Runtime.Gameplay.Camera
{
    public class CursorView : MonoBehaviour
    {
        public void Show()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Hide()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
