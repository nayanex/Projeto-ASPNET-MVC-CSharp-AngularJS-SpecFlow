using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo.Metadata;
using System.Drawing;

namespace WexProject.Library.Libs.Xaf
{
    public class ColorValueConverter : ValueConverter
    {
        public override Type StorageType { get { return typeof(Int32); } }
        public override object ConvertToStorageType(object value)
        {
            if (!(value is Color)) return null;
            return ((Color)value).ToArgb();
        }
        public override object ConvertFromStorageType(object value)
        {
            if (!(value is Int32)) return null;
            return Color.FromArgb((Int32)value);
        }
    }
}
