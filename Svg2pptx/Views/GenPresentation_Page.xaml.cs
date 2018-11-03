using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Syncfusion.Presentation;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Text;
using Svg;
using System.Net.Http.Headers;
using System.Globalization;
using Windows.Web.Http;
using System.Net.Http;
using System.Diagnostics;

namespace Svg2pptx.Views
{
    /// <summary>
    /// Prezentáció generálásra szolgáló nézet.
    /// </summary>
    public sealed partial class GenPresentation_Page : Page
    {
        public GenPresentation_Page()
        {
            this.InitializeComponent();
        }

        private async void GeneratePresentation(object sender, RoutedEventArgs e)
        {
            var a = ((App)Application.Current).loadedFile;


            // save loaded file to C:\Users\***\AppData\Local\Packages\4fb09f32-481f-4c3f-8618-f2f1dbc911d0_4vdk1afx0z06j\LocalState
            MemoryStream stream3 = new MemoryStream();
            (await a.OpenStreamForReadAsync()).CopyTo(stream3);
            SendReq(await a.OpenStreamForReadAsync());
            //StorageFolder storageFolder2 = ApplicationData.Current.LocalFolder;
            //StorageFile sampleFile2 = await storageFolder2.CreateFileAsync("sample.png", CreationCollisionOption.ReplaceExisting);


            //using (var fileStream = await sampleFile2.OpenStreamForWriteAsync())
            //{
            //    fileStream.Seek(0, SeekOrigin.Begin);
            //    stream3.WriteTo(fileStream);
            //}


            //// read from assets, save it to C:\Users\***\AppData\Local\Packages\4fb09f32-481f-4c3f-8618-f2f1dbc911d0_4vdk1afx0z06j\LocalState
            //MemoryStream stream4 = new MemoryStream();
            //StorageFile sourceFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/sample3.png"));
            //(await sourceFile.OpenStreamForReadAsync()).CopyTo(stream4);
            //StorageFile sampleFile22 = await storageFolder2.CreateFileAsync("sample33.png", CreationCollisionOption.ReplaceExisting);

            ////get asets folder
            //StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            //StorageFolder assetsFolder = await appInstalledFolder.GetFolderAsync("Assets");

            ////move file from public folder to assets
            //await sampleFile22.MoveAsync(assetsFolder, "new_file.png", NameCollisionOption.ReplaceExisting);


            //using (var fileStream = await sampleFile22.OpenStreamForWriteAsync())
            //{
            //    fileStream.Seek(0, SeekOrigin.Begin);
            //    stream4.WriteTo(fileStream);
            //}










            ////Creates a new instance of PowerPoint presentation
            //IPresentation presentation = Presentation.Create();

            ////Adds a slide to the PowerPoint Presentation
            //ISlide firstSlide = presentation.Slides.Add(SlideLayoutType.Blank);

            ////Adds a textbox in a slide by specifying its position and size
            //IShape textShape = firstSlide.AddTextBox(100, 75, 756, 200);

            ////Adds a paragraph into the textShape
            //IParagraph paragraph = textShape.TextBody.AddParagraph();

            ////Set the horizontal alignment of paragraph
            //paragraph.HorizontalAlignment = HorizontalAlignmentType.Center;

            ////Adds a textPart in the paragraph
            //ITextPart textPart = paragraph.AddTextPart("Hello Presentation");
            //////Gets the image from file path
            ////Image image = Image.(@"image.jpg");

            ////// Adds the image to the slide by specifying position and size

            //MemoryStream stream2 = new MemoryStream();
            //(await a.OpenStreamForReadAsync()).CopyTo(stream2);
            //firstSlide.Pictures.AddPicture(stream2, 300, 270, 410, 250);

            ////Applies font formatting to the text
            //textPart.Font.FontSize = 80;
            //textPart.Font.Bold = true;
            ////Creates an instance of memory stream
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    //Saves the Presentation in the given name 
            //    presentation.Save(stream);

            //    // Egyelőre ide menti: C:\Users\***\AppData\Local\Packages\4fb09f32-481f-4c3f-8618-f2f1dbc911d0_4vdk1afx0z06j\LocalState
            //    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //    StorageFile sampleFile = await storageFolder.CreateFileAsync("sample.pptx", CreationCollisionOption.ReplaceExisting);

            //    using (var fileStream = await sampleFile.OpenStreamForWriteAsync())
            //    {
            //        fileStream.Seek(0, SeekOrigin.Begin);
            //        stream.WriteTo(fileStream);
            //    }
            //}

            //presentation.Close();
        }
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    var parameter = (StorageFile)e.Parameter;
        //    image = parameter;
        //}
        private async void SendReq(Stream stream)
        {
            byte[] fileBytes;
            var binaryReader = new BinaryReader(stream);
            fileBytes = binaryReader.ReadBytes((int)stream.Length);
            string base64 = Convert.ToBase64String(fileBytes);
            var imageBinaryContent = new ByteArrayContent(fileBytes);
            //Stream stream2 = new MemoryStream();

            //await imageBinaryContent.CopyToAsync(stream2);
            //stream2.Seek(0, SeekOrigin.Begin);
            var formData = new MultipartFormDataContent();
            //HttpContent content = new StreamContent(stream2);
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent("O6M1sGUhYzreXAq8xXZCNKfEbK6zvLHEj6PbkviP7dvx0m0szoQsG0ACheqLbNNU"), "apikey");
            multipartContent.Add(new StringContent("base64"), "input");
            multipartContent.Add(new StringContent("kiwi.svg"), "filename");
            multipartContent.Add(new StringContent("true"), "wait");
            multipartContent.Add(new StringContent("inline"), "download");
            multipartContent.Add(new StringContent("svg"), "inputformat");
            multipartContent.Add(new StringContent("png"), "outputformat");



