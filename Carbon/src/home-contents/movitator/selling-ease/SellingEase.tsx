import React from 'react';
import './style.css';

const SellingEase = () => {
    return (
        <>
            <div id='ease-intro' className='row'>
                <p>Start selling and earn on Hidrogen.</p>
                <hr />
                <span>Find out how to sell</span>
            </div>
            <div className='md-stepper-horizontal'>
                <div className='md-step active'>
                    <div className='md-step-circle'><i className='fas fa-user fa-2x'></i></div>
                    <div className='md-step-title'>1. Create Account</div>
                    <div className='md-step-optional'>Optional</div>
                    <div className='md-step-bar-left'></div>
                    <div className='md-step-bar-right'></div>
                </div>
                <div className='md-step active'>
                    <div className='md-step-circle'><i className='fas fa-tape fa-2x'></i></div>
                    <div className='md-step-title'>2. Prepare Items</div>
                    <div className='md-step-optional'>Optional</div>
                    <div className='md-step-bar-left'></div>
                    <div className='md-step-bar-right'></div>
                </div>
                <div className='md-step active'>
                    <div className='md-step-circle'><i className='fas fa-paper-plane fa-2x'></i></div>
                    <div className='md-step-title'>3. Create Listings</div>
                    <div className='md-step-optional'>Optional</div>
                    <div className='md-step-bar-left'></div>
                    <div className='md-step-bar-right'></div>
                </div>
                <div className='md-step active'>
                    <div className='md-step-circle'><i className='fas fa-people-carry fa-2x'></i></div>
                    <div className='md-step-title'>4. Ship & Done!</div>
                    <div className='md-step-optional'>Optional</div>
                    <div className='md-step-bar-left'></div>
                    <div className='md-step-bar-right'></div>
                </div>
            </div>
            <div className='row'>
                <div className='ease-wrapper'>
                    <div className='col l4 m6 s12'>
                        <h4>Anyone can be seller!</h4>
                        <div className='row'>
                            <div className='col s12'>
                                <p><i className='fas fa-award'></i>&nbsp;We make it super easy for you to sell^</p>
                                <p><i className='fas fa-award'></i>&nbsp;Enjoy your business more than ever.</p>
                            </div>
                        </div>
                        <button className='btn'>Start Selling Now</button>
                    </div>
                    <div className='col l8 m6 s12 ease-photo'>

                    </div>
                </div>
            </div>
        </>
    );
}

export default SellingEase;