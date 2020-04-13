import { CONSTANTS } from "../../../../helpers/helper";

export const GET_PRIVATE_PROFILE_BEGIN = 'GET_PRIVATE_PROFILE_BEGIN';
export type T_GET_PRIVATE_PROFILE_BEGIN = typeof GET_PRIVATE_PROFILE_BEGIN;

export const GET_PRIVATE_PROFILE_FAILED = 'GET_PRIVATE_PROFILE_FAILED';
export type T_GET_PRIVATE_PROFILE_FAILED = typeof GET_PRIVATE_PROFILE_FAILED;

export const GET_PRIVATE_PROFILE_SUCCESS = 'GET_PRIVATE_PROFILE_SUCCESS';
export type T_GET_PRIVATE_PROFILE_SUCCESS = typeof GET_PRIVATE_PROFILE_SUCCESS;

export const UPDATE_PRIVATE_PROFILE_BEGIN = 'UPDATE_PRIVATE_PROFILE_BEGIN';
export type T_UPDATE_PRIVATE_PROFILE_BEGIN = typeof UPDATE_PRIVATE_PROFILE_BEGIN;

export const UPDATE_PRIVATE_PROFILE_SUCCESS = 'UPDATE_PRIVATE_PROFILE_SUCCESS';
export type T_UPDATE_PRIVATE_PROFILE_SUCCESS = typeof UPDATE_PRIVATE_PROFILE_SUCCESS;

export const UPDATE_PRIVATE_PROFILE_FAILED = 'UPDATE_PRIVATE_PROFILE_FAILED';
export type T_UPDATE_PRIVATE_PROFILE_FAILED = typeof UPDATE_PRIVATE_PROFILE_FAILED;

export const SET_NEW_PROFILE_AFTER_UPDATE = 'SET_NEW_PROFILE_AFTER_UPDATE';
export type T_SET_NEW_PROFILE_AFTER_UPDATE = typeof SET_NEW_PROFILE_AFTER_UPDATE;

export interface IBioShow {
    showBioForm: any,
    user : any,
    profileResult : any
}

export interface IBioForm {
    showBioForm : any,
    user : any,
    profileResult : any,
    updateResult : any,
    updatePrivateProfile : any,
    bioShowShouldUpdate : any
}

interface IBirth {
    friendlyBirth : string,
    birth : string | null
}

export interface IProfile {
    id : number,
    hidrogenianId : number,
    familyName : string,
    givenName : string,
    fullName : string,
    birthday : IBirth,
    gender : number,
    ethnicity : string,
    company : string,
    jobTitle : string,
    website : string,
    selfIntroduction : string
};

export const VOID_PROFILE : IProfile = {
    id : 0,
    hidrogenianId : 0,
    familyName : CONSTANTS.EMPTY,
    givenName : CONSTANTS.EMPTY,
    fullName : CONSTANTS.EMPTY,
    birthday : {
        friendlyBirth : CONSTANTS.EMPTY,
        birth : null
    } as IBirth,
    gender : 0,
    ethnicity : CONSTANTS.EMPTY,
    company : CONSTANTS.EMPTY,
    jobTitle : CONSTANTS.EMPTY,
    website : CONSTANTS.EMPTY,
    selfIntroduction : CONSTANTS.EMPTY
}