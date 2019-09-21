
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using VideoPlayerProima.Model;

namespace VideoPlayerProima.Helpers
{
    /// <summary>
    /// DownloadHelper
    /// </summary>
    public static class DownloadHelper
    {
        /// <summary>
        /// BufferSize
        /// </summary>
        public static readonly int BufferSize = 2048;

        /// <summary>
        /// DownloadTask
        /// </summary>
        /// <param name="urlToDownload"></param>
        /// <param name="pathToSave"></param>
        /// <param name="progessReporter"></param>
        /// <returns></returns>
        public static Task<int> CreateDownloadTask(string urlToDownload, string pathToSave, IProgress<DownloadBytesProgress> progessReporter)
        {
            return Task.Run<int>(async () =>
              {
                  int receivedBytes = 0;
                  int totalBytes = 0;
                  using (WebClient client = new WebClient())
                  {
                      using (var stream = await client.OpenReadTaskAsync(urlToDownload))
                      {
                          byte[] buffer = new byte[BufferSize];
                          totalBytes = Int32.Parse(client.ResponseHeaders[HttpResponseHeader.ContentLength]);
                          using (MemoryStream ms = new MemoryStream())
                          {
                              while (true)
                              {
                                  int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                                  ms.Write(buffer, 0, bytesRead);
                                  if (bytesRead == 0)
                                  {
                                      await Task.Yield();
                                      break;
                                  }

                                  receivedBytes += bytesRead;
                                  if (progessReporter != null)
                                  {
                                      progessReporter.Report(new DownloadBytesProgress(urlToDownload, receivedBytes, totalBytes));
                                  }
                              }

                              using (FileStream file = new FileStream(pathToSave, FileMode.Create, System.IO.FileAccess.Write))
                                  ms.CopyTo(file);
                          }
                      }
                  }
                  return receivedBytes;
              });
        }
    }
}