import React from 'react';
import moment from 'moment-timezone';
import _ from 'lodash';
import $ from 'jquery';
import M from 'materialize-css';

import { IPaymentInfo } from '../redux/constants';

const PaymentMethodForm = (props: any) => {
    const [paymentInfo, setPaymentInfo] = React.useState(null as unknown as IPaymentInfo);

    React.useEffect(() => {
        if (props.paymentInfo) setPaymentInfo(props.paymentInfo);
    }, [props.paymentInfo]);

    return (
        paymentInfo &&
        <div className='row'>
            <div className='col s12'><h6 className='header'>Add Credit Card</h6></div>
            <div className='input-field col l6 m12 s12'>
                <i className='fas fa-file-signature prefix hidro-primary-icon'></i>
                <input id='card-holder' type='text' value={ paymentInfo.paymentMethod.creditCard?.holderName }
                    onChange={ (e: any) => props.savePaymentInfo('holder', e.target.value) } />
                <label htmlFor='card-holder'>Holder Name</label>
            </div>
            <div className='input-field col l6 m12 s12'>
                <i className='fas fa-credit-card prefix hidro-primary-icon'></i>
                <input id='card-number' type='text' value={ paymentInfo.paymentMethod.creditCard?.cardNumber }
                    onChange={ (e: any) => props.savePaymentInfo('number', e.target.value) } />
                <label htmlFor='card-number'>Card Number</label>
            </div>
            <div className='col l6 m12 s12'>
                <div className='row'>
                    <div className='input-field col s6'>
                        <i className='far fa-calendar-alt prefix hidro-primary-icon'></i>
                        <input id='card-date' type='number' max='12' min='1'
                            value={ paymentInfo.paymentMethod.creditCard?.expiryDate?.split('/')[0] }
                            onChange={ (e: any) => props.savePaymentInfo('month', e.target.value) } />
                        <label htmlFor='card-date'>Expiry Month</label>
                    </div>
                    <div className='input-field col s6'>
                        <i className='far fa-calendar-alt prefix hidro-primary-icon'></i>
                        <input id='card-date' type='number'
                            max={ moment().toDate().getFullYear() + 50 }
                            min={ moment().toDate().getFullYear() }
                            value={ paymentInfo.paymentMethod.creditCard?.expiryDate?.split('/')[1] }
                            onChange={ (e: any) => props.savePaymentInfo('year', e.target.value) } />
                        <label htmlFor='card-date'>Expiry Year</label>
                    </div>
                </div>
            </div>
            <div className='input-field col l6 m12 s12'>
                <i className='fas fa-key prefix hidro-primary-icon'></i>
                <input id='card-code' type='text' value={ paymentInfo.paymentMethod.creditCard?.securityCode }
                    onChange={ (e: any) => props.savePaymentInfo('code', e.target.value) } />
                <label htmlFor='card-code'>Security Code</label>
            </div>

            <div className='col s12'><h6 className='header'>Add Paypal</h6></div>
            <div className='input-field col s12'>
                <i className='fas fa-at prefix hidro-primary-icon'></i>
                <input id='paypal-email' type='text' value={ paymentInfo.paymentMethod.paypal?.email }
                    onChange={ (e: any) => props.savePaymentInfo('email', e.target.value) } />
                <label htmlFor='paypal-email'>Email</label>
            </div>

            <div className='col s12'>
                <button className='btn' onClick={ () => props.savePaymentInfo() }>Update</button>
                <button className='btn right grey darken-1' onClick={ () => props.closeModal() }>Cancel</button>
            </div>
        </div>
    );
}

export default PaymentMethodForm;