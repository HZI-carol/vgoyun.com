import Service from './service'

export default class UserService extends Service {
  login (user) {
    return this.httpPost('api/admin/login', user, false)
  }

  updatePassword (model) {
    return this.httpPut('api/admin/user/password', model)
  }

  getTypeList (parentid) {
    return this.httpGet('api/admin/types/' + parentid)
  }
}
