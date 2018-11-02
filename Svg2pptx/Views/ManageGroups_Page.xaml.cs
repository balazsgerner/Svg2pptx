using System;
using System.Collections.Generic;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace svg
{
    /// <summary>
    /// A csoportok láthatóságának beállítására szolgáló nézet.
    /// </summary>
    public sealed partial class ManageGroups_Page : Page
    {
        public List<string> GroupNames { get; }

        public ManageGroups_Page()
        {
            this.InitializeComponent();
            GroupNames = ((App)Application.Current).GroupNames;
        }

        private void InitGroups(object sender, RoutedEventArgs e)
        {
            // 1. sor és oszlop
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(150)
            });
            grid.RowDefinitions.Add(new RowDefinition());

            // 1. sor - csoportok neve
            for (int i = 0; i < GroupNames.Count; i++)
            {
                AddGroup(i);
            }

            // slide-ok hozzá adása - 3 dummy slide
            for (int i = 1; i < 3; i++)
            {
                AddSlide(null, null);

            }
        }

        private void AddGroup(int i)
        {
            grid.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(50),
            });
            var tb = new TextBlock
            {
                Text = GroupNames[i],
                VerticalAlignment = VerticalAlignment.Bottom,
                Padding = new Thickness(10),
                FontWeight = FontWeights.Bold
            };
            grid.Children.Add(tb);
            Grid.SetRow(tb, i + 1);
            Grid.SetColumn(tb, 0);
        }

        private void AddSlide(object sender, RoutedEventArgs e)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(55),
            });

            int colIdx = grid.ColumnDefinitions.Count - 1;

            var tb = new TextBlock
            {
                Text = "Slide" + colIdx,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Padding = new Thickness(0, 10, 0, 5),
                FontWeight = FontWeights.Bold
            };
            grid.Children.Add(tb);
            Grid.SetRow(tb, 0);
            Grid.SetColumn(tb, colIdx);

            var rand = new Random();
            for (int j = 0; j < GroupNames.Count; j++)
            {
                var cb = new CheckBox()
                {
                    Margin = new Thickness(15, 0, 0, 0),
                    // random láthatóságot generál
                    IsChecked = rand.Next(100) <= 50 ? true : false,
                    VerticalAlignment = VerticalAlignment.Bottom,
                };
                grid.Children.Add(cb);
                Grid.SetRow(cb, j + 1);
                Grid.SetColumn(cb, colIdx);
            }

        }

        // az utolsó slide-ot törli (és az oszlop minden elemét)
        private void RemoveSlide(object sender, RoutedEventArgs e)
        {
            int rows = grid.RowDefinitions.Count;
            int children = grid.Children.Count;
            for (int i = children - 1; i > children - rows - 1; i--)
            {
                grid.Children.RemoveAt(i);
            }
            grid.ColumnDefinitions.RemoveAt(grid.ColumnDefinitions.Count - 1);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SaveGroups();
        }

        // elmentjük diánként a látható csoportok nevét
        private void SaveGroups()
        {
            var visibleGroupsBySlide = new Dictionary<int, List<string>>();
            int rows = grid.RowDefinitions.Count;
            int columns = grid.ColumnDefinitions.Count;
            int children = grid.Children.Count;

            for (int startIndex = rows; startIndex < children; startIndex += rows)
            {
                var visibleGroups = new List<string>();
                for(int row = 0; row < rows - 1; row++)
                {
                    var cb = ((CheckBox)grid.Children[startIndex + row]);
                    bool isChecked = cb.IsChecked.GetValueOrDefault(false);
                    if(isChecked)
                    {
                        visibleGroups.Add(GetGroupNameByRow(row));
                    }
                }
                visibleGroupsBySlide[(startIndex / rows) - 1] = visibleGroups;
            }

            ((App)Application.Current).VisibleGroupsBySlide = visibleGroupsBySlide;
        }

        // adott sorhoz tartozó csoport neve
        private string GetGroupNameByRow(int row)
        {
            return ((TextBlock)grid.Children[row]).Text;
        }
    }

}