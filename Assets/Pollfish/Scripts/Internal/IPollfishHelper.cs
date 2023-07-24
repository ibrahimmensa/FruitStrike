namespace PollfishUnity
{
    #if UNITY_ANDROID || UNITY_IPHONE

    public partial interface IPollfishHelper
    {
        void Init(Pollfish.Params pollfishParams);
        void Show();
        void Hide();
        bool IsPollfishPresent();
        bool IsPollfishPanelOpen();
        void ShouldQuit();
    }

    #endif
}
