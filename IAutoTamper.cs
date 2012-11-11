using System;
using System.Collections.Generic;

using System.Text;
using Fiddler;
namespace Weiddler
{
    public interface IAutoTamper : IFiddlerExtension
    {
        // Called before the user can edit a request using the Fiddler Inspectors
        void AutoTamperRequestBefore(Session oSession);

        // Called after the user has had the chance to edit the request using the Fiddler Inspectors, but before the request is sent
        void AutoTamperRequestAfter(Session oSession);

        // Called before the user can edit a response using the Fiddler Inspectors, unless streaming.
        void AutoTamperResponseBefore(Session oSession);

        // Called after the user edited a response using the Fiddler Inspectors.  Not called when streaming.
        void AutoTamperResponseAfter(Session oSession);

        // Called Fiddler returns a self-generated HTTP error (for instance DNS lookup failed, etc)
        void OnBeforeReturningError(Session oSession);
    }
}
