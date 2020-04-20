import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import M from 'materialize-css';
import $ from 'jquery';

import CarbonAlert from '../../../../shared/CarbonAlert';
import AddressForm from '../sub-contents/AddressForm';
import AddressRow from './AddressRow';

import { IAddressList, IAddress, VOID_ADDRESS, IAddressBinder, IFieldSetter } from '../redux/address/constants';
import { ModalOptions, CONSTANTS } from '../../../../helpers/helper';
import { saveAddressFor, updateAddressFor, deleteAddress, updateAddressField } from '../redux/address/actions';
import {
    setAddressValues,
    checkAddressSavingResult,
    checkAddressUpdatingResult,
    checkAddressDeletingResult,
    checkAddressSetFieldResult
} from '../utility';

const mapStateToProps = (state: any) => ({
    saveAddress : state.AddressStore.saveAddress,
    updating : state.AddressStore.updateAddress,
    setField : state.AddressStore.setField,
    deleting : state.AddressStore.deleteAddress
});

const mapDispatchToProps = {
    saveAddressFor,
    updateAddressFor,
    deleteAddress,
    updateAddressField
};

const AddressList = (props : IAddressList) => {
    const [addresses, setAddresses] = React.useState([]);
    const [countries, setCountries] = React.useState([]);
    const [selectedAddress, setSelectedAddress] = React.useState(VOID_ADDRESS as IAddress);
    const [currentTab, setCurrentTab] = React.useState('standard');
    const [isUpdating, setIsUpdating] = React.useState(false);
    const [readyToSave, setReadyToSave] = React.useState(false);
    const [actionResultError, setActionResultError] = React.useState(CONSTANTS.EMPTY);

    React.useEffect(() => {
        M.Modal.init($('.modal'), ModalOptions);
    }, []);

    React.useEffect(() => {
        if (!_.isEmpty(props.addresses)) setAddresses(props.addresses);
        if (!_.isEmpty(props.countries)) setCountries(props.countries);
    }, [props.addresses]);

    const updateSelectedAddress = (type: string = 'standard', field: string, value: any) => {
        setAddressValues(type, field, value, selectedAddress, setSelectedAddress, countries);
    }

    const saveAddress = () => {
        if (currentTab === 'standard')
            setSelectedAddress({
                ...selectedAddress,
                isStandard : true,
                lAddress : null
            });
        else
            setSelectedAddress({
                ...selectedAddress,
                sAddress : null
            });

        setReadyToSave(true);
    }

    React.useEffect(() => {
        if (readyToSave) {
            let addressBinder : IAddressBinder = {
                hidrogenianId : props.user.userId,
                localAddress : currentTab === 'standard' ? null : selectedAddress,
                standardAddress : currentTab === 'standard' ? selectedAddress : null
            };

            if (!isUpdating) {
                const { saveAddressFor } = props;
                saveAddressFor(addressBinder);
            }
            else {
                const { updateAddressFor } = props;
                updateAddressFor(addressBinder);
            }

            setReadyToSave(false);
        }
    }, [readyToSave]);

    React.useEffect(() => {
        let message = checkAddressSavingResult(props.saveAddress);
        if (!_.isEmpty(props.saveAddress.newAddress) &&
            props.saveAddress.newAddress.hasOwnProperty('result') &&
            props.saveAddress.newAddress.result === 1
        ) {
            let newAddress = props.saveAddress.newAddress.message;

            props.onSaveSuccess(newAddress as IAddress);
            closeModalForm();
            setSelectedAddress(VOID_ADDRESS);
        }
        else setActionResultError(message);
    }, [props.saveAddress]);

    const closeModalForm = () => {
        M.Modal.getInstance(
            document.querySelector('.modal') as Element
        ).close();
        setIsUpdating(false);
        setReadyToSave(false);
        setSelectedAddress(VOID_ADDRESS);
        setCurrentTab('standard');
    }

    const autoDetectAddress = () => {
        alert('auto detect address');
    }

    const openModalForUpdateAddress = (address: IAddress) => {
        setSelectedAddress(address);
        M.Modal.getInstance(
            document.querySelector('.modal') as Element
        ).open();

        setIsUpdating(true);
        setCurrentTab(address.isStandard ? 'standard' : 'local');
    }

    React.useEffect(() => {
        let result = checkAddressUpdatingResult(props.updating);
        if (!_.isEmpty(props.updating.updatedAddress) &&
            props.updating.updatedAddress.hasOwnProperty('result') &&
            props.updating.updatedAddress.result === 1
        ) {
            let newAddress = props.updating.updatedAddress.message;

            props.onUpdateSuccess(newAddress as IAddress);
            closeModalForm();
            setSelectedAddress(VOID_ADDRESS);
        }
        else setActionResultError(result);
    }, [props.updating]);

    const deleteAddress = (addressId: number) => {
        if (window.confirm('The address will be deleted completely from your account. Continue?')) {
            const { deleteAddress } = props;
            deleteAddress(addressId);
        }
    }

    React.useEffect(() => {
        let result = checkAddressDeletingResult(props.deleting);
        if (!_.isEmpty(props.deleting.result) &&
            props.deleting.result.hasOwnProperty('result') &&
            props.deleting.result.result === 1
        ) {
            props.onSetFieldOrDeleteSuccess(CONSTANTS.DELETE);
            closeModalForm();
        }
        else setActionResultError(result);
    }, [props.deleting]);

    const setAddressAsPrimaryOrDelivery = (fieldSetter: IFieldSetter) => {
        fieldSetter.hidrogenianId = props.user.userId;
        const { updateAddressField } = props;

        updateAddressField(fieldSetter);
    }

    React.useEffect(() => {
        let result = checkAddressSetFieldResult(props.setField);
        if (!_.isEmpty(props.setField.result) &&
            props.setField.result.hasOwnProperty('result') &&
            props.setField.result.result === 1
        ) {
            props.onSetFieldOrDeleteSuccess();
            closeModalForm();
        }
        else setActionResultError(result);
    }, [props.setField]);

    return (
        <>
            <div className='add-address-row modal-trigger' data-target='address-form'>
                <i className='fas fa-plus-circle'></i>&nbsp;New address
            </div>

            <div id='address-form' className='modal'>
                <div className='modal-content'>
                    <h5><i className='fas fa-map-marked-alt hidro-primary-icon'></i>&nbsp;&nbsp;Add Address</h5>
                    <AddressForm
                        address={ selectedAddress }
                        countries={ countries }
                        currentTab={ currentTab }
                        setCurrentTab={ setCurrentTab }
                        updateAddress={ updateSelectedAddress }
                        detectAddress={ autoDetectAddress }
                        saveAddress={ saveAddress }
                        closeModal={ closeModalForm }
                        actionError={ actionResultError }
                        isUpdating={ isUpdating } />
                </div>
            </div>

            <div className='address-list'>
                {
                    (
                        _.isEmpty(addresses) &&
                        <CarbonAlert messages='You have added no address. Please add one by clicking the button above.'
                            type='info' persistent={ true } />
                    ) ||
                    addresses.map((address: IAddress) =>
                        <AddressRow key={ address.id } address={ address }
                            deleteAddress={ deleteAddress }
                            updateAddress={ openModalForUpdateAddress }
                            updateField={ setAddressAsPrimaryOrDelivery } />
                    )
                }
            </div>
        </>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(AddressList);