            var multipartContentFirst = new MultipartFormDataContent();
            multipartContentFirst.Add(new StringContent(base64), "file");
            multipartContentFirst.Add(new StringContent("O6M1sGUhYzreXAq8xXZCNKfEbK6zvLHEj6PbkviP7dvx0m0szoQsG0ACheqLbNNU"), "apikey");
            multipartContentFirst.Add(new StringContent("base64"), "input");
            multipartContentFirst.Add(new StringContent("kiwi.svg"), "filename");
            multipartContentFirst.Add(new StringContent("true"), "wait");
            multipartContentFirst.Add(new StringContent("false"), "download");
            multipartContentFirst.Add(new StringContent("svg"), "inputformat");
            multipartContentFirst.Add(new StringContent("png"), "outputformat");
            formData.Add(multipartContentFirst);

            //multipartContentFirst.Add(new StringContent("upload"), "input");
            //Create an HTTP client object
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

            ////Add a user-agent header to the GET request. 
            //var headers = httpClient.DefaultRequestHeaders;

            ////The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            ////especially if the header value is coming from user input.
            //string header = "ie";
            //if (!headers.UserAgent.TryParseAdd(header))
            //{
            //    throw new Exception("Invalid header value: " + header);
            //}

            //header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            //if (!headers.UserAgent.TryParseAdd(header))
            //{
            //    throw new Exception("Invalid header value: " + header);
            //}

            Uri requestUri = new Uri("https://api.cloudconvert.com/process");

            //Send the GET request asynchronously and retrieve the response as a string.
            System.Net.Http.HttpResponseMessage httpResponse = new System.Net.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                //httpResponse = await httpClient.GetAsync(requestUri);
                //httpResponse.EnsureSuccessStatusCode();
                //httpResponseBody = await httpResponse.Content.ReadAsStringAsync();


                //Debug.Write(url);
                httpResponse = await httpClient.PostAsync(new Uri("https://api.cloudconvert.com/process"), multipartContent);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

                string id = httpResponseBody.Split("\"url\":\"")[1];
                string id2 = id.Split("\"")[0];
                string url = ("https:" + id2).Replace(@"\", "");
                httpResponse = await httpClient.PostAsync(new Uri(url), formData);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                //string idO = httpResponseBody.Split("\"output\":")[1];
                //idO = idO.Split("\"input\":")[0];
                //idO = idO.Split("\"url\":\"")[1];
                //string id2O = idO.Split("\",\"expire\"")[0];
                //string outputurl = ("https:" + id2O).Replace(@"\", "").Replace("\"},", "");

                //id = httpResponseBody.Split("\"upload\":")[1];
                //id = id.Split("\"url\":\"")[1];
                //id2 = id.Split("\"}}")[0];
                //string url2 = ("https:" + id2).Replace(@"\", "").Replace("\"},", "");
                //Debug.Write(url);

                ////multipartContent.Headers.Add("Content-Length", ((int)stream.Length).ToString());

                //httpResponse = await httpClient.PutAsync(new Uri(url2 +"/kiwi.svg"), content);
                //httpResponse.EnsureSuccessStatusCode();
                //httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                //httpResponse = await httpClient.GetAsync(new Uri(url));
                //httpResponse.EnsureSuccessStatusCode();
                //httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                //httpResponse = await httpClient.GetAsync(new Uri(outputurl));
                //httpResponse.EnsureSuccessStatusCode();
                //httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                //var a = 2;
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
        }
    }


}
