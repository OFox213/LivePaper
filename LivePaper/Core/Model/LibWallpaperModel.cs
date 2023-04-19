using LivePaper.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivePaper.Core.Model
{
    public class LibWallpaperModel
    {
        private readonly LibWallpaperListView items;
        public LibWallpaperModel() { 
            this.items = new LibWallpaperListView();
        }

        public LibWallpaperListView Items
        {
            get { return this.items; }
        }
    }
}
