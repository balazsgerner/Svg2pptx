using System;
using System.Collections.Generic;
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
            setFileName();
        }

        private void setFileName()
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
                return doc.ToString();
            }
        }

        private async void Page_LoadedAsync(object sender, RoutedEventArgs e)
        {
            setFileName();
            await loadFileDetailsAsync(((App)Application.Current).LoadedFile);
        }
    }

}
