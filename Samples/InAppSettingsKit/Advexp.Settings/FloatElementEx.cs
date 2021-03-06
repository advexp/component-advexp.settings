using System;
using CoreGraphics;
using Foundation;
using MonoTouch.Dialog;
using UIKit;

namespace InAppSettingsKitSample
{
    public class FloatElementEx : Element
    {
        static NSString skey = new NSString("FloatElementEx");
        const float LockImageWidth = 32.0f;
        const float LockImageHeight = 32.0f;

        /// <summary>
        /// Set a string to reserve a certain amount of space for the 
        /// caption used in the FloatElement. Useful when there is no
        /// initial caption to show - allows space to be reserved for 
        /// when it will be set.
        /// </summary>
        public string ReserveCaptionPlaceholderString { get; set; }
        /// <summary>
        /// Returns the locked status
        /// </summary>
        public bool IsLocked { get { return _valueLocked; } }
        public bool ShowCaption { get; set; }
        /// <summary>
        /// Ties the displayed caption to the value of the slider
        /// </summary>
        public bool UseCaptionForValueDisplay { get; set; }
        public bool Continuous { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int Value { get; private set; }
        public UIImage LockImage { get; set; }
        public UIImage UnlockImage { get; set; }

        private UIButton _lockImageView;
        private UISlider _slider;
        private Action<int> _valueChangedCallback;
        private bool _valueLocked;
        private bool _lockable = false;


        public FloatElementEx(int value, Action<int> valueChanged = null, bool continuous = true, bool lockable = false)
            : base(null)
        {
            MinValue = 0;
            MaxValue = 100;
            Value = value;
            Continuous = continuous;
            _lockable = lockable;
            _valueChangedCallback = valueChanged;
        }

        static CGSize DeviceSize()
        {
            return UIApplication.SharedApplication.KeyWindow.RootViewController.View.ConvertRectFromView(UIScreen.MainScreen.Bounds, null).Size;
        }

        protected override NSString CellKey { get { return skey; } }

        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(CellKey);
            if (cell == null) {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellKey);
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            else
                RemoveTag(cell, 1);

            CGSize captionSize = new CGSize(0, 0);
            if (ShowCaption && (Caption != null || ReserveCaptionPlaceholderString != null || UseCaptionForValueDisplay)) {
                if (Caption == null) {
                    if (UseCaptionForValueDisplay)
                    {
                        NSString str = new NSString(MaxValue.ToString());
                        UIStringAttributes attributes = new UIStringAttributes();
                        attributes.Font = UIFont.FromName(cell.TextLabel.Font.Name, UIFont.LabelFontSize);
                        captionSize = str.GetSizeUsingAttributes (attributes);
                    }
                    else if (!string.IsNullOrEmpty(ReserveCaptionPlaceholderString))
                    {
                        NSString str = new NSString(ReserveCaptionPlaceholderString);
                        UIStringAttributes attributes = new UIStringAttributes();
                        attributes.Font = UIFont.FromName(cell.TextLabel.Font.Name, UIFont.LabelFontSize);
                        captionSize = str.GetSizeUsingAttributes (attributes);
                    }
                }
                else 
                {
                    NSString str = new NSString(Caption);
                    UIStringAttributes attributes = new UIStringAttributes();
                    attributes.Font = UIFont.FromName(cell.TextLabel.Font.Name, UIFont.LabelFontSize);
                    captionSize = str.GetSizeUsingAttributes (attributes);
                }

                captionSize.Width += 10; // Spacing

                if (Caption != null)
                    cell.TextLabel.Text = Caption;
            }

            var lockImageWidth = _lockable ? LockImageWidth : 0;

            CGSize deviceSize = DeviceSize();
            CGRect sliderRect = new CGRect(10f + captionSize.Width, 0f,
                                                   deviceSize.Width - captionSize.Width - lockImageWidth - 20f, 
                                                   cell.Frame.Height);

            if (_slider == null) {
                _slider = new UISlider(sliderRect) {
                    BackgroundColor = UIColor.Clear,
                    MinValue = this.MinValue,
                    MaxValue = this.MaxValue,
                    Continuous = this.Continuous,
                    Value = this.Value,
                    Tag = 1
                };
                _slider.ValueChanged += delegate {
                    Value = (int)_slider.Value;
                    if (UseCaptionForValueDisplay) {
                        Caption = Value.ToString();
                        // force repaint/redraw
                        if (GetContainerTableView() != null) {
                            var root = GetImmediateRootElement();
                            root.Reload(this, UITableViewRowAnimation.None);
                        }
                    }
                    if (_valueChangedCallback != null)
                        _valueChangedCallback(Value);
                };
            }
            else {
                _slider.Value = Value;
                _slider.Frame = sliderRect;
            }

            if (_lockable){
                if (_lockImageView == null)
                    _lockImageView = new UIButton(new CGRect(_slider.Frame.X + _slider.Frame.Width, 2f, lockImageWidth, LockImageHeight));
                
                _lockImageView.SetBackgroundImage((_valueLocked) ? LockImage : UnlockImage, UIControlState.Normal);
                _lockImageView.TouchUpInside += (object sender, EventArgs e) => {
                    _valueLocked = !_valueLocked;
                    _lockImageView.SetBackgroundImage((_valueLocked) ? LockImage : UnlockImage, UIControlState.Normal);
                    if (_valueLocked)
                        _slider.Enabled = (!_valueLocked);
                };
                cell.ContentView.AddSubview(_lockImageView);
            }
            cell.ContentView.AddSubview(_slider);
            return cell;
        }

        public override string Summary()
        {
            return Value.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_slider != null)
                {
                    _slider.Dispose();
                    _slider = null;
                }
            }
        }

        public void SetValue(int f)
        {
            if (!IsLocked && _slider != null)
                _slider.SetValue(f, false);
        }

        public void SetCaption(string caption)
        {
            Caption = caption;
            // force repaint/redraw
            if (GetContainerTableView() != null) {
                var root = GetImmediateRootElement();
                root.Reload(this, UITableViewRowAnimation.None);
            }
        }
    }
}