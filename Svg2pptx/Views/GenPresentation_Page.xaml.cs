using Newtonsoft.Json;
using Syncfusion.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace svg
{
    /// <summary>
    /// Prezentáció generálásra szolgáló nézet.
    /// </summary>
    public sealed partial class GenPresentation_Page : Page
    {
        Dictionary<int, XDocument> svgContentBySlide;

        public GenPresentation_Page()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var originalFileContent = ((App)Application.Current).FileContent;
            if (originalFileContent == null)
            {
                return;
            }

            XNamespace xmlns = "http://www.w3.org/2000/svg";
            var visibleGroupsBySlide = ((App)Application.Current).VisibleGroupsBySlide;
            var svgContentBySlide = new Dictionary<int, XDocument>();

            XDocument doc;
            for (int i = 0; i < visibleGroupsBySlide.Count; i++)
            {
                var groups = visibleGroupsBySlide[i];
                doc = XDocument.Parse(originalFileContent);

                // kitöröljük az összes olyan csoportot, amely nem kell az adott slide-ra
                var forRemoval = doc.Root.Descendants(xmlns + "g")
                    .Where(item => (item.Attribute("id") == null) ||
                        !groups.Contains(item.Attribute("id").Value))
                    .ToList();
                foreach (var element in forRemoval)
                {
                    element.Remove();
                }
                svgContentBySlide[i] = doc;
            }

            // svgContentBySlide - slide-onkénti végleges svg tartalom --> fáljba irni, konvertálni
            Console.WriteLine(svgContentBySlide);
            this.svgContentBySlide = svgContentBySlide;
            //GeneratePresentation(svgContentBySlide);

        }

        private async void GeneratePresentation(object sender, RoutedEventArgs e)
        {
            GeneratePresentationReallyPls();
        }

        public async void GeneratePresentationReallyPls()
        {
            //var firstRaw = this.svgContentBySlide[0].ToString();

            //var streamAsd = await SendReq(firstRaw);
            var a = 0;

            //Creates a new instance of PowerPoint presentation
            IPresentation presentation = Presentation.Create();
            for (var i = 0; i < svgContentBySlide.Count; i++)
            {

                //Adds a slide to the PowerPoint Presentation
                ISlide newSlide = presentation.Slides.Add(SlideLayoutType.Blank);

                //Adds a textbox in a slide by specifying its position and size
                //IShape textShape = firstSlide.AddTextBox(100, 75, 756, 200);

                //Adds a paragraph into the textShape
                //IParagraph paragraph = textShape.TextBody.AddParagraph();

                //Set the horizontal alignment of paragraph
                //paragraph.HorizontalAlignment = HorizontalAlignmentType.Center;

                //Adds a textPart in the paragraph
                //ITextPart textPart = paragraph.AddTextPart("Hello Presentation");
                ////Gets the image from file path
                //Image image = Image.(@"image.jpg");

                //// Adds the image to the slide by specifying position and size

                var raw = this.svgContentBySlide[i].ToString();

                var streamAsd = await SendReq(raw);
                MemoryStream stream2 = new MemoryStream();
                (streamAsd).CopyTo(stream2);
                newSlide.Pictures.AddPicture(stream2, Convert.ToDouble(xPos.Text), Convert.ToDouble(yPos.Text), Convert.ToDouble(width.Text), Convert.ToDouble(height.Text));

            }
            //Applies font formatting to the text
            //textPart.Font.FontSize = 80;
            //textPart.Font.Bold = true;
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

        private async Task<Stream> SendReq(string rawString)
        {
            var httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            using (var clientHandler = new HttpClientHandler() { AllowAutoRedirect = true })
            using (var httpClient = new HttpClient(clientHandler))
            {
                try
                {
                    // create input map
                    var convertOptionsMap = new Dictionary<string, object>
                    {
                        { "resize","500x500"},
                        { "density", 96},
                    };
                    var inputMap = new Dictionary<string, object>()
                    {
                        { "apikey" , "mpwfPIpmNTFYEHzMNFp3OUlOSzaUO842J6UOi1VdIh4jbCzU6jqQhX4TcGj6RXhL" },
                        { "input" , "raw" },
                        { "inputformat", "svg"  },
                        { "outputformat" , "png" },
                        { "filename", "sample.svg" },
 
                         // content: raw string-ként
                        { "file" , rawString },//((App)Application.Current).FileContent},
                        { "converteroptions" , convertOptionsMap},
                        { "timeout" , 0},
                        { "wait" , true},
                        { "download" , true},
                    };

                    // convert to json
                    var inputMapJson = JsonConvert.SerializeObject(inputMap);
                    var content = new StringContent(inputMapJson);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    // call web service
                    Uri requestUri = new Uri("https://api.cloudconvert.com/convert");
                    httpResponse = await httpClient.PostAsync(requestUri, content);
                    httpResponse.EnsureSuccessStatusCode();

                    // save converted file
                    FileSavePicker savePicker = new FileSavePicker
                    {
                        SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                        SuggestedFileName = "sample.png"
                    };
                    savePicker.FileTypeChoices.Add("Image", new List<string>() { ".png" });

                    //StorageFile file = await savePicker.PickSaveFileAsync();
                    //if (file != null)
                    //{
                    //    using (var outStream = await file.OpenStreamForWriteAsync())
                    //    {
                    //        (await httpResponse.Content.ReadAsStreamAsync()).CopyTo(outStream);
                    //    }
                    //}
                    return await httpResponse.Content.ReadAsStreamAsync();

                }
                catch (Exception ex)
                {
                    httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    return null;
                }
            }
        }
    }
}
