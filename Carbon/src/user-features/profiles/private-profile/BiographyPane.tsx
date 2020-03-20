import React from 'react';
import { connect } from 'react-redux';

import CarbonAtavar from '../../../shared/CarbonAtavar';
import BiographyForm from './sub-contents/BiographyForm';
import BiographyShow from './sub-contents/BiographyShow';

const mapStateToProps = (state: any) => ({

});

const mapDispatchToProps = {

};

const BiographyPane = () => {
    return (
        <div className='row'>
            <h6 className='content-caption'>
                <i className='fas fa-user-circle hidro-primary-icon'></i>&nbsp;&nbsp;Biography
            </h6>
            <div className='col m3 s12' style={{ textAlign:'center' }}>
                <CarbonAtavar /><br />
                <a className='link-text'>Change Avatar</a>
            </div>
            <div className='col m9 s12'>
                <BiographyShow />
                <BiographyForm />
            </div>
        </div>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(BiographyPane);