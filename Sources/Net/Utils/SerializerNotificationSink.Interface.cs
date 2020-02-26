namespace Advexp
{
    internal interface ISerializerNotificationSink
    {
        void OnStartSerializerAction();
        void OnEndSerializerAction();

        void OnStartLoadSettings();
        void OnEndLoadSettings();

        void OnStartSaveSettings();
        void OnEndSaveSettings();

        void OnStartDeleteSettings();
        void OnEndDeleteSettings();
    }
}
