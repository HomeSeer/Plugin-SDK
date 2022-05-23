namespace HomeSeer.PluginSdk.Features.Responses.PageActions {
    
    /// <summary>
    /// A type of action that can be taken on a feature page using a <see cref="FeaturePageAction"/>
    /// </summary>
    public enum EPageAction {
        
        /// <summary>
        /// Show an element.
        /// </summary>
        /// <remarks>Use the <see cref="ShowPageAction"/> class to generate a <see cref="FeaturePageAction"/> using this type</remarks>
        Show,
        
        /// <summary>
        /// Hide an element
        /// </summary>
        /// <remarks>Use the <see cref="HidePageAction"/> class to generate a <see cref="FeaturePageAction"/> using this type</remarks>
        Hide,
        
        /// <summary>
        /// Set the text on an element
        /// </summary>
        /// <remarks>Use the <see cref="SetTextPageAction"/> class to generate a <see cref="FeaturePageAction"/> using this type</remarks>
        SetText,
        
        /// <summary>
        /// Set the HTML content of an element
        /// </summary>
        /// <remarks>Use the <see cref="SetHtmlPageAction"/> class to generate a <see cref="FeaturePageAction"/> using this type</remarks>
        SetHtml,
        
        /// <summary>
        /// Set the value of an element
        /// </summary>
        /// <remarks>Use the <see cref="SetValuePageAction"/> class to generate a <see cref="FeaturePageAction"/> using this type</remarks>
        SetValue,
        
        /// <summary>
        /// Go to the next step on an mdbootstrap stepper
        /// </summary>
        /// <remarks>Use the <see cref="NextStepPageAction"/> class to generate a <see cref="FeaturePageAction"/> using this type</remarks>
        NextStep,
        
        /// <summary>
        /// Go to the previous step on an mdbootstrap stepper
        /// </summary>
        /// <remarks>Use the <see cref="PreviousStepPageAction"/> class to generate a <see cref="FeaturePageAction"/> using this type</remarks>
        PreviousStep,
        
        /// <summary>
        /// Set the current step on an mdboostrap stepper
        /// </summary>
        /// <remarks>Use the <see cref="SetStepPageAction"/> class to generate a <see cref="FeaturePageAction"/> using this type</remarks>
        SetStep,
        
        /// <summary>
        /// Callback to the plugin after waiting an amount of time
        /// </summary>
        /// <remarks>Use the <see cref="CallbackPageAction"/> class to generate a <see cref="FeaturePageAction"/> using this type</remarks>
        Callback

    }
}