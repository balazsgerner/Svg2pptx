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
using Windows.Storage.Pickers;
using Windows.Storage;
using Svg2pptx.Views;

namespace Svg2pptx
{
    /// <summary>
    /// A főoldal, amely a navigációért, azaz az oldalak közötti váltásért felelős.
    /// <list type="bullet">
    ///     <listheader>Nézetek:</listheader>
    ///     <item><term>SVG fájl betöltése</term></item>
    ///     <item><term>Csoportok láthatóságának módosítása</term></item>
    ///     <item><term>Prezentáció generálása</term></item>
    /// </list>
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void nvTopLevelNav_Loaded(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(LoadSVGPage));
        }

        private void nvTopLevelNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavigationViewItem;
            nvTopLevelNav.Header = item.Content;
        }

        private void nvTopLevelNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            // TODO Ezt ideiglenes csináltam meg így hogy lehessen navigálni, bugos, mert kétszer kell rákattintani, az előző verzió nem működött
            String itemContent = sender.Header.ToString(); //as NavigationViewItem;
            switch(itemContent)
            {
                case "Load SVG":
                    contentFrame.Navigate(typeof(LoadSVGPage));
                    break;
                case "Manage groups":
                    contentFrame.Navigate(typeof(ManageGroups_Page));
                    break;
                case "Generate presentation":
                    contentFrame.Navigate(typeof(GenPresentation_Page));
                    break;
            }
        }
    }
}
