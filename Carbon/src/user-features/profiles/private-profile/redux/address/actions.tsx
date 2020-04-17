import * as addressConstants from './constants';
import * as addressServices from './services';

export const getAddressListFor = (hidrogenianId: number) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : addressConstants.GET_ADDRESS_LIST_BEGIN });

        addressServices.getAddressListFor(hidrogenianId)
        .then((result: any) => {
            dispatch({
                type : addressConstants.GET_ADDRESS_LIST_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : addressConstants.GET_ADDRESS_LIST_FAILED, error });
        });
    };
}

export const getCountriesForDropdown = () => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : addressConstants.GET_DROPDOWN_COUNTRIES_BEGIN });

        addressServices.getCountriesForDropdown()
        .then((result: any) => {
            dispatch({
                type : addressConstants.GET_DROPDOWN_COUNTRIES_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : addressConstants.GET_DROPDOWN_COUNTRIES_FAILED, error });
        });
    };
}

export const saveAddressFor = (addressBinder: addressConstants.IAddressBinder) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : addressConstants.SAVE_NEW_ADDRESS_BEGIN });

        addressServices.saveNewAddress(addressBinder)
        .then((result: any) => {
            dispatch({
                type : addressConstants.SAVE_NEW_ADDRESS_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : addressConstants.SAVE_NEW_ADDRESS_FAILED, error });
        });
    };
}