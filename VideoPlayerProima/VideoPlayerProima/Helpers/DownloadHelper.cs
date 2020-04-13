
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SetBoxTV.VideoPlayer.Model;

namespace SetBoxTV.VideoPlayer.Helpers
{
    /// <summary>
    /// DownloadHelper
    /// </summary>
    public static class DownloadHelper
    {
        /// <summary>
        /// BufferSize
        /// </summary>
        public static readonly int BufferSize = 4095;

        /// <summary>
        /// DownloadTask
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="pathToSave"></param>
        /// <param name="progessReporter"></param>
        /// <returns></returns>
        public static async Task CreateDownloadTask(string requestUri, string pathToSave, string name, IProgress<DownloadBytesProgress> progessReporter, CancellationToken token)
        {
            using (HttpClient _client = new HttpClient())
            {
                // Step 1 : Get call
                var response = await _client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(true);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                // Step 2 : Filename
                var fileName = response.Content.Headers?.ContentDisposition?.FileName ?? name;

                // Step 3 : Get total of data
                var totalBytes = response.Content.Headers.ContentLength.GetValueOrDefault(-1L);
                var canSendProgress = totalBytes != -1L && progessReporter != null;

                // Step 4 : Get total of data
                var filePath = Path.Combine(pathToSave, fileName);

                // Step 5 : Download data
                using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, BufferSize))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var receivedBytes = 0L;
                        var buffer = new byte[BufferSize];
                        var isMoreDataToRead = true;

                        do
                        {
                            token.ThrowIfCancellationRequested();

                            var read = await stream.ReadAsync(buffer, 0, buffer.Length, token);

                            if (read == 0)
                            {
                                isMoreDataToRead = false;
                            }
                            else
                            {
                                // Write data on disk.
                                await fileStream.WriteAsync(buffer, 0, read);

                                receivedBytes += read;

                                if (canSendProgress)
                                {
                                    progessReporter.Report(new DownloadBytesProgress(fileName, receivedBytes, totalBytes));
                                }
                            }
                        } while (isMoreDataToRead);
                    }
                }
            }
        }
    }
}