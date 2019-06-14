import axios from 'axios'
import UserService from './userservice'
import CaseService from './caseservice'
import ArticleService from './articleservice'
import CommonService from './commonservice'
import IntentionService from './intentionservice'
import BannerService from './bannerservice'

let $service

export default {
  install (Vue, options) {
    axios.defaults.baseURL = options.baseURL || ''
    Vue.mixin({
      created () {
        if (!this.$http) {
          this.$http = axios
        }
        if (!$service) {
          const user = new UserService(this)
          const cases = new CaseService(this)
          const banner = new BannerService(this)
          const article = new ArticleService(this)
          const common = new CommonService(this)
          const intention = new IntentionService(this)
          $service = { user, cases, banner, article, common, intention }
        }
        this.$service = $service
      }
    })
  }
}
