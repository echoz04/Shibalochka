
namespace Sources
{
    public interface IUIService
    {
        IUIView View { get;}
        
        void Enable();
        void Disable();
    }
}
