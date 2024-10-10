export type Token = {
  nameid?: string,
  name: string,
  email: string,
  permissions: string[],
  roles: string | string[],
  exp: number,
  iat: number,
  nbf: number
}
