using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Svg2pptx.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadSVGPage : Page
    {
        public StorageFile loadedFile { get; set; }

        public LoadSVGPage()
        {
            this.InitializeComponent();
        }


        private async void LoadFileAsync(object sender, RoutedEventArgs e)
        {
            fileContent.Text = PrintXML(await FileIO.ReadTextAsync(loadedFile));
            image.Source = await LoadImageSource(loadedFile);
        }

        private async void BrowseFileAsync(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".svg");
            openPicker.FileTypeFilter.Add(".jpg");

            loadedFile = await openPicker.PickSingleFileAsync();
            if (loadedFile != null)
            {
                fileName.Text = loadedFile.Name;
                fileFullName.Text = loadedFile.Path;
            }
        }

        private static async Task<ImageSource> LoadImageSource(StorageFile file)
        {
            SvgImageSource src = new SvgImageSource();
            await src.SetSourceAsync(await file.OpenReadAsync());
            return src;
        }

        public string PrintXML(string xml)
        {
            string result = string.Empty;
            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, System.Text.Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                document.LoadXml(xml);

                writer.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream);

                // Extract the text from the StreamReader.
                string formattedXml = sReader.ReadToEnd();

                result = formattedXml;
            }
            catch (XmlException)
            {
                // Handle the exception
            }

            mStream.Close();
            writer.Close();

            return result;
        }
    }

}
