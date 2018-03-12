using System;
using Foundation;
using MonoTouch.Dialog;
using UIKit;

namespace Sample.CognitoSyncSettings.iOS
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

    //------------------------------------------------------------------------------
    public class CheckboxElementEx : CheckboxElement
    {
        Action<CheckboxElementEx, EventArgs> onCLick;

        //------------------------------------------------------------------------------
        public CheckboxElementEx (string caption, bool value, string group, 
                                  Action<CheckboxElementEx, EventArgs> onCLick) : base (caption, value, group)
        {
            this.onCLick = onCLick;
        }

        //------------------------------------------------------------------------------
        public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
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

