export type UserInformation = {
  userName?: string | null;
  email?: string | null;
  organizationName?: string | null;
  timeZone?: string | null;
  language?: string | null;
  displayName?: string | null;
  phoneNumber?: string | null;
};

export type UserResponse = {
  token: string;
  user?: UserInformation;
};
