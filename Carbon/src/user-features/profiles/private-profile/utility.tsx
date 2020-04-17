import _ from 'lodash';
import { IStandardLocation, ILocalLocation, IAddress, ICountry } from './redux/address/constants';
import { CONSTANTS } from '../../../helpers/helper';

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

export const checkAddressAndCountryListRetrievingResult = (listName: string, result : any, setStatus: any) => {
    if (result.isRetrieving) return false;

    if (!result.isRetrieving && !result.retrieveSuccess) {
        setStatus({ messages : 'Unable to retrieve ' +
                (listName === 'address' ? 'your addresses' : 'country data') +
                ' due to network lost. Please check your network connection.',
            type : 'error',
            persistent : true
        });
        return true;
    }

    if (!result.isRetrieving && result.retrieveSuccess && result.retrieveResult.hasOwnProperty('result') && result.retrieveResult.result !== 1) {
        setStatus({ messages : result.message, type : 'error', persistent : true });
        return true;
    }
    
    setStatus({});
    return (!result.isRetrieving && result.retrieveSuccess && result.retrieveResult.hasOwnProperty('result') && result.retrieveSuccess.result === 1);
}

export const setAddressValues = (
    type: string = 'standard', field: string, value: any, selectedAddress: any, setSelectedAddress: any, countries: any
) => {
    if (field === 'title') setSelectedAddress({ ...selectedAddress, title : value });

    let country;
    if (field === 'country') country = countries.filter((c: any) => c.id === +value);

    if (type === 'standard') {
        if (field === 'building') setSelectedAddress({ ...selectedAddress, sAddress : { ...selectedAddress.sAddress, buildingName : value } as IStandardLocation });
        if (field === 'street') setSelectedAddress({ ...selectedAddress, sAddress : { ...selectedAddress.sAddress, streetAddress : value } as IStandardLocation });
        if (field === 'country') setSelectedAddress({ ...selectedAddress, sAddress : { ...selectedAddress.sAddress, country : { id : +value, name : country[0].name } as ICountry } as IStandardLocation });
        if (field === 'alternate') setSelectedAddress({ ...selectedAddress, sAddress : { ...selectedAddress.sAddress, alternateAddress : value } as IStandardLocation });

        if (field === 'suburb') setSelectedAddress({ ...selectedAddress, sAddress : { ...selectedAddress.sAddress, suburb : value } as IStandardLocation });
        if (field === 'state') setSelectedAddress({ ...selectedAddress, sAddress : { ...selectedAddress.sAddress, state : value } as IStandardLocation });
        if (field === 'postcode') setSelectedAddress({ ...selectedAddress, sAddress : { ...selectedAddress.sAddress, postcode : value } as IStandardLocation });
    }
    else {
        if (field === 'building') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, buildingName : value } as ILocalLocation });
        if (field === 'street') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, streetAddress : value } as ILocalLocation });
        if (field === 'country') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, country : { id : +value, name : country[0].name } as ICountry } as ILocalLocation });
        if (field === 'alternate') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, alternateAddress : value } as ILocalLocation });

        if (field === 'group') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, group : value } as ILocalLocation });
        if (field === 'lane') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, lane : value } as ILocalLocation });
        if (field === 'quarter') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, quarter : value } as ILocalLocation });
        if (field === 'hamlet') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, hamlet : value } as ILocalLocation });
        if (field === 'commute') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, commute : value } as ILocalLocation });
        if (field === 'ward') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, ward : value } as ILocalLocation });
        if (field === 'district') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, district : value } as ILocalLocation });
        if (field === 'town') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, town : value } as ILocalLocation });
        if (field === 'province') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.lAddress, province : value } as ILocalLocation });
        if (field === 'city') setSelectedAddress({ ...selectedAddress, lAddress : { ...selectedAddress.sAddress, city : value } as ILocalLocation });
    }
}

export const checkAddressSavingResult = (result: any) => {
    if (result.isSending) return CONSTANTS.EMPTY;

    if (!result.isSending && !result.sendSuccess && !_.isEmpty(result.newAddress) && result.newAddress.hasOwnProperty('stack'))
        return 'Unable to save your new address due to network lost. Please check your network connection.';

    if (!result.isSending && result.sendSuccess && !_.isEmpty(result.newAddress) && result.newAddress.result === 0)
        return result.newAddress.message;
    
    return CONSTANTS.EMPTY;
}