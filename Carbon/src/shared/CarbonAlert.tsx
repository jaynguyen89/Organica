import React from 'react';
import _ from 'lodash';
import $ from 'jquery';
import './style.css';

const ALERT_CLASS = {
    'error' : 'red darken-4',
    'success' : 'green darken-4',
    'warning' : 'yellow darken-4',
};

export interface IStatus {
    messages: string | string[];
    error?: number,
    type: string,
    persistent?: Boolean
}

const CarbonAlert = ({ messages, error, type, persistent } : IStatus) => {
    React.useEffect(() => {
        if (!persistent)
            setTimeout(() => {
                $('.card-panel').hide();
            }, 60000);
    }, []);

    React.useEffect(() => {
        if (messages && messages.length !== 0 && $('.card-panel').is(':hidden'))
            $('.alert').show();
    }, [messages]);

    const hideAlert = () => { $('.card-panel').hide(); }

    return (
        <>
        {
            messages && messages.length !== 0 &&
            <div className={ 'card-panel ' + (ALERT_CLASS as any)[type] }>
                <i className="fas fa-times right alert-close" onClick={ hideAlert }></i>
                {
                    error != undefined && <h6>Error: { error }</h6>
                }
                {
                    (
                        _.isString(messages) && <span>{ messages }</span>
                    ) ||
                    (messages as string[]).map((m: string, i: number) =>
                        <p key={ i }>{ m }</p>
                    )
                }
            </div>
        }
        </>
    );
}

export default CarbonAlert;