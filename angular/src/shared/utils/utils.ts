
export function expiredJwt(token: string) {
  let base64Url = token.split('.')[1];
  let base64 = base64Url.replace('-', '+').replace('_', '/');
  let jwt = JSON.parse(window.atob(base64));
  let exp = jwt.exp * 1000;
  let currentTime = new Date().getTime() / 1000;
  if (currentTime > jwt.exp) {
    return true;
  } else {
    return false;
  }
}

export function checkOptions(options?: any) {
  if (options !== undefined) {
    if (options.useAuth) {
      return true;
    }
  } else {
    return false;
  }
}
