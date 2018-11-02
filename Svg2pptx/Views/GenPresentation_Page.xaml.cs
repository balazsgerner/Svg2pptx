using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public GenPresentation_Page()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            XNamespace xmlns = "http://www.w3.org/2000/svg";
            var originalFileContent = ((App)Application.Current).FileContent;
            var visibleGroupsBySlide = ((App)Application.Current).VisibleGroupsBySlide;
            var svgContentBySlide = new Dictionary<int, XDocument>();

            for (int i = 0; i < visibleGroupsBySlide.Count; i++)
            {
                var groups = visibleGroupsBySlide[i];
                XDocument doc = XDocument.Parse(originalFileContent);

                // kitöröljük az összes olyan csoportot, amely nem kell az adott slide-ra
                var forRemoval = doc.Root.Descendants(xmlns + "g")
                    .Where(item => !groups.Contains(item.Attribute("id").Value))
                    .ToList();
                foreach (var element in forRemoval)
                {
                    element.Remove();
                }
                svgContentBySlide[i] = doc;
            }

            // svgContentBySlide - slide-onkénti végleges svg tartalom --> fáljba irni, konvertálni
            Console.WriteLine(svgContentBySlide);

        }

    }
}
