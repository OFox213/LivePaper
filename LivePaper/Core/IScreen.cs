using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivePaper.Core
{
    public interface IScreen : IEquatable<IScreen>
    {
        int Bpp { get; set; }
        string Id { get; set; }
        string Name { get; set; }
        string Number { get; set; }
        Rectangle Bounds { get; set; }
        Rectangle Area { get; set; }

    }
}
