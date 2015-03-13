using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ImageMosaic
{
    public class MosaicEventArgs
    {
        public MosaicEventArgs(Image img) { Img = img; }
        public Image Img { get; private set; }
    }
    public class Mosaic
    {
        public delegate void MosaicEventHandler(object sender, MosaicEventArgs e);
        public event MosaicEventHandler MosaicChangedEvent;
        private Image _img;

        public Image Image {
            get
            {
                return _img;
            }
            set
            {
                if (MosaicChangedEvent != null)
                    MosaicChangedEvent(this, new MosaicEventArgs(value));

                _img = value;
            }
        }
        public List<MosaicTile> Tiles { get; set; }
    }
}
