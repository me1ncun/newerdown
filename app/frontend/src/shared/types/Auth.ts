export type User = {
  name: string;
  email: string;
};

export type AuthResponse = {
  accessToken: string;
  user?: User;
};

export type SingUp = {
  userName: string;
  email: string;
  password: string;
};

export type Login = {
  email: string;
  password: string;
};

export type ChangePassword = {
  currentPassword: string;
  newPassword: string;
};
