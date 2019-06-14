import Service from './service'

export default class ArticleService extends Service {
  add (model) {
    return this.httpPost('api/admin/articles', model)
  }

  update (model) {
    return this.httpPut(`api/admin/articles/${model.id}`, model)
  }

  get (id) {
    return this.httpGet(`api/admin/articles/${id}`)
  }

  getList (page, pageSize, title, type, ishot, isnew, isshow, orderBy) {
    type = type === '' ? 0 : type
    ishot = ishot === '' ? -1 : ishot
    isnew = isnew === '' ? -1 : isnew
    isshow = isshow === '' ? -1 : isshow
    return this.httpGet(`api/admin/articles/${page}/${pageSize}`, { title, type, ishot, isnew, isshow, orderBy })
  }

  remove (id) {
    return this.httpDelete(`api/admin/articles/${id}`)
  }
}
