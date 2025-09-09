using System.Collections.Generic;
using UnityEngine;

namespace Sources
{
    public abstract class UIScreenBase : MonoBehaviour, IUIScreen
    {
        protected List<IUIService> UIServices = new();
        
        public virtual void Open()
        {
            // Debug.Log($"Open {gameObject.name}");
            gameObject.SetActive(true);
            
            foreach (var uiService in UIServices)
            {
                uiService?.Enable();
            }
        }

        public virtual void Close()
        {
            // Debug.Log($"Close {gameObject.name}");
            gameObject.SetActive(false);
            
            foreach (var uiService in UIServices)
            {
                uiService?.Disable();
            }
        }
    }
}