import Vue from 'vue'
import Router from 'vue-router'
import Layout from '@/components/shared/Layout'
import NotFound from '@/components/shared/404'
import Login from '@/components/Login'
import Case from '@/components/case/List'
import Banner from '@/components/banner/List'
import Article from '@/components/article/List'
import ArticleEdit from '@/components/article/Edit'
import Comment from '@/components/comment/List'
import Intention from '@/components/intention/List'

Vue.use(Router)

/**
 * hidden: 表示隐藏一级路由（用于顶部菜单）
 * hiddenchildren : 表示隐藏子路由（用于顶部菜单）
 * @type {VueRouter}
 */
const router = new Router({
  routes: [
    {
      path: '/',
      name: 'Login',
      component: Login,
      hidden: true
    },
    {
      path: '/index',
      name: 'Index',
      redirect: '/case/index',
      hidden: true
    },
    {
      path: '/case',
      name: '案例管理',
      redirect: '/case/index',
      component: Layout,
      hiddenchildren: true,
      children: [
        {
          path: 'index',
          name: '案例列表',
          component: Case,
          meta: { requireAuth: true }
        }
      ]
    },
    {
      path: '/article',
      name: '资讯管理',
      redirect: '/article/index',
      component: Layout,
      hiddenchildren: true,
      children: [
        {
          path: 'index',
          name: '资讯列表',
          component: Article,
          meta: { requireAuth: true }
        },
        {
          path: 'edit',
          name: '资讯编辑',
          component: ArticleEdit,
          meta: { requireAuth: true }
        }
      ]
    },
    {
      path: '/banner',
      name: 'Banner管理',
      redirect: '/banner/index',
      component: Layout,
      hiddenchildren: true,
      children: [
        {
          path: 'index',
          name: 'Banner列表',
          component: Banner,
          meta: { requireAuth: true }
        }
      ]
    },
    {
      path: '/comment',
      name: '留言管理',
      redirect: '/comment/index',
      component: Layout,
      hiddenchildren: true,
      children: [
        {
          path: 'index',
          name: '留言列表',
          component: Comment,
          meta: { requireAuth: true }
        }
      ]
    },
    {
      path: '/intention',
      name: '意向管理',
      redirect: '/intention/index',
      component: Layout,
      hiddenchildren: true,
      children: [
        {
          path: 'index',
          name: '意向列表',
          component: Intention,
          meta: { requireAuth: true }
        }
      ]
    },
    {
      path: '*',
      name: 'NotFound',
      component: NotFound,
      hidden: true
    }
  ]
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(r => r.meta.requireAuth)) {
    // check if login
    let session = router.app.$session
    if (session && !session.logged) {
      next({
        path: '/'
      })
    } else {
      next()
    }
  } else {
    next()
  }
})

export default router
