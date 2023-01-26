using System.Collections.Generic;
using Augur.Utilities;

namespace Augur.Emails.Models
{
    public abstract class EmailTemplateDefinition
    {
        private readonly string namespaceDefinition;

        protected EmailTemplateDefinition(string name, string subject, string namespaceDefinition)
        {
            this.namespaceDefinition = namespaceDefinition;
            this.Name = name;
            this.Subject = subject;
            this.Tokens = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string Subject { get; set; }
        public virtual IDictionary<string, string> Tokens { get; protected set; }

        public string HtmlBody => this.ReplaceTokens(ResourceReader.Read(this.GetType().Assembly, $"{this.namespaceDefinition}.{this.Name}.{this.Name}.html"));
        public string PlainTextBody => this.ReplaceTokens(ResourceReader.Read(this.GetType().Assembly, $"{this.namespaceDefinition}.{this.Name}.{this.Name}.txt"));

        private string ReplaceTokens(string text)
        {
            foreach (var token in this.Tokens)
            {
                text = text.Replace(token.Key, token.Value);
            }

            return text;
        }
    }
}
