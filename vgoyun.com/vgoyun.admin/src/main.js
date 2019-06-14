// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import dateformat from 'dateformat'
import Service from './services'
import Session from './services/session'
import Shared from './components/shared'
// 导入quill富文本编辑器
import './components/quill/index'

Vue.config.productionTip = false

/* register httpservice plugin */
let baseURL = 'http://localhost:59359/'
if (process && process.env.NODE_ENV === 'production') {
  baseURL = '/'
}

Vue.use(ElementUI)
Vue.use(Service, { baseURL: baseURL })
Vue.use(Session)
Vue.use(Shared)

Vue.filter('dateFilter', function (value) {
  if (!value) {
    return ''
  }
  return dateformat(value, 'yyyy-mm-dd')
})

Vue.filter('datetimeFilter', function (value) {
  if (!value) {
    return ''
  }
  return dateformat(value, 'yyyy-mm-dd HH:MM:ss')
})

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: { App },
  template: '<App/>',
  created () {
    console.info('build-20190605')
  }
})
