using System.Threading.Tasks;

namespace VideoPlayerProima.Interface
{
    public interface IDirectoyPicker
    {
        /* -- Android
         *  private void UploadBrowse_Click(object sender, EventArgs e)
        *  {
        *   _Uploadintent = new Intent(Intent.ActionGetContent);
        *  _Uploadintent.SetType("file/*");
        *  _Uploadintent.SetAction(Intent.ActionGetContent);
        *  StartActivityForResult(Intent.CreateChooser(_Uploadintent, "Select Picture"), 1);
        *  }
          
         */
        //https://stackoverflow.com/questions/49809468/how-to-open-a-folder-by-browsing-and-display-the-name-of-the-selected-item-in-xa

        //Intent intent = new Intent(Intent.ActionGetContent);
        //intent.SetType("*/*");
        //intent.AddCategory(Intent.CategoryOpenable);
        //StartActivityForResult(intent, 1);
        Task<string> OpenSelectFolderAsync();

        string GetStorageFolderPath();
    }
}
