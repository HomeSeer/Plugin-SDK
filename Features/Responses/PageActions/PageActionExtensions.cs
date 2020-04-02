using System;

namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// Extension methods for members of the <see cref="HomeSeer.PluginSdk.Features.Responses.PageActions"/> namespace
    /// </summary>
    public static class PageActionExtensions {

        /// <summary>
        /// Get the page action key associated with a particular <see cref="EPageAction"/>
        /// </summary>
        /// <param name="pageAction">The <see cref="EPageAction"/> to get the key for</param>
        /// <returns>The key string used on feature page javascript for the specified <see cref="EPageAction"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid <see cref="EPageAction"/> is specified</exception>
        public static string GetKey(this EPageAction pageAction) {
            switch (pageAction) {
                case EPageAction.Show:
                    return "show";
                case EPageAction.Hide:
                    return "hide";
                case EPageAction.SetText:
                    return "set_text";
                case EPageAction.SetHtml:
                    return "set_html";
                case EPageAction.SetValue:
                    return "set_value";
                case EPageAction.NextStep:
                    return "next_step";
                case EPageAction.PreviousStep:
                    return "prev_step";
                case EPageAction.SetStep:
                    return "set_step";
                case EPageAction.Callback:
                    return "callback";
                default:
                    throw new ArgumentOutOfRangeException(nameof(pageAction), pageAction, null);
            }
        }
    }
}