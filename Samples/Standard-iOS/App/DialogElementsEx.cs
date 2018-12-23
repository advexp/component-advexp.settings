using System;
using Foundation;
using MonoTouch.Dialog;
using UIKit;

namespace Sample.App.iOS
{
    //------------------------------------------------------------------------------
    public class RadioElementEx : RadioElement
    {
        Action<RadioElementEx, EventArgs> onCLick;

        //------------------------------------------------------------------------------
        public RadioElementEx (string s, Action<RadioElementEx, EventArgs> onCLick) : base (s)
        {
            this.onCLick = onCLick;
        }

        //------------------------------------------------------------------------------
        public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
        {
            base.Selected (dvc, tableView, indexPath);
            var selected = onCLick;
            if (selected != null)
            {
                selected (this, EventArgs.Empty);
            }
        }
    }
}

