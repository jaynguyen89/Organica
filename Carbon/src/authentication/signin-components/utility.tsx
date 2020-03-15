import _ from 'lodash';
import { CONSTANTS } from '../../helpers/helper';

export const displayAuthMessages = (props: any, setStatus: any) => {
    if (!props.authSending && !props.authSuccess && !_.isEmpty(props.authResult) && props.authResult.hasOwnProperty('stack'))
        setStatus({ messages : 'We\'re unable to send your login credentials to server due to network connection lost. Please check your network.', type : 'error' });
    
    if (!props.authSending && props.authSuccess && !_.isEmpty(props.authResult) && _.isBoolean(props.authResult.result) && props.authResult.hasOwnProperty('hostName'))
        setStatus({ messages : 'We\'re unable to verify your humanity. Please confirm ReCaptcha again.', type : 'error' });

    if (!props.authSending && props.authSuccess && !_.isEmpty(props.authResult) && _.isNumber(props.authResult.result) && props.authResult.result === CONSTANTS.FAILED)
        setStatus({ messages : props.authResult.message, type : 'error' });
    
    if (!props.authSending && props.authSuccess && !_.isEmpty(props.authResult) && _.isNumber(props.authResult.result) && props.authResult.result === CONSTANTS.SUCCESS) {
        setStatus({ messages : 'Hello! ' + props.authResult.message.userName + '. Welcome to Hidrogen.', type : 'success' });
        window.location.href = '/';
    }
}