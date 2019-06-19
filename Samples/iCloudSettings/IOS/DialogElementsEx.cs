using System;
using Foundation;
using MonoTouch.Dialog;
using UIKit;

namespace Sample.iCloudSettings.iOS
{
    //------------------------------------------------------------------------------
    public class CheckboxElementEx : CheckboxElement
    {
        Action<CheckboxElementEx, EventArgs> onCLick;

        //------------------------------------------------------------------------------
        public CheckboxElementEx(string s, bool value, Action<CheckboxElementEx, EventArgs> onCLick) : base (s, value)
        {
            this.onCLick = onCLick;
        }

        //------------------------------------------------------------------------------
        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            base.Selected (dvc, tableView, path);
            var selected = onCLick;
            if (selected != null)
            {
                selected (this, EventArgs.Empty);
            }
        }
    }
}

