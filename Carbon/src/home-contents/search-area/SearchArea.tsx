import React from 'react';
import './style.css';

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
                    <div className='row'>
                        <div className='input-field col l5 m12 s12'>
                            <i className='material-icons prefix'>search</i>
                            <textarea id='search-anything' className='materialize-textarea'></textarea>
                            <label htmlFor='search-anything'>Search for anything</label>
                            <span className='helper-text'>
                                <a href='/'>Advanced search</a>
                            </span>
                        </div>
                        <div className='input-field col l3 m6 s6'>
                            <i className='material-icons prefix'>location_searching</i>
                            <textarea id='search-location' className='materialize-textarea'></textarea>
                            <label htmlFor='search-location'>Location</label>
                        </div>
                        <div className='input-field col l3 m6 s6'>
                            <select id='search-categories' multiple>
                                <option value='0' selected>Categories</option>
                                <option value='1'>Option 1</option>
                                <option value='2'>Option 2</option>
                                <option value='3'>Option 3</option>
                            </select>
                        </div>
                        <div className='col l1 m12 s12'>
                            <button className='btn btn-large right'>Search</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default SearchArea;