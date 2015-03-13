using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageMosaic
{
    public class TileEventArgs
    {
        public TileEventArgs(Bitmap m) { TileToRender = m; }
        public Bitmap TileToRender { get; private set; } // readonly
    }

    public class MosaicGenerator
    {
        public delegate void TileEventHandler(object sender, TileEventArgs e);
        public event TileEventHandler TileEvent;
      
        public async void Generate( string imageToMash, string srcImageDirectory, int sourceImages )
        {

            
            var _imageProcessing = new ImageProcessing();
            var _imageInfos = new List<ImageInfo>();
            var _mosaic = new Mosaic();

            var di = new DirectoryInfo( srcImageDirectory );
            var files = di.GetFiles( "*.jpg", SearchOption.AllDirectories ).Take( sourceImages ).ToList();

            _imageProcessing.MosaicEvent += (sender, eventArgs) => TileEvent(this, new TileEventArgs(eventArgs.Tile));

            foreach( var f in files )
            {
                using( var inputBmp = _imageProcessing.Resize( f.FullName ) )
                {
                    var _info = _imageProcessing.GetAverageColor( inputBmp, f.FullName );

                    if( _info != null )
                        _imageInfos.Add( _info );
                }
            }

            using( var source = new Bitmap( imageToMash ) )
            {
                var _colorMap = _imageProcessing.CreateMap( source );
                await Task.Run(() => _imageProcessing.Render(source, _colorMap, _imageInfos));
            }

        }
        
    }
}
