﻿using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Extension
{
    public static class ImageButtonExtension
    {
        public static ImageButton BindCommand(this ImageButton self, string path,
            BindingMode mode = BindingMode.Default, IValueConverter converter = null,
            string stringFormat = null)
        {
            return BaseExtension.Bind(self, ImageButton.CommandProperty, path, mode, converter, stringFormat);
        }
    }
}