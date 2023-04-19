using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivePaper.Core.Model
{
    public class LibWallpaperListView : ObservableCollection<LibWallpaperItem>
    {
        public LibWallpaperListView() {
        }
    }
    public class LibWallpaperItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }
    }
}
