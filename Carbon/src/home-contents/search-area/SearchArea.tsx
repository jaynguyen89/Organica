import React from 'react';
import './style.css';

import SearchBar from '../../shared/SearchBar';

const SearchArea = () => {
    return (
        <div className='row search-row'>
            <div id='search-intro' className='row'>
                <p>Your new way to shop. Convenient. Joyful.</p>
                <hr />
                <span>Search now for everything you want</span>
            </div>
            <div className='search-bar'>
                <div className='search-wrapper'>
                    <SearchBar />
                </div>
            </div>
        </div>
    );
}

export default SearchArea;