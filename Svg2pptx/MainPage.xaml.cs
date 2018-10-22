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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Svg2pptx
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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
            NavigationViewItem itemContent = sender.SelectedItem as NavigationViewItem;
            switch(itemContent.Tag)
            {
                case "LoadSVG_Page":
                    contentFrame.Navigate(typeof(LoadSVGPage));
                    break;
                case "ManageGroups_Page":
                    contentFrame.Navigate(typeof(ManageGroups_Page));
                    break;
                case "GenPresentation_Page":
                    contentFrame.Navigate(typeof(GenPresentation_Page));
                    break;
            }
        }
    }
}
