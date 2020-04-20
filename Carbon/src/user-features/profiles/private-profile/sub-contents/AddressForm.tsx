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
            onShow : () => props.setCurrentTab($('.active').attr('href')?.substr(1) || 'standard')
        });
    }, [props.currentTab]);

    React.useEffect(() => {
        M.Tabs.getInstance(
            document.querySelector('.tabs') as Element
        ).select(props.currentTab);

        M.Tabs.getInstance(
            document.querySelector('.tabs') as Element
        ).updateTabIndicator();
    }, [props.address]);

    React.useEffect(() => {
        if (props.actionError.length !== 0)
            setStatus({ messages : props.actionError, type : 'error' });
        else setStatus({} as IStatus);
    }, [props.actionError]);

    return (
        <div className='row'>
            <div className='col s12'>
                <p className='header'>Select a format and go. We only save address on the tab you are openning.</p>
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
                    <button className='btn left' onClick={ () => props.saveAddress() }>{ (props.isUpdating && 'Update') || 'Save' }</button>
                    <button className='btn right red' onClick={ () => props.closeModal() }>Cancel</button>
                </div>
            </div>
        </div>
    );
}

export default AddressForm;