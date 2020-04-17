import { sendRequestForResult } from '../../../../../providers/serviceProvider';
import { IAddressBinder } from './constants';

let ADDRESS_CONTROLLER = 'address/';
let COUNTRY_CONTROLLER = 'country/';

export const getAddressListFor = (hidrogenianId: number) => {
    return sendRequestForResult(ADDRESS_CONTROLLER + 'address-list/' + hidrogenianId, null, null, 'GET');
}

export const getCountriesForDropdown = () => {
    return sendRequestForResult(COUNTRY_CONTROLLER + 'compact-countries', null, null, 'GET');
}

export const saveNewAddress = (addressBinder: IAddressBinder) => {
    return sendRequestForResult(ADDRESS_CONTROLLER + 'add-address', null, addressBinder, 'POST');
}