using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BookHeaven.Web.Infrastructure.TagHelper
{
    [HtmlTargetElement("script", Attributes = "on-content-loaded")]
    public class ScriptTagHelper : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
    {
        public bool OnContentLoaded { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!OnContentLoaded)
            {
                base.Process(context, output);
            }

            var content = output.GetChildContentAsync().Result;
            var javascript = content.GetContent();

            var sb = new StringBuilder();
            sb.Append("document.addEventListener('DOMContentLoaded',");
            sb.Append("function() {");
            sb.Append(javascript);
            sb.Append("});");

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
