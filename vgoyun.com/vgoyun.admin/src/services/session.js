import uuid from 'uuid'
import VueCookie from 'vue-cookie'

const SessionKeys = {
  token: '__admin_sess',
  user: '__admin_user'
}

class Session {
  constructor () {
    this.$cookie = VueCookie
    this.sessionId = ''
    this.logged = false
    this.token = ''
    this.user = null
    this.init()
  }

  init () {
    this.sessionId = uuid().replace(/-/g, '')
    this.token = this.$cookie.get(SessionKeys.token)
    let json = this.$cookie.get(SessionKeys.user)
    if (json) {
      this.user = JSON.parse(decodeURIComponent(json))
    }
    if (this.token && this.user) {
      this.logged = true
      this.user = Object.assign(this.user, {
        hasPermission: (pid) => {
          return this.user.permissions.includes(pid)
        }
      })
    } else {
      this.logged = false
    }
  }

  set (data) {
    this.token = data.access_token
    this.user = {
      username: data.username,
      nickname: data.nickname,
      expires: data.expires
    }
    // 保存登录信息到cookie
    let expires = new Date(data.expires * 1000)
    this.$cookie.set(SessionKeys.token, this.token, { expires: expires })
    this.$cookie.set(SessionKeys.user, JSON.stringify(this.user), { expires: expires })
    this.user = Object.assign(this.user, {
      hasPermission: (pid) => {
        return this.user.permissions.includes(pid)
      }
    })
    this.logged = true
  }

  clear () {
    this.token = ''
    this.user = null
    this.logged = false
    this.$cookie.delete(SessionKeys.token)
    this.$cookie.delete(SessionKeys.user)
  }
}

const session = new Session()

export default {
  install (Vue, options) {
    Vue.mixin({
      created () {
        this.$session = session
      }
    })
  }
}
