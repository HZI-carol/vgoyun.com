import Service from './service'

export default class CaseService extends Service {
  add (model) {
    return this.httpPost('api/admin/cases', model)
  }

  update (model) {
    return this.httpPut(`api/admin/cases/${model.id}`, model)
  }

  getList (page, pageSize, title, type, orderBy) {
    type = type === '' ? 0 : type
    return this.httpGet(`api/admin/cases/${page}/${pageSize}`, { title, type, orderBy })
  }

  remove (id) {
    return this.httpDelete(`api/admin/cases/${id}`)
  }
}
