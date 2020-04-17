import React from 'react';
import M from 'materialize-css';
import $ from 'jquery';

import { IAddressForm } from '../redux/address/constants';
import { CONSTANTS } from '../../../../helpers/helper';

import StandardAddressForm from './StandardAddressForm';
import LocalAddressForm from './LocalAddressForm';
import CarbonAlert, { IStatus } from '../../../../shared/CarbonAlert';

const AddressForm = (props : IAddressForm) => {
    const [status, setStatus] = React.useState({ messages : CONSTANTS.EMPTY, type : CONSTANTS.EMPTY } as IStatus);

    React.useEffect(() => {
        M.Tabs.init($('.tabs'), {
            duration : 200,
            onShow : () => props.setCurrentTab(props.currentTab === 'standard' ? 'local' : 'standard')
        });
    }, [props.currentTab]);

    React.useEffect(() => {
        if (props.saveError.length !== 0)
            setStatus({ messages : props.saveError, type : 'error' });
        else setStatus({} as IStatus);
    }, [props.saveError]);

    return (
        <div className='row'>
            <div className='col s12'>
                <p className='header'>Select a format and go. We only save address of the format you are selecting.</p>
                <CarbonAlert { ...status } />
            </div>
            
            <div className='col s12'>
                <ul className='tabs'>
                    <li className='tab col s6'><a className='active' href='#standard'>Standard</a></li>
                    <li className='tab col s6'><a href='#local'>Local</a></li>
                </ul>
            </div>

            <div className='col s12'>
                <div id='standard' className='col s12'>
                    <StandardAddressForm { ...props } />
                </div>
                <div id='local' className='col s12'>
                    <LocalAddressForm { ...props } />
                </div>
            </div>

            <div className='modal-footer'>
                <div className='col s12'>
                    <button className='btn left' onClick={ () => props.saveAddress() }>Save</button>
                    <button className='btn right red' onClick={ () => props.closeModal() }>Cancel</button>
                </div>
            </div>
        </div>
    );
}

export default AddressForm;