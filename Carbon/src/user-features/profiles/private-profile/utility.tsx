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

export const checkProfileResult = (result: any, setStatus: any) => {
    if (!result.profileUpdating && !result.updateSuccess && _.isEmpty(result.newProfile))
        return false;

    if (!result.profileUpdating && !result.updateSuccess && !_.isEmpty(result.newProfile) && result.newProfile.hasOwnProperty('stack')) {
        setStatus({ messages : 'Unable to update your profile due to network connection lost. Please check your network.', type : 'error' });
        return false;
    }

    if (!result.profileUpdating && result.updateSuccess && !_.isEmpty(result.newProfile) && result.newProfile.hasOwnProperty('result') && result.newProfile.result === 0) {
        setStatus({ messages : 'An error occurred while attempting to update your profile. Please try again.', type : 'error' });
        return false;
    }
    
    return true;
}