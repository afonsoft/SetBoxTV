using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VideoPlayerProima.API;

namespace SetBoxWebUITest
{
    [TestClass]
    public class UnitTestSetBoxUI
    {
        [TestMethod]
        public async System.Threading.Tasks.Task TestMethodAPIAsync()
        {
            var api = new VideoPlayerProima.API.SetBoxAPI("ABCD", "1111", "https://setbox.afonsoft.com.br/api");
            var itens = await api.GetFilesCheckSums();
            Assert.IsTrue(itens.Count() > 0);
        }
    }
}
