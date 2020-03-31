//Generic POST handler for feature pages
function featurePagePost(featureJsonRequest, pageUrl, successCallback = onFeaturePagePostSuccess, errorCallback = onFeaturePagePostError, timeout = 5000, async = true) {
    $.ajax({
        type: "POST",
        async: async,
        url: pageUrl,
        data: featureJsonRequest,
        timeout: timeout,
        success: successCallback,
        error: errorCallback
    });
}

//Default JSONResponse handler
function onFeaturePagePostSuccess(response) {
    if (response == null || response === "") {
        alert("ERROR : POST to page returned nothing");
        return;
    }
    try {
        var responseObj = JSON.parse(response);
        if (responseObj.data.response === "error") {
            //Process error
            alert("ERROR : POST to page errored : " + responseObj.error);
            return;
        }
        if (responseObj.data.response === "page_actions") {
            //Process page actions
            var pageActions = JSON.parse(responseObj.data.page_actions);
            for (var i = 0 ; i < pageActions.length ; i++) {
                var pageAction = pageActions[i];
                switch(pageAction.page_action) {
                    case "show":
                        $(pageAction.selector).show();
                        break;
                    case "hide":
                        $(pageAction.selector).hide();
                        break;
                    case "set_text":
                        $(pageAction.selector).text(pageAction.data);
                        break;
                    case "set_html":
                        $(pageAction.selector).html(pageAction.data);
                        break;
                    case "set_value":
                        $(pageAction.selector).val(pageAction.data);
                        break;
                    case "next_step":
                        $(pageAction.selector).destroyFeedback();
                        $(pageAction.selector).nextStep();
                        break;
                    case "prev_step":
                        $(pageAction.selector).destroyFeedback();
                        $(pageAction.selector).prevStep();
                        break;
                    case "set_step":
                        $(pageAction.selector).destroyFeedback();
                        $(pageAction.selector).openStep(pageAction.data);
                        break;
                }
            }
        }
    } catch (e) {
        alert("ERROR : POST to page returned data but errored unexpectedly while processing it. : " + e.message);
    }
}

//Default POST error handler
function onFeaturePagePostError(response) {
    alert("ERROR : POST to page failed : " + response);
}