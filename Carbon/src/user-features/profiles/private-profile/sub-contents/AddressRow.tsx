import React from 'react';
import M from 'materialize-css';
import $ from 'jquery';
import _ from 'lodash';

import { IAddress, IFieldSetter } from '../redux/address/constants';
import CarbonPreloader from '../../../../shared/CarbonPreloader';
import CarbonFab from '../../../../shared/CarbonFab';

const AddressRow = (props: any) => {
    const [address, setAddress] = React.useState(null as unknown as IAddress);

    React.useEffect(() => {
        M.FloatingActionButton.init($('.fixed-action-btn'), {
            direction : 'left'
        });
    }, []);

    React.useEffect(() => {
        setAddress(props.address);
        M.FloatingActionButton.init($('.fixed-action-btn'), {
            direction : 'left'
        });
    }, [props]);

    return (
        (
            _.isEmpty(address) && <CarbonPreloader />
        ) ||
        <>
            <div className='address-row'>
                <h6>
                    { address.title }
                    {
                        address.isPrimary &&
                        <span className='new blue badge right'>Primary</span>
                    }
                    {
                        address.forDelivery &&
                        <span className='new orange badge right'>Delivery</span>
                    }
                </h6>
                <p>{ address.normalizedAddress }</p>
                <p style={{ fontSize:'12px' }}><b>Last update:</b>&nbsp;{ address.lastUpdate }</p>
                
                <CarbonFab
                    fab={{ tooltip: 'Actions', icon: 'fas fa-hand-point-up' }}
                    actions={
                        address.isPrimary ? (
                            address.forDelivery ? [{ name: 'Edit', icon: 'fas fa-pen-nib green-text', handleActionClicked: () => props.updateAddress(address) }]
                                                : [{ name: 'Delivery Address', icon: 'fas fa-truck blue-text', handleActionClicked: () => props.updateField({ id : address.id, field : 'isDeliveryAddress' } as IFieldSetter) },
                                                   { name: 'Edit', icon: 'fas fa-pen-nib green-text', handleActionClicked: () => props.updateAddress(address) }]
                        ) : (
                            address.forDelivery ? [{ name: 'Primary Address', icon: 'fas fa-home blue-text', handleActionClicked: () => props.updateField({ id : address.id, field : 'isPrimaryAddress' } as IFieldSetter) },
                                                   { name: 'Edit', icon: 'fas fa-pen-nib green-text', handleActionClicked: () => props.updateAddress(address) }]
                                                : [{ name: 'Primary Address', icon: 'fas fa-home blue-text', handleActionClicked: () => props.updateField({ id : address.id, field : 'isPrimaryAddress' } as IFieldSetter) },
                                                   { name: 'Delivery Address', icon: 'fas fa-truck blue-text', handleActionClicked: () => props.updateField({ id : address.id, field : 'isDeliveryAddress' } as IFieldSetter) },
                                                   { name: 'Edit', icon: 'fas fa-pen-nib green-text', handleActionClicked: () => props.updateAddress(address) },
                                                   { name: 'Delete', icon: 'fas fa-trash-alt red-text', handleActionClicked: () => props.deleteAddress(address.id) }]
                        )
                    } />
            </div>
        </>
    );
}

export default AddressRow;