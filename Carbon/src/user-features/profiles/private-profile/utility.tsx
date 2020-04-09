import _ from 'lodash';

export const checkApiKeyResult = (
    result: any, setStatus: any, setApiKey: any
) => {
    if (!_.isEmpty(result) && result.hasOwnProperty('result') && result.result === 0)
        setStatus({ messages : result.message, type : 'error' });
    else if (!_.isEmpty(result) && result.hasOwnProperty('stack'))
        setStatus({ messages : 'Unable to upload your avatar due to network connection lost. Please check your network.', type : 'error' });
    else if (!_.isEmpty(result) && result.hasOwnProperty('result') && result.result === 1)
        setApiKey(result.message);
}

export const checkAvatarUploadResult = (
    result: any, setStatus: any, setShouldAllowUpload: any
) => {
    if (!_.isEmpty(result) && result.hasOwnProperty('stack'))
        setStatus({ messages : 'Unable to upload your avatar due to network connection lost. Please check your network.', type : 'error' });
    
    if (!_.isEmpty(result) && result.hasOwnProperty('error') && _.isBoolean(result.error) && !result.error)
        setStatus({ messages : result.errorMessage, type : 'error' });

    if (!_.isEmpty(result) && result.hasOwnProperty('error') && _.isBoolean(result.error) && result.error)
        setShouldAllowUpload({
            hasImage : false,
            isUploading : false
        });
}