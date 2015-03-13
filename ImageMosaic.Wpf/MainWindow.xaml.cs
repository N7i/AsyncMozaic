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
using ImageMosaic.Logic;

namespace WpfApplication1
{
    /// <summary> 
    /// Interaction logic for MainWindow.xaml 
    /// </summary> 
    public partial class MainWindow : Window
    {
        private GettyImagesProvider _imagesProvider;


        public MainWindow()
        {
            _imagesProvider = new GettyImagesProvider("22ywrytgjcfg9spzr2qd7g69");

            InitializeComponent();
            SynchonizePictures();
        }

        private async void SynchonizePictures()
        {
            // TODO Disable renderer button till sync job end
            await Task.Run(() => _imagesProvider.SynchronizeAsync());
            button1.Click += button1_Click;
        }

        private async void button1_Click( object sender, RoutedEventArgs e )
        {
            MosaicGenerator generator = new MosaicGenerator();
            TaskScheduler ctx = TaskScheduler.FromCurrentSynchronizationContext();

            int take = SourceImagesCount();
            string imageToMash = _imagesProvider.RndLocalPicturePick().FullName;
            string srcImageDirectory = _imagesProvider.DownloadDir.FullName;

            generator.TileEvent += (origin, tileEventArgs) => Task.Factory.StartNew(() => SetImageSource(tileEventArgs.TileToRender), System.Threading.CancellationToken.None, TaskCreationOptions.None, ctx);
            await Task.Run(() => generator.Generate(imageToMash, srcImageDirectory, take));
        }

        private int SourceImagesCount()
        {
            int take = 100000;
            string content = ((ComboBoxItem)combo1.SelectedItem).Content.ToString();
            if( content != null || content != "All" ) take = Int32.Parse( ((ComboBoxItem)combo1.SelectedItem).Content.ToString() );
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
