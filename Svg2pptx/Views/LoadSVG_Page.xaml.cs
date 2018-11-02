//using Spire.Presentation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
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


namespace svg
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
            StorageFile file = ((App)Application.Current).LoadedFile;
            await loadFileDetailsAsync(file);
        }

        private async Task loadFileDetailsAsync(StorageFile file)
        {
            if (file != null)
            {
                fileContent.Text = await LoadXMLAsync(file);
                ((App)Application.Current).FileContent = fileContent.Text;
                image.Source = await LoadImageSource(file);
            }
        }

        private async void BrowseFileAsync(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".svg");

            ((App)Application.Current).LoadedFile = await openPicker.PickSingleFileAsync();
            SetFileName();
        }

        private void SetFileName()
        {
            StorageFile file = ((App)Application.Current).LoadedFile;
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

        public async Task<string> LoadXMLAsync(StorageFile file)
        {
            using (var stream = await file.OpenStreamForReadAsync())
            {
                XDocument doc = XDocument.Load(stream, LoadOptions.PreserveWhitespace);

                XNamespace xmlns = "http://www.w3.org/2000/svg";
                ((App)Application.Current).GroupNames = doc.Root.Descendants(xmlns + "g")
                    .Attributes("id")
                    .Where(item => item != null)
                    .Select(item => item.Value)
                    .ToList();

                return doc.ToString();
            }
        }

        private async void Page_LoadedAsync(object sender, RoutedEventArgs e)
        {
            SetFileName();
            await loadFileDetailsAsync(((App)Application.Current).LoadedFile);
        }
    }

}
