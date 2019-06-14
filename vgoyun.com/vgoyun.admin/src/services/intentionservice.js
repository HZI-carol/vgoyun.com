import Service from './service'

export default class IntentionService extends Service {
  getList (page, pageSize, keyword, intention, orderBy) {
    return this.httpGet(`api/admin/intentions/${page}/${pageSize}`, { keyword, intention, orderBy })
  }
}
