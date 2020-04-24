import React, { Component } from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import M from 'materialize-css';
import $ from 'jquery';

import { Paper, Tooltip } from '@material-ui/core';
import CarbonPreloader from '../../../shared/CarbonPreloader';
import CarbonAlert from '../../../shared/CarbonAlert';
import IdentityForm from './IdentityForm';
import PaymentMethodForm from './PaymentMethodForm';

import { IAccountShow, IIdentity, IPaymentInfo } from '../redux/constants';
import { CONSTANTS, ModalOptions } from '../../../helpers/helper';
import {
    getPaymentInformation,
    saveAccountIdentity,
    savePaymentInfo
} from '../redux/actions';
import { updateIdentityFields, updatePaymentFields } from '../utility';

const mapStateToProps = (state: any) => ({
    getPaymentInfo : state.AccountStore.getPaymentInfo
});

const mapDispatchToProps = {
    getPaymentInformation,
    saveAccountIdentity,
    savePaymentInfo
};

const AccountShow = (props : IAccountShow) => {
    const [identity, setIdentity] = React.useState(null as unknown as IIdentity);
    const [paymentInfo, setPaymentInfo] = React.useState(null as unknown as IPaymentInfo);
    const [backup, setBackup] = React.useState({
        identity, paymentInfo
    });

    React.useEffect(() => {
        const { getPaymentInformation } = props;
        getPaymentInformation(props.user.userId);
    }, []);

    React.useEffect(() => {
        M.Modal.init($('.modal'), ModalOptions);
    }, [props]);

    React.useEffect(() => {
        if (!_.isEmpty(props.identity)) {
            setIdentity(props.identity);
            setBackup({ ...backup, identity : props.identity });
        }
    }, [props.identity]);

    React.useEffect(() => {
        if (props.getPaymentInfo.getResult && props.getPaymentInfo.getResult.hasOwnProperty('result') &&
            props.getPaymentInfo.getResult.result === 1) {
            setPaymentInfo(props.getPaymentInfo.getResult.message);
            setBackup({ ...backup, paymentInfo : props.getPaymentInfo.getResult.message });
        }
    }, [props.getPaymentInfo]);

    const closeModal = () => {
        M.Modal.getInstance(document.querySelector('#identity-form') as Element).close();
        M.Modal.getInstance(document.querySelector('#payment-form') as Element).close();

        setBackup({ identity : identity, paymentInfo : paymentInfo });
    }

    const setIdentityFields = (field: string, value: string) => {
        updateIdentityFields(backup, setBackup, field, value);
    }

    const setPaymentMethodFields = (field: string, value : string) => {
        updatePaymentFields(backup, setBackup, field, value);
    }

    const saveNewIdentity = () => {
        const { saveAccountIdentity } = props;
        saveAccountIdentity(backup.identity);

        setBackup({ ...backup, identity : identity });
    }

    const saveNewPaymentMethod = () => {
        const { savePaymentInfo } = props;
        savePaymentInfo(backup.paymentInfo);

        setBackup({ ...backup, paymentInfo : paymentInfo });
    }

    return (
        identity &&
        <Paper className='content-container'>
            <div className='row'>
                <div className='col s12'>
                    <p>
                        <b>Identity</b>
                        <Tooltip title='Update Identity' placement='right' arrow>
                            <i className='fas fa-edit link-icon modal-trigger' data-target='identity-form'></i>
                        </Tooltip>
                        
                    </p>
                    <p className='account-show'>
                        <b>Email:</b> { identity.email }&nbsp;
                        {
                            (
                                identity.emailConfirmed &&
                                <Tooltip title='Email Confirmed' placement='right' arrow>
                                    <i className='fas fa-check-circle green-text'></i>
                                </Tooltip>
                            ) ||
                            <Tooltip title='Email Not Confirmed' placement='right' arrow>
                                <i className='fas fa-times-circle green-text'></i>
                            </Tooltip>
                        }
                    </p>
                </div>
                <div className='col m6 s12'>
                    <p className='account-show'>
                        <b>Username:</b> { identity.userName }
                    </p>
                </div>
                <div className='col m6 s12'>
                    <p className='account-show'>
                        <b>Phone Number:</b> { identity.phoneNumber || CONSTANTS.NA }&nbsp;
                        {
                            identity.phoneNumber && (
                                (
                                    identity.phoneConfirmed &&
                                    <Tooltip title='Phone Number Confirmed' placement='right' arrow>
                                        <i className='fas fa-check-circle green-text'></i>
                                    </Tooltip>
                                ) ||
                                <Tooltip title='Phone Number Not Confirmed' placement='right' arrow>
                                    <i className='fas fa-times-circle green-text'></i>
                                </Tooltip>
                            )
                        }
                    </p>
                </div>

                <div id='identity-form' className='modal' style={{ width:'25%' }}>
                    <div className='modal-content'>
                        <h5>
                            <i className='fas fa-user-edit hidro-primary-icon'></i>&nbsp;&nbsp;
                            Update Identity
                        </h5>
                        <IdentityForm
                            identity={ backup.identity }
                            closeModal={ closeModal }
                            updateIdentity={ setIdentityFields }
                            saveIdentity={ saveNewIdentity } />
                    </div>
                </div>

                <div className='col s12'>
                    { props.getPaymentInfo.isSending && <CarbonPreloader /> }
                    {
                        !props.getPaymentInfo.isSending && !props.getPaymentInfo.getSuccess && (_.isEmpty(props.getPaymentInfo.getResult) ||
                        (!_.isEmpty(props.getPaymentInfo.getResult) && props.getPaymentInfo.getResult.hasOwnProperty('stack'))) &&
                        <CarbonAlert messages='Unable to get your payment information due to network lost. Please refresh page to try again.' type='warning' persistent />
                    }

                    {
                        !props.getPaymentInfo.isSending && props.getPaymentInfo.getSuccess && !_.isEmpty(props.getPaymentInfo.getResult) &&
                        props.getPaymentInfo.getResult.hasOwnProperty('result') && props.getPaymentInfo.getResult.result !== 1 &&
                        <CarbonAlert messages={ props.getPaymentInfo.getResult.message } type='warning' persistent />
                    }

                    {
                        !props.getPaymentInfo.isSending && props.getPaymentInfo.getSuccess && !_.isEmpty(props.getPaymentInfo.getResult) &&
                        props.getPaymentInfo.getResult.hasOwnProperty('result') && props.getPaymentInfo.getResult.result === 1 &&
                        <>
                            <p>
                                <b>Payment Types</b>
                                <Tooltip title='Update Payment Method' placement='right' arrow>
                                    <i className='fas fa-edit link-icon modal-trigger' data-target='payment-form'></i>
                                </Tooltip>
                            </p>
                            <p className='account-show'>
                                <b>Account Balance:</b>&nbsp;
                                { (paymentInfo && paymentInfo.paymentMethod.accountBalance) || 'None.' }
                                <a href='/' className='right'>Add balance</a>
                            </p>
                            <p className='account-show'>
                                <b>Balance date:</b>&nbsp;
                                { (paymentInfo && paymentInfo.paymentMethod.accountBalance && paymentInfo.paymentMethod.balanceDate) || CONSTANTS.NA }
                            </p>

                            <br />
                            <p className='account-show'>
                                <b>Card:</b>&nbsp;
                                { (paymentInfo && paymentInfo.paymentMethod.creditCard?.cardNumber) || 'You have added no credit card.' }
                            </p>
                            <p className='account-show'>
                                <b>Paypal:</b>&nbsp;
                                { (paymentInfo && paymentInfo.paymentMethod.paypal?.email) || 'You do not add a Paypal account.' }
                            </p>

                            <div id='payment-form' className='modal'>
                                <div className='modal-content'>
                                    <h5>
                                        <i className='fas fa-user-edit hidro-primary-icon'></i>&nbsp;&nbsp;
                                        Update Payment Methods
                                    </h5>
                                    <PaymentMethodForm
                                        paymentInfo={ backup.paymentInfo }
                                        closeModal={ closeModal }
                                        updatePaymentInfo={ setPaymentMethodFields }
                                        savePaymentInfo={ saveNewPaymentMethod } />
                                </div>
                            </div>
                        </>
                    }
                </div>
            </div>
        </Paper>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(AccountShow);