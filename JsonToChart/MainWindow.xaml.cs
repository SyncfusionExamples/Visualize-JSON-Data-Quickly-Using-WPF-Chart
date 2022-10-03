using Microsoft.Win32;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChartSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadJson(object sender, RoutedEventArgs e)
        {
            JsonBox.Text = "[\r\n\t{\"Title\":\"Movie A\",\"BoxOffice\":1190.8},\r\n\t{\"Title\":\"Movie B\",\"BoxOffice\":836.8},\r\n\t{\"Title\":\"Movie C\",\"BoxOffice\":2798},\r\n\t{\"Title\":\"Movie D\",\"BoxOffice\":1600.9},\r\n\t{\"Title\":\"Movie E\",\"BoxOffice\":700.3},\r\n\t{\"Title\":\"Movie F\",\"BoxOffice\":1344},\r\n\t{\"Title\":\"Movie G\",\"BoxOffice\":1800.5},\r\n\t{\"Title\":\"Movie H\",\"BoxOffice\":1000.7},\r\n\t{\"Title\":\"Movie I\",\"BoxOffice\":2048},\r\n\t{\"Title\":\"Movie J\",\"BoxOffice\":600.5}\r\n]";
        }

        private void PrepareChart(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel).JsonData = JsonBox.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SfChart chart = (tab.SelectedContent as SfChart);

            if (chart != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "Untitled";
                sfd.Filter = "JPEG(*.jpg,*.jpeg)|*.jpg;*.jpeg|Gif (*.gif)|*.gif|PNG(*.png)|*.png|Bitmap(*.bmp)|*.bmp|All files (*.*)|*.*";
                if (sfd.ShowDialog() == true)
                {
                    using (Stream fs = sfd.OpenFile())
                    {
                        double w = chart.ActualWidth;
                        double h = chart.ActualHeight;
                        double dpi = 300;
                        double scale = dpi / 96;

                        RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)(w * scale), (int)(h * scale), dpi, dpi, PixelFormats.Pbgra32);
                        renderTargetBitmap.Render(chart);
                        JpegBitmapEncoder jpgImage = new JpegBitmapEncoder();
                        jpgImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                        chart.Save(fs, new JpegBitmapEncoder());
                    }
                }
            }
        }
    }
}
