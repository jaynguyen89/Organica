import React from 'react';

import ReminderCard from './ReminderCard';

const YourReminder = (props: any) => {
    return (
        <>
            <h5>
                <i className="fas fa-bell hidro-primary-icon"></i>
                &nbsp;&nbsp;Your Reminders
            </h5>

            <div className='row'>
                <div className='col xl3 l4 m6 s12'>
                    <div className='card'>
                        <div className='card-content'>
                            <span className='card-title'>Inbox</span>
                            <p>You have an unread message from seller/buyer <a>Furniturize</a>. Please respond ASAP.</p>
                        </div>
                        <div className='card-action'>
                            <a href='/'>Open Message</a>
                            <a href='/' className='right'>Ignore</a>
                        </div>
                    </div>
                </div>
                <div className='col xl3 l4 m6 s12'>
                    <div className='card'>
                        <div className='card-content'>
                            <span className='card-title'>Inbox</span>
                            <p>Seller/buyer <a>GarageOnline</a> has sent a question about your purchased/selling item <a>Samsung Galaxy Note 10+ 256GB</a>. Please reply ASAP.</p>
                        </div>
                        <div className='card-action'>
                            <a href='/'>Open Message</a>
                            <a href='/' className='right'>Ignore</a>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default YourReminder;