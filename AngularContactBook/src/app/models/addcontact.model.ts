export interface AddContact{
    countryId: number,
    stateId: number|null,
    firstName: string,
    lastName: string,
    contactNumber: string,
    email: string,
    contactDescription: string,
    profilePic: string,
    gender: string,
    address: string,
    favourite: boolean,
    imageByte:string,
  }