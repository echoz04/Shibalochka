using System;
using VContainer;

namespace Sources.UI.Services
{
    public class FadeService : IService
    {
        private FadeView _view;

        public IUIView View { get; }

        [Inject]
        private void Construct(
            FadeView fadeView)
        {
            _view = fadeView;
        }

        public void FadeIn(Action callback = null)
        {
            _view.FadeIn(() => callback?.Invoke());
        }

        public void FadeOut(Action callback = null)
        {
            _view.FadeOut(()=> callback?.Invoke());
        }
    }
}