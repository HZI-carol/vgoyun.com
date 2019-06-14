import Service from './service'

export default class CommonService extends Service {
  getTypeList (parentid) {
    return this.httpGet('api/admin/types/' + parentid)
  }

  uploadImage (formData) {
    return this.httpPost('api/admin/image/upload', formData)
  }

  getCommentList (page, pageSize, created, orderBy) {
    return this.httpGet(`api/admin/comments/${page}/${pageSize}`, { created, orderBy })
  }
}
