import React from 'react';

const BiographyShow = () => {
    return (
        <div className='row'>
            <div className='col l3 m6 s12'>
                <p className='bioshow'><b>Full Name:</b> Le Kim Phuc Nguyen</p>
            </div>
            <div className='col l3 m6 s12'>
                <p className='bioshow'><b>Birth:</b> 01 Aug 1989</p>
            </div>
            <div className='col l3 m6 s6'>
                <p className='bioshow'><b>Sex:</b> Male</p>
            </div>
            <div className='col l3 m6 s6'>
                <p className='bioshow'><b>Ethnicity:</b> Kinh</p>
            </div>
            <div className='col m6 s12'>
                <p className='bioshow'><b>Company:</b> Example Pty. Ltd.</p>
            </div>
            <div className='col m6 s12'>
                <p className='bioshow'><b>Job Title:</b> Software Developer</p>
            </div>
            <div className='col m6 s12'>
                <p className='bioshow'><b>Website:</b> <a href=''>www.example.com</a></p>
            </div>
            <div className='col s12'>
                <p><b>Self Introduction</b></p>
                <p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p>
            </div>
            <div className='col s12'>
                <button className='btn'>Update</button>
            </div>
        </div>
    );
}

export default BiographyShow;