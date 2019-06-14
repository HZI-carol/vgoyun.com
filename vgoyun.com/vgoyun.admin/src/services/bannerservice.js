import Service from './service'

export default class BannerService extends Service {
  add (model) {
    return this.httpPost('api/admin/banners', model)
  }

  update (model) {
    return this.httpPut(`api/admin/banners/${model.id}`, model)
  }

  getList (page, pageSize, title, type, orderBy) {
    type = type === '' ? 0 : type
    return this.httpGet(`api/admin/banners/${page}/${pageSize}`, { title, type, orderBy })
  }

  remove (id) {
    return this.httpDelete(`api/admin/banners/${id}`)
  }
}
