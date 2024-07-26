export interface ContactCountryState{
    contactId: number,
    countryId: number,
    stateId: number,
    firstName: string,
    lastName: string,
    contactNumber: number,
    email: string,
    contactDescription: string,
    profilePic: string,
    gender: string,
    address: string,
    favourite: boolean,
    imageByte:string,
    state: {
      stateId: number,
      stateName: string,
      countryId: number
    },
    country: {
      countryId: number,
      countryName: string
    }
  }