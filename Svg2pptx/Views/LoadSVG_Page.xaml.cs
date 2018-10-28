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


namespace Svg2pptx.Views
{
    /// <summary>
    /// SVG fájl betöltésére szolgáló nézet.
    /// </summary>
    public sealed partial class LoadSVGPage : Page
    {

        private SvgImageSource ImageSource { get; set; }

        public LoadSVGPage()
        {
            this.InitializeComponent();
            ImageSource = new SvgImageSource();
        }


        private async void LoadFileAsync(object sender, RoutedEventArgs e)
        {
            StorageFile file = ((App)Application.Current).loadedFile;
            await loadFileDetailsAsync(file);
        }

        private async Task loadFileDetailsAsync(StorageFile file)
        {
            if (file != null)
            {
                fileContent.Text = PrintXML(await FileIO.ReadTextAsync(file));
                image.Source = await LoadImageSource(file);
            }
        }

        private async void BrowseFileAsync(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".jpg");

            ((App)Application.Current).loadedFile = await openPicker.PickSingleFileAsync();
            setFileName();
            //Frame.Navigate(typeof(GenPresentation_Page), ((App)Application.Current).loadedFile);
        }

        private void setFileName()
        {
            StorageFile file = ((App)Application.Current).loadedFile;
            if (file != null)
            {
                fileName.Text = file.Name;
                fileFullName.Text = file.Path;
            }
            
        }

        private async Task<ImageSource> LoadImageSource(StorageFile file)
        {
            await ImageSource.SetSourceAsync(await file.OpenReadAsync());
            return ImageSource;
        }

        public string PrintXML(string xml)
        {
            string result = string.Empty;
            XmlDocument document = new XmlDocument();
            using (var mStream = new MemoryStream())
            using (var writer = new XmlTextWriter(mStream, System.Text.Encoding.Unicode))
            using (var sReader = new StreamReader(mStream))
            {
                try
                {
                    document.LoadXml(xml);
                    writer.Formatting = Formatting.Indented;

                    document.WriteContentTo(writer);
                    writer.Flush();
                    mStream.Flush();

                    mStream.Position = 0;
                    result = sReader.ReadToEnd();
                }
                catch (XmlException)
                {
                    // Error occured while printing XML content
                }

            }
            return result;
        }

        private async void Page_LoadedAsync(object sender, RoutedEventArgs e)
        {
            //setFileName();
            //await loadFileDetailsAsync(((App)Application.Current).loadedFile);
        }
    }

}
