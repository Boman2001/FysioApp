using System;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApp.helpers
{
    [HtmlTargetElement("form", Attributes = "ajax")]
    public class AjaxForm : TagHelper
    {
        public string replaceId { get; set; }
        public string onAjaxBegin { get; set; }
        public string onAjaxComplete { get; set; }
        
        public string onAjaxFailure { get; set; }
        public string id { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("data-ajax", "true");
            output.Attributes.SetAttribute("data-ajax-method", "POST");
            output.Attributes.SetAttribute("data-ajax-mode", "replace");
            output.Attributes.SetAttribute("method", "post");
            output.Attributes.SetAttribute("id", id);

            if (!string.IsNullOrWhiteSpace(onAjaxBegin))
                output.Attributes.SetAttribute("data-ajax-begin", onAjaxBegin);
            if (!string.IsNullOrWhiteSpace(onAjaxComplete))
                output.Attributes.SetAttribute("data-ajax-complete", onAjaxComplete);
            
            if (!string.IsNullOrWhiteSpace(onAjaxFailure))
                output.Attributes.SetAttribute("data-ajax_failure", onAjaxComplete);
            
            

            if (string.IsNullOrWhiteSpace(replaceId))
                throw new Exception("ReplaceId is required!");

            output.Attributes.SetAttribute("data-ajax-update", $"#{replaceId.TrimStart('#')}");
        }
    }
}