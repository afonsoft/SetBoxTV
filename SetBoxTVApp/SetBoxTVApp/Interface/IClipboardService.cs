namespace SetBoxTVApp.Interface
{
    public interface IClipboardService
    {
        string GetTextFromClipboard();
        void SendTextToClipboard(string text);
    }
}
