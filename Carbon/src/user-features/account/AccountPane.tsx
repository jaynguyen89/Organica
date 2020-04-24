import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';

import AccountShow from './sub-contents/AccountShow';
import SecurityPane from './sub-contents/SecurityPane';
import CarbonPreloader from '../../shared/CarbonPreloader';
import CarbonAlert from '../../shared/CarbonAlert';

import { IIdentity, ITwoFa } from './redux/constants';
import {
    getAccountIdentity,
    getAccountTimeStamps,
    getAccountTwoFaData
} from './redux/actions';

const mapStateToProps = (state: any) => ({
    user : state.AuthenticationStore.authUser,
    getIdentity : state.AccountStore.getIdentity,
    getTwoFa : state.AccountStore.getTwoFa,
    getTimeStamps : state.AccountStore.getTimeStamps
});

const mapDispatchToProps = {
    getAccountIdentity,
    getAccountTimeStamps,
    getAccountTwoFaData
};

const AccountPane = (props : any) => {
    const [identity, setIdentity] = React.useState(null as unknown as IIdentity);
    const [twoFa, setTwoFa] = React.useState(null as unknown as ITwoFa);

    React.useEffect(() => {
        const {
            getAccountIdentity,
            getAccountTimeStamps,
            getAccountTwoFaData
        } = props;

        getAccountIdentity(props.user.userId);
        getAccountTimeStamps(props.user.userId);
        getAccountTwoFaData(props.user.userId);
    }, []);

    React.useEffect(() => {
        if (props.getIdentity.getResult && props.getIdentity.getResult.hasOwnProperty('result') &&
            props.getIdentity.getResult.result === 1)
            setIdentity(props.getIdentity.getResult.message);

        if (props.getTwoFa.getResult && props.getTwoFa.getResult.hasOwnProperty('result') &&
            props.getTwoFa.getResult.result === 1 && props.getTwoFa.getResult.hasOwnProperty('message'))
            setTwoFa(props.getTwoFa.getResult.message);
    }, [props]);

    return (
        <div className='row'>
            <h6 className='content-caption'>
                <i className='fas fa-user-circle hidro-primary-icon'></i>&nbsp;&nbsp;Signup Information
            </h6>

            <div className='col l6 m12'>
                { props.getIdentity.isSending && <CarbonPreloader /> }
                {
                    !props.getIdentity.isSending && !props.getIdentity.getSuccess && (_.isEmpty(props.getIdentity.getResult) ||
                    (!_.isEmpty(props.getIdentity.getResult) && props.getIdentity.getResult.hasOwnProperty('stack'))) &&
                    <CarbonAlert messages='Unable to get your account information due to network lost. Please check your network connection.' type='warning' persistent />
                }

                {
                    !props.getIdentity.isSending && props.getIdentity.getSuccess && !_.isEmpty(props.getIdentity.getResult)
                    && props.getIdentity.getResult.hasOwnProperty('result') && props.getIdentity.getResult.result !== 1 &&
                    <CarbonAlert messages={ props.getIdentity.getResult.message } type='error' persistent />
                }

                {
                    !props.getIdentity.isSending && props.getIdentity.getSuccess && !_.isEmpty(props.getIdentity.getResult)
                    && props.getIdentity.getResult.hasOwnProperty('result') && props.getIdentity.getResult.result === 1 &&
                    <AccountShow
                        user={ props.user }
                        identity={ identity } />
                }
            </div>
            <div className='col l6 m12'>
                { props.getTwoFa.isSending && <CarbonPreloader /> }
                {
                    !props.getTwoFa.isSending && !props.getTwoFa.getSuccess && (_.isEmpty(props.getTwoFa.getResult) ||
                    (!_.isEmpty(props.getTwoFa.getResult) && props.getTwoFa.getResult.hasOwnProperty('stack'))) &&
                    <CarbonAlert messages='Unable to get your security information due to network lost. Please check your network connection.' type='warning' persistent />
                }

                {
                    !props.getTwoFa.isSending && props.getTwoFa.getSuccess && !_.isEmpty(props.getTwoFa.getResult)
                    && props.getTwoFa.getResult.hasOwnProperty('result') && props.getTwoFa.getResult.result !== 1 &&
                    <CarbonAlert messages={ props.getTwoFa.getResult.message } type='error' persistent />
                }

                {
                    !props.getTwoFa.isSending && props.getTwoFa.getSuccess && !_.isEmpty(props.getTwoFa.getResult)
                    && props.getTwoFa.getResult.hasOwnProperty('result') && props.getTwoFa.getResult.result === 1 &&
                    <SecurityPane
                        user={ props.user }
                        twoFa={ props.getTwoFa.getResult.hasOwnProperty('message') ? twoFa : null as unknown as ITwoFa } />
                }
            </div>

            <div className='col s12'>
                { props.getTimeStamps.isSending && <CarbonPreloader /> }
                {
                    !props.getTimeStamps.isSending && !props.getTimeStamps.getSuccess && (_.isEmpty(props.getTimeStamps.getResult) ||
                    (!_.isEmpty(props.getTimeStamps.getResult) && props.getTimeStamps.getResult.hasOwnProperty('stack'))) &&
                    <CarbonAlert messages='A problem happenned while getting timestamps for your account. We will try again later.' type='warning' persistent />
                }

                {
                    !props.getTimeStamps.isSending && props.getTimeStamps.getSuccess && !_.isEmpty(props.getTimeStamps.getResult) &&
                    props.getTimeStamps.getResult.hasOwnProperty('result') && props.getTimeStamps.getResult.result === 0 &&
                    <CarbonAlert messages={ props.getTimeStamps.getResult.message } type='warning' persistent />
                }

                {
                    !props.getTimeStamps.isSending && props.getTimeStamps.getSuccess && !_.isEmpty(props.getTimeStamps.getResult) &&
                    props.getTimeStamps.getResult.hasOwnProperty('result') && props.getTimeStamps.getResult.result === 1 &&
                    <div className='row'>
                        <hr style={{ width:'100%' }} />
                        <div className='col m3 s6'>
                            <p className='account-show'><b>Signup On:</b> { props.getTimeStamps.getResult.message.registeredOn }</p>
                        </div>
                        <div className='col m3 s6'>
                            <p className='account-show'><b>Last Update:</b> { props.getTimeStamps.getResult.message.lastUpdate }</p>
                        </div>
                        <div className='col m3 s6'>
                            <p className='account-show'><b>Last Online:</b> { props.getTimeStamps.getResult.message.lastOnline }</p>
                        </div>
                        <div className='col m3 s6'>
                            <p className='account-show'><b>Last Signout:</b> { props.getTimeStamps.getResult.message.lastOffline }</p>
                        </div>
                        <div className='col m3 s6'>
                            <p className='account-show'><b>Last Device:</b> { props.getTimeStamps.getResult.message.lastDevice }</p>
                        </div>
                        <div className='col m3 s6'>
                            <p className='account-show'><b>Last IP:</b> { props.getTimeStamps.getResult.message.lastIpAddress }</p>
                        </div>
                        <div className='col m3 s6'>
                            <p className='account-show'><b>Last Location:</b> { props.getTimeStamps.getResult.message.lastLocation }</p>
                        </div>
                        <div className='col m3 s6'>
                            <p className='account-show'><b>Last Browser:</b> { props.getTimeStamps.getResult.message.lastBrowser }</p>
                        </div>
                    </div>
                }
            </div>
        </div>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(AccountPane);