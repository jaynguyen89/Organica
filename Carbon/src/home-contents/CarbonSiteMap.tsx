import React from 'react';
import './style.css';

const CarbonSiteMap = () => {
    return (
        <>
            <div className='sitemap-wrapper'>
                <div className='row'>
                    <div className='col l3 m6 s6'>
                        <h6>Hidrogen Marketplace</h6>
                        <p className='address'>
                            Melbourne, VIC 3000, Australia.<br />
                            Developed by <a href='/'>Jay Nguyen</a> with <i className='fas fa-heart pink-text'></i>.
                            <br /> <br />
                            Follow us on:
                        </p>
                        <div className='row'>
                            <div className='col m2 s3'><a href='/'><i className='fab fa-facebook fa-2x'></i></a></div>
                            <div className='col m2 s3'><a href='/'><i className='fab fa-youtube fa-2x'></i></a></div>
                            <div className='col m2 s3'><a href='/'><i className='fab fa-twitter fa-2x'></i></a></div>
                        </div>
                    </div>
                    <div className='col l3 m6 s6'>
                        <h6>Support</h6>
                        <div className='row'>
                            <div className='col s12'>
                                <ul className='tree-layout'>
                                    <li><a href='/'>About Hidrogen</a></li>
                                    <li><a href='/'>Seller Protection</a></li>
                                    <li><a href='/'>Buyer Protection</a></li>
                                    <li><a href='/'>Buy & Sell Tips</a></li>
                                    <li><a href='/'>For Business & Ads</a></li>
                                    <li><a href='/'>Feedbacks & Ideas</a></li>
                                    <li><a href='/'>Contact Support</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div className='col l3 m6 s6'>
                        <h6>Legal</h6>
                        <div className='row'>
                            <div className='col s12'>
                                <ul className='tree-layout'>
                                    <li><a href='/'>Terms & Conditions</a></li>
                                    <li><a href='/'>Privacy Policy</a></li>
                                    <li><a href='/'>Buy & Sell Policy</a></li>
                                    <li><a href='/'>Cookie Policy</a></li>
                                    <li><a href='/'>Promotion Policy</a></li>
                                    <li><a href='/'>Disclaimer</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div className='col l3 m6 s6'>
                        <h6>On the go</h6>
                        <p className='address'>Sell and Shop from your mobile phone.<br />Download the app:</p>
                        <div className='row'>
                            <div className='col s4'>
                                <a href='/'><img src='assets/appstore.png' className='responsive-img' /></a>
                            </div>
                            <div className='col s4'>
                                <a href='/'><img src='assets/playstore.png' className='responsive-img' /></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default CarbonSiteMap;