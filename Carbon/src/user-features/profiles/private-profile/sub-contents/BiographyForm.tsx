import React from 'react';

import CKEditor from '@ckeditor/ckeditor5-react';
import InlineEditor from '@ckeditor/ckeditor5-build-inline';

const BiographyForm = () => {
    return (
        <div className='row'>
            <div className='input-field col m6 s12'>
                <i className='fas fa-file-signature prefix hidro-primary-icon'></i>
                <input id='family-name' type='text' value='' />
                <label htmlFor='family-name'>Family Name</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-file-signature prefix hidro-primary-icon'></i>
                <input id='given-name' type='text' value='' />
                <label htmlFor='given-name'>Given Name</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-venus-mars prefix hidro-primary-icon'></i>
                <select id='gender'>
                    <option value='' disabled selected>Select a gender</option>
                    <option value='1'>Gentlement</option>
                    <option value='2'>Lady</option>
                    <option value='3'>Others</option>
                </select>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-calendar-alt prefix hidro-primary-icon'></i>
                <input id='birthday' type='text' value='' />
                <label htmlFor='birthday'>Date of Birth</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-user prefix hidro-primary-icon'></i>
                <input id='ethnicity' type='text' value='' />
                <label htmlFor='ethnicity'>Ethnicity</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-building prefix hidro-primary-icon'></i>
                <input id='company' type='text' value='' />
                <label htmlFor='company'>Company</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-user-tie prefix hidro-primary-icon'></i>
                <input id='job-title' type='text' value='' />
                <label htmlFor='job-title'>Job Title</label>
            </div>
            <div className='input-field col l4 m12'>
                <i className='fas fa-link prefix hidro-primary-icon'></i>
                <input id='website' type='text' value='' />
                <label htmlFor='website'>Website</label>
            </div>
            <div className='col s12' style={{ marginBottom:'10px' }}>
                <h6>Self Introduction</h6>
                <div className='self-intro'>
                    <CKEditor
                        editor={ InlineEditor }
                        data={ 'Self Introduction' } />
                </div>
            </div>
            <div className='col s12 last-col'>
                <button className='btn grey darken-1 right'>Cancel</button>
                <button className='btn'>Save Changes</button>
            </div>
        </div>
    );
}

export default BiographyForm;