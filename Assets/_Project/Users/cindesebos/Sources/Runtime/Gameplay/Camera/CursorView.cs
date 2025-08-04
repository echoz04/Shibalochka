using UnityEngine;

namespace Sources.Runtime.Gameplay.Camera
{
    public class CursorView
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
