using System;
using System.Collections.Generic;

using System.Text;

namespace Weiddler
{
    public interface IFiddlerExtension
    {
        // Called when Fiddler User Interface is fully available
        void OnLoad();

        // Called when Fiddler is shutting down
        void OnBeforeUnload();
    }
}
