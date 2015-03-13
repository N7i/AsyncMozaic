using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using ImageMosaic;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    /// <summary> 
    /// Interaction logic for MainWindow.xaml 
    /// </summary> 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // For this example, values are hard-coded to a mosaic of 8x8 tiles. 

            button1.Click += button1_Click;

        }

        private async void button1_Click( object sender, RoutedEventArgs e )
        {
            MosaicGenerator generator = new MosaicGenerator();

            int take = SourceImagesCount();

            string imageToMash = @"C:\Users\Clem\Pictures\Async\" + "images.JPG";
            string srcImageDirectory = @"C:\Users\Clem\Pictures\master rabbit";

            var ctx = TaskScheduler.FromCurrentSynchronizationContext();
            generator.TileEvent += (origin, tileEventArgs) => Task.Factory.StartNew(() => SetImageSource(tileEventArgs.TileToRender), System.Threading.CancellationToken.None, TaskCreationOptions.None, ctx);

            await Task.Run(() => generator.Generate(imageToMash, srcImageDirectory, take));


            //SetImageSource( result.Image );


        }

        private void drawTile(MosaicTile tile) {

        }

        private int SourceImagesCount()
        {
            int take = 100000;
            string content = ((ComboBoxItem)combo1.SelectedItem).Content.ToString();
            if( content != "All" ) take = Int32.Parse( ((ComboBoxItem)combo1.SelectedItem).Content.ToString() );
            return take;
        }

        private void SetImageSource( System.Drawing.Image image )
        {
            
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();

            MemoryStream ms = new MemoryStream();
            image.Save( ms, ImageFormat.Bmp );
            ms.Seek( 0, SeekOrigin.Begin );
            bi.StreamSource = ms;

            bi.EndInit();
            image1.Source = bi;
        }

        private void ComboBox_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {

        }


    }
}
