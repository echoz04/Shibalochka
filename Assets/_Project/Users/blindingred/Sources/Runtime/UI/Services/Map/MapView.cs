using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Sources.UI.Services
{
    public class MapView : UIViewBase
    {
        [SerializeField] private AlphaTweenButton _shopIslandButton;
        [SerializeField] private AlphaTweenButton _tropicalIslandButton;
        [SerializeField] private AlphaTweenButton _winterIslandButton;
        
        [SerializeField] private AlphaTweenButton _goToIslandButton;

        public readonly string ShopIslandKey = "Shop";
        public readonly string TropicalIslandKey = "Tropical";
        public readonly string WinterIslandKey = "Winter";
        public readonly string GoToIslandKey = "GoTo";

        [Inject]
        private void Construct()
        {
            _actions = new Dictionary<string, Action>
            {
                {ShopIslandKey, null},
                {TropicalIslandKey, null},
                {WinterIslandKey, null},
                {GoToIslandKey, null}
            };
        }

        public override void Show(Action onComplete = null)
        {
            Listen();
            base.Show(onComplete);
        }

        public override void Hide(Action onComplete = null)
        {
            Unlisten();
            base.Hide(onComplete);
        }

        private void Listen()
        {
            _shopIslandButton.AddListener(() => _actions[ShopIslandKey].Invoke());
            _tropicalIslandButton.AddListener(() => _actions[TropicalIslandKey].Invoke());
            _winterIslandButton.AddListener(() => _actions[WinterIslandKey].Invoke());
            _goToIslandButton.AddListener(() => _actions[GoToIslandKey].Invoke());
        }

        private void Unlisten()
        {
            _shopIslandButton.RemoveAllListeners();
            _tropicalIslandButton.RemoveAllListeners();
            _winterIslandButton.RemoveAllListeners();
            _goToIslandButton.RemoveAllListeners();
        }
    }
}