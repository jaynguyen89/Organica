import React from 'react';

const ReminderCard = (props: any) => {
    return (
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
    );
}

export default ReminderCard;