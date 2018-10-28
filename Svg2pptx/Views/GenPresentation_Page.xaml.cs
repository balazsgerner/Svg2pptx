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
            //SendReq();
            var a = ((App)Application.Current).loadedFile;



            MemoryStream stream3 = new MemoryStream();
            (await a.OpenStreamForReadAsync()).CopyTo(stream3);
            StorageFolder storageFolder2 = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile2 = await storageFolder2.CreateFileAsync("sample.svg", CreationCollisionOption.ReplaceExisting);

            using (var fileStream = await sampleFile2.OpenStreamForWriteAsync())
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                stream3.WriteTo(fileStream);
            }











            //Creates a new instance of PowerPoint presentation
            IPresentation presentation = Presentation.Create();

            //Adds a slide to the PowerPoint Presentation
            ISlide firstSlide = presentation.Slides.Add(SlideLayoutType.Blank);

            //Adds a textbox in a slide by specifying its position and size
            IShape textShape = firstSlide.AddTextBox(100, 75, 756, 200);

            //Adds a paragraph into the textShape
            IParagraph paragraph = textShape.TextBody.AddParagraph();

            //Set the horizontal alignment of paragraph
            paragraph.HorizontalAlignment = HorizontalAlignmentType.Center;

            //Adds a textPart in the paragraph
            ITextPart textPart = paragraph.AddTextPart("Hello Presentation");
            ////Gets the image from file path
            //Image image = Image.(@"image.jpg");

            //// Adds the image to the slide by specifying position and size







            MemoryStream stream2 = new MemoryStream();
            (await a.OpenStreamForReadAsync()).CopyTo(stream2);
            firstSlide.Pictures.AddPicture(stream2, 300, 270, 410, 250);






            //Applies font formatting to the text
            textPart.Font.FontSize = 80;
            textPart.Font.Bold = true;
            //Creates an instance of memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                //Saves the Presentation in the given name 
                presentation.Save(stream);

                // Egyelőre ide menti: C:\Users\***\AppData\Local\Packages\4fb09f32-481f-4c3f-8618-f2f1dbc911d0_4vdk1afx0z06j\LocalState
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.CreateFileAsync("sample.pptx", CreationCollisionOption.ReplaceExisting);

                using (var fileStream = await sampleFile.OpenStreamForWriteAsync())
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    stream.WriteTo(fileStream);
                }
            }














            presentation.Close();
        }
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    var parameter = (StorageFile)e.Parameter;
        //    image = parameter;
        //}
        //private async void SendReq()
        //{

        //    //Create an HTTP client object
        //    Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

        //    ////Add a user-agent header to the GET request. 
        //    //var headers = httpClient.DefaultRequestHeaders;

        //    ////The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
        //    ////especially if the header value is coming from user input.
        //    //string header = "ie";
        //    //if (!headers.UserAgent.TryParseAdd(header))
        //    //{
        //    //    throw new Exception("Invalid header value: " + header);
        //    //}

        //    //header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
        //    //if (!headers.UserAgent.TryParseAdd(header))
        //    //{
        //    //    throw new Exception("Invalid header value: " + header);
        //    //}

        //    Uri requestUri = new Uri("https://api.cloudconvert.com/process?inputformat=svg&outputformat=png&apikey=8ozTEcwGp3h5qJYtCOWdXZBg3sPzq3kLpLv77AofQCuaFCDVPBpEEIholjG078Ud");

        //    //Send the GET request asynchronously and retrieve the response as a string.
        //    Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
        //    string httpResponseBody = "";

        //    try
        //    {
        //        //Send the GET request
        //        httpResponse = await httpClient.GetAsync(requestUri);
        //        httpResponse.EnsureSuccessStatusCode();
        //        httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
        //    }
        //}
    }

}
