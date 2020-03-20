import React from 'react';
import $ from 'jquery';
import M from 'materialize-css';

const SearchBar = () => {
    React.useEffect(() => {
        M.FormSelect.init($('select'), {});
    }, []);

    return (
        <div className='row'>
            <div className='col l6 m12 s12'>
                <div className='search'>
                    <button className='search-field-btn-icon'>
                        <i className='fa fa-search'></i>
                    </button>
                    <input type='text' className='search-field' placeholder='What are you looking for?' />
                </div>
                <span className='helper-text'>
                    <a href='/'>Advanced search</a>
                </span>
            </div>
            <div className='col l3 m6 s12'>
                <div className='search'>
                    <div className='search-field-icon'>
                        <i className='fa fa-map-marker-alt'></i>
                    </div>
                    <input type='text' className='search-field' placeholder='Search area' />
                </div>
            </div>
            <div className='col l3 m6 s12'>
                <div className='search'>
                    <div className='search-field-icon'>
                        <i className='fa fa-layer-group'></i>
                    </div>
                    <select multiple>
                        <option value='0' selected>Categories</option>
                        <optgroup label='team 1'>
                            <option value='1'>Option 1</option>
                            <option value='2'>Option 2</option>
                        </optgroup>
                        <optgroup label='team 2'>
                            <option value='3'>Option 3</option>
                            <option value='4'>Option 4</option>
                        </optgroup>
                    </select>
                </div>
            </div>
        </div>
    );
}

export default SearchBar;