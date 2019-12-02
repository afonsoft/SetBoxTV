namespace SetBoxTV.VideoPlayer.Interface
{
    public interface IClipboardService
    {
        string GetTextFromClipboard();
        void SendTextToClipboard(string text);
    }
}
