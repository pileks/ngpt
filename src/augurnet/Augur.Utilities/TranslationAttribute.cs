using System;

namespace Augur.Utilities
{
    public class TranslationAttribute : Attribute
    {
        public string Translation { get; set; }

        public TranslationAttribute(string translation)
        {
            Translation = translation;
        }
    }
}