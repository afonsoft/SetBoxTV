using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VideoPlayerProima.API;
using VideoPlayerProima.Helpers;

namespace SetBoxWebUITest
{
    [TestClass]
    public class UnitTestSetBoxUI
    {
        SetBoxApi api;

        [TestInitialize]
        public void TestCreateApi()
        {
            try
            {
                api = new VideoPlayerProima.API.SetBoxApi("ABCD", "1111", "https://setbox.afonsoft.com.br/Api/SetBox/");
                Assert.IsNotNull(api);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetSupportAsync()
        {
            try
            {
                var support = await api.GetSupport();
                Assert.IsNotNull(support);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetConfigAsync()
        {
            try
            {
                var config = await api.GetConfig();
                Assert.IsNotNull(config);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetFilesCheckSumsAsync()
        {
            try
            {
                var itens = await api.GetFilesCheckSums();
                int i =0;
                foreach(var item in itens)
                {
                    i++;
                    using (WebClient myWebClient = new WebClient())
                    {
                        myWebClient.DownloadFile(new Uri(item.url), "C:\\Xamarin\\" + item.name);
                    }

                    string CheckMD5Sum = CheckSumHelpers.CalculateMD5("C:\\Xamarin\\" + item.name);

                    if(CheckMD5Sum != item.checkSum)
                    {
                        Assert.Fail();
                    }
                }


                Assert.IsTrue(i > 0);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
