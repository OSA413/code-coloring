using System;
using System.Text;

namespace CodeColoring.OutputFormat
{
    internal class TagToken : IDisposable
    {
        private readonly StringBuilder builder;
        private readonly string tag;

        private TagToken(string tag, StringBuilder builder)
        {
            this.builder = builder;
            this.tag = tag;

            builder.Append($"<{tag}>\n");
        }

        public void Dispose() => builder.Append($"</{tag}>\n");

        public static TagToken Tag(string tag, StringBuilder builder) => new(tag, builder);
    }
}