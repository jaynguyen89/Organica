import React from 'react';

const CustomerItems = () => {
    return (
        <>
            <li className='exclusive ex-active'>
                <a href='#' onClick={ () => window.location.href = '/customer-home' }>Home</a>
            </li>
            <li className='exclusive'><a href='/'>World</a></li>
            <li className='li-active'><a href='/'>Selling</a></li>
            <li><a href='/'>Buying</a></li>
            <li><a href='/'>Watching</a></li>
            <li><a href='/'>Following</a></li>
            <li><a href='/'>Recents</a></li>
        </>
    );
}

export default CustomerItems;