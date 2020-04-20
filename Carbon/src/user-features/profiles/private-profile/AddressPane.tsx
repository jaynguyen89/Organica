import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';

import AddressMap from './sub-contents/AddressMap';
import AddressList from './sub-contents/AddressList';
import CarbonPreloader from '../../../shared/CarbonPreloader';
import CarbonAlert, { IStatus } from '../../../shared/CarbonAlert';

import { CONSTANTS } from '../../../helpers/helper';
import { IAddress } from './redux/address/constants';

import {
    getAddressListFor,
    getCountriesForDropdown
} from './redux/address/actions';
import { checkAddressAndCountryListRetrievingResult } from './utility';

const mapStateToProps = (state: any) => ({
    user : state.AuthenticationStore.authUser,
    addressList : state.AddressStore.addressList,
    countryList : state.AddressStore.countryList,
    saveAddress : state.AddressStore.saveAddress,
    updating : state.AddressStore.updateAddress,
    setField : state.AddressStore.setField,
    deleting : state.AddressStore.deleteAddress
});

const mapDispatchToProps = {
    getAddressListFor,
    getCountriesForDropdown
};

const AddressPane = (props : any) => {
    const [anyError, setAnyError] = React.useState(false);
    const [status, setStatus] = React.useState({ messages : CONSTANTS.EMPTY, type : CONSTANTS.EMPTY } as IStatus);

    const [addresses, setAddresses] = React.useState([]);
    const [countries, setCountries] = React.useState([]);

    React.useEffect(() => {
        if (props.user) {
            const {
                getAddressListFor,
                getCountriesForDropdown
            } = props;
            getAddressListFor(props.user.userId);
            getCountriesForDropdown();
        }
    }, []);

    React.useEffect(() => {
        let addressError = checkAddressAndCountryListRetrievingResult('address', props.addressList, setStatus);
        let countryError = checkAddressAndCountryListRetrievingResult('country', props.countryList, setStatus);
        
        setAnyError(addressError || countryError);
        if (!_.isEmpty(props.addressList.retrieveResult) && !_.isEmpty(props.countryList.retrieveResult)) {
            setAddresses(props.addressList.retrieveResult.message);
            setCountries(props.countryList.retrieveResult.message);
        }
    }, [props]);

    const appendNewAddressOnSaveSuccess = (address: IAddress) => {
        let clonedAddressList = _.cloneDeep(addresses);
        clonedAddressList.push(address);

        setStatus({ messages : 'Your new address has been saved. You can now set it as Primary or for Delivery.', type : 'success' });
        setAddresses(clonedAddressList);
    }

    const replaceAddressOnUpdateSuccess = (address: IAddress) => {
        let clonedAddressList = _.cloneDeep(addresses);
        clonedAddressList.forEach((adr: IAddress) => {
            if (adr.id === address.id) {
                clonedAddressList[clonedAddressList.indexOf(adr)] = address;
                return;
            }
        });

        setStatus({ messages : 'Your address has been updated successfully.', type : 'success' });
        setAddresses(clonedAddressList);
    }

    const refreshAddressListOnSetFieldAndDeleteSuccess = (action: string = CONSTANTS.EMPTY) => {
        if (action === CONSTANTS.DELETE)
            setStatus({ messages : 'The address has been permanently deleted.', type : 'success' });
        else
            setStatus({ messages : 'The label has been successfully set on your address.', type : 'success' });
        
        const { getAddressListFor } = props;
        getAddressListFor(props.user.userId);
    }

    return (
        <div className='row'>
            <h6 className='content-caption'>
                <i className='fas fa-user-circle hidro-primary-icon'></i>&nbsp;&nbsp;Addresses
            </h6>

            {
                (props.addressList.isRetrieving ||
                props.countryList.isRetrieving) &&
                <div className='row center'>
                    <CarbonPreloader />
                </div>
            }

            <CarbonAlert { ...status } />
            {
                !anyError &&
                <>
                    <div className='col l4 m6 s12'>
                        <AddressList
                            user={ props.user }
                            addresses={ addresses }
                            countries={ countries }
                            onSaveSuccess={ appendNewAddressOnSaveSuccess }
                            onUpdateSuccess={ replaceAddressOnUpdateSuccess }
                            onSetFieldOrDeleteSuccess={ refreshAddressListOnSetFieldAndDeleteSuccess } />
                    </div>
                    <div className='col l8 m6 s0'>
                        <AddressMap addresses={ addresses } />
                    </div>
                </>
            }
        </div>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(AddressPane);