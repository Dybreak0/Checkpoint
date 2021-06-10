using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace MobileJO.iOS.CustomRenderers
{
    public class PickerDelegate : NSObject, IUITextFieldDelegate
    {
        [Export("textField:shouldChangeCharactersInRange:replacementString:")]
        public bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString) =>
            false;
    }
}