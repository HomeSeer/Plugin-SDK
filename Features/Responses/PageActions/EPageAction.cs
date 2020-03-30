namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A type of action that can be taken on a feature page using a <see cref="FeaturePageAction"/>
    /// </summary>
    public enum EPageAction {
        
        /// <summary>
        /// Show an element
        /// </summary>
        Show,
        
        /// <summary>
        /// Hide an element
        /// </summary>
        Hide,
        
        /// <summary>
        /// Set the text on an element
        /// </summary>
        SetText,
        
        /// <summary>
        /// Set the HTML content of an element
        /// </summary>
        SetHtml,
        
        /// <summary>
        /// Set the value of an element
        /// </summary>
        SetValue,
        
        /// <summary>
        /// Go to the next step on an mdbootstrap stepper
        /// </summary>
        NextStep,
        
        /// <summary>
        /// Go to the previous step on an mdbootstrap stepper
        /// </summary>
        PreviousStep,
        
        /// <summary>
        /// Set the current step on an mdboostrap stepper
        /// </summary>
        SetStep,
        
        /// <summary>
        /// Callback to the plugin after waiting an amount of time
        /// </summary>
        Callback

    }
}