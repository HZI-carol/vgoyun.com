let configured = false

export default class Service {
  constructor ($vue) {
    this.$vue = $vue
    this.$http = $vue.$http
    this.$session = $vue.$session
    this.$router = $vue.$router
    if (!configured) {
      this.setup($vue)
      configured = true
    }
  }

  /**
   * setup axios defaults config
   */
  setup ($vue) {
    // Add a request interceptor
    this.$http.interceptors.request.use(config => {
      this.loading = $vue.$loading({
        lock: true,
        background: 'rgba(0, 0, 0, 0.7)'
      })
      // 判断当前接口是否需要授权码访问
      if (config.requireAuth && $vue.$session.token) {
        if (config.url.lastIndexOf('?') < 0) {
          config.url += '?token=' + $vue.$session.token
        } else {
          config.url += '&token=' + $vue.$session.token
        }
      }
      return config
    }, function (error) {
      this.loading.close()
      return Promise.reject(error)
    })
    // Add a response interceptor
    this.$http.interceptors.response.use((response) => {
      this.loading.close()
      return response.data
    }, (error) => {
      console.info(error)
      this.loading.close()
      let response = error.response
      let message = ''
      // if 401 go login
      if (response && response.status === 401) {
        $vue.$session.clear()
        location.href = '/'
      } else {
        message = error.message || '请求出错，请重试～'
        $vue.$message.error(message)
      }
      return Promise.reject(error)
    })
  }

  /**
   * 返回结果处理
   * @param res
   * @param resolve
   * @private
   */
  _handleResult (res, resolve, reject) {
    if (res.code === 200 || res.code === '200') {
      resolve(res.data)
    } else {
      this.$vue.$message.error(res.msg || '请求出错，请重试～')
      reject(res)
    }
  }

  /**
   * get方式请求接口
   * @param url
   * @param data
   * @param requireAuth (options)
   * @returns {Promise}
   */
  httpGet (url, data, requireAuth = true) {
    return new Promise((resolve, reject) => {
      this.$http.get(url, { params: data, requireAuth: requireAuth }).then(res => {
        this._handleResult(res, resolve, reject)
      }).catch(err => reject(err))
    })
  }

  /**
   * 当data为 object 时候为 'application/json' 方式
   * 当data为 formdata 时候为 'multipart/form-data' 方式
   * @param url
   * @param data
   * @param requireAuth (options)
   * @returns {Promise}
   */
  httpPost (url, data, requireAuth = true) {
    return new Promise((resolve, reject) => {
      this.$http.post(url, data, { requireAuth: requireAuth }).then(res => {
        this._handleResult(res, resolve, reject)
      }).catch(err => reject(err))
    })
  }

  /**
   * put 方式请求接口
   * @param url
   * @param data
   * @param requireAuth
   * @returns {Promise}
   */
  httpPut (url, data, requireAuth = true) {
    return new Promise((resolve, reject) => {
      this.$http.put(url, data, { requireAuth: requireAuth }).then(res => {
        this._handleResult(res, resolve, reject)
      }).catch(err => reject(err))
    })
  }

  /**
   * delete 方式请求接口
   * @param url
   * @param requireAuth
   * @returns {Promise}
   */
  httpDelete (url, requireAuth = true) {
    return new Promise((resolve, reject) => {
      this.$http.delete(url, { requireAuth: requireAuth }).then(res => {
        this._handleResult(res, resolve, reject)
      }).catch(err => reject(err))
    })
  }
}
