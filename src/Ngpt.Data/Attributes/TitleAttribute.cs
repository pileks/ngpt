using System;

namespace Ngpt.Platform.Data.Attributes
{
    public class TitleAttribute : Attribute
    {
        public string Title { get; set; }

        public TitleAttribute(string title)
        {
            Title = title;
        }
    }
}
