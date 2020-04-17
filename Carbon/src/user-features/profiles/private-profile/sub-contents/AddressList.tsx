import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import M from 'materialize-css';
import $ from 'jquery';

import CarbonAlert from '../../../../shared/CarbonAlert';
import AddressForm from '../sub-contents/AddressForm';

import { IAddressList, IAddress, VOID_ADDRESS, VOID_LLOCATION, VOID_SLOCATION, IAddressBinder } from '../redux/address/constants';
import { setAddressValues, checkAddressSavingResult } from '../utility';
import { ModalOptions, CONSTANTS } from '../../../../helpers/helper';
import { saveAddressFor } from '../redux/address/actions';

const mapStateToProps = (state: any) => ({
    saveAddress : state.AddressStore.saveAddress
});

const mapDispatchToProps = {
    saveAddressFor
};

const AddressList = (props : IAddressList) => {
    const [addresses, setAddresses] = React.useState([]);
    const [countries, setCountries] = React.useState([]);
    const [selectedAddress, setSelectedAddress] = React.useState(VOID_ADDRESS as IAddress);
    const [currentTab, setCurrentTab] = React.useState('standard');
    const [readyToSave, setReadyToSave] = React.useState(false);
    const [saveError, setSaveError] = React.useState(CONSTANTS.EMPTY);

    React.useEffect(() => {
        M.Modal.init($('.modal'), ModalOptions);

        M.Dropdown.init($('.dropdown-trigger'), {
            constrainWidth : false,
            closeOnClick : true,
            alignment : 'right'
        });
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
            const { saveAddressFor } = props;

            let addressBinder : IAddressBinder = {
                hidrogenianId : props.user.userId,
                localAddress : currentTab === 'standard' ? null : selectedAddress,
                standardAddress : currentTab === 'standard' ? selectedAddress : null
            };
            saveAddressFor(addressBinder);
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
        else setSaveError(message);
    }, [props.saveAddress]);

    const closeModalForm = () => {
        M.Modal.getInstance(
            document.querySelector('.modal') as Element
        ).close();
    }

    const autoDetectAddress = () => {
        alert('auto detect address');
    }

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
                        saveError={ saveError } />
                </div>
            </div>

            <div className='address-list'>
                {
                    (
                        _.isEmpty(addresses) &&
                        <CarbonAlert messages='You have added no address. Please add one by clicking the button above.'
                            type='info' persistent={ true } />
                    ) ||
                    <div className='address-row'>
                        <h6>
                            Address Title
                            <span className='new blue badge right'>Primary</span>
                            <span className='new orange badge right'>Delivery</span>
                        </h6>
                        <p>Ap.5, Bldg 12A, Block 1B, 111 Somewhere Street, Somewhere Place, VIC 3020</p>
                        <p style={{ fontSize:'12px' }}><b>Last update:</b> 23 Mar 2020 12:33PM</p>
                        <a className='address-edit btn-floating blue dropdown-trigger' data-target='address-options'><i className='material-icons'>edit</i></a>
                        <a className='address-delete btn-floating red'><i className='material-icons'>delete_forever</i></a>
                    </div>
                }

                <ul id='address-options' className='dropdown-content'>
                    <li><a href='#!'>Set as primary</a></li>
                    <li><a href='#!'>Set as delivery</a></li>
                    <li className='divider'></li>
                    <li><a href='#!'>Edit</a></li>
                </ul>
            </div>
        </>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(AddressList);