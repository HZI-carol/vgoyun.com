<template>
  <el-container>
    <el-header class="app-header">
      <div style="display: flex; ">
        <div class="title">维构官网后台</div>
        <el-menu class="app-navmenu" mode="horizontal" :default-active="activeIndex" backgroundColor="#409EFF"
                 textColor="rgba(255, 255, 255, 0.5)" activeTextColor="#fff" router>
          <template v-for="route in routes">
            <!--一级菜单-->
            <el-menu-item :index="route.path" :key="route.path" v-if="showTopMenu(route)">
              {{route.name}}
            </el-menu-item>
            <!--一级二级菜单-->
            <el-submenu :index="route.path" v-else-if="!route.hidden" :key="route.path">
              <template slot="title">{{route.name}}</template>
              <!--submenu must use fullpath -->
              <el-menu-item :index="route.path + '/' + subroute.path" v-for="subroute in route.children"
                            :key="subroute.name">
                {{subroute.name}}
              </el-menu-item>
            </el-submenu>
          </template>
        </el-menu>
      </div>
      <div class="account">
        <el-dropdown @command="handleCommand">
          <span class="el-dropdown-link" style="color: #fff;cursor: pointer">
            {{$session.user.nickname}}<i class="el-icon-arrow-down el-icon--right"></i>
          </span>
          <el-dropdown-menu slot="dropdown">
            <el-dropdown-item command="updatepassword" style="width: 60px">修改密码</el-dropdown-item>
            <el-dropdown-item command="logout" style="width: 60px">退出</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </div>
    </el-header>
    <el-main class="app-main">
      <div class="breadcrumb">
        <el-breadcrumb separator-class="el-icon-arrow-right">
          <template v-for="item in matchedRoutes">
            <el-breadcrumb-item :key="item.path" v-if="item.name">
              {{item.name}}
            </el-breadcrumb-item>
          </template>
        </el-breadcrumb>
      </div>
      <router-view/>
    </el-main>
    <el-footer class="app-footer" height="45" v-if="matchedRoutes.length > 0">
      © 2017 - {{new Date().getFullYear()}} vgoyun.com
    </el-footer>

    <!--password dialog-->
    <el-dialog title="修改密码" :visible.sync="showPasswordDialog" width="420px" :close-on-click-modal="false"
               top="25vh">
      <password :dialogBodyVisible="showPasswordDialog" ref="password"></password>
      <div slot="footer" class="dialog-footer">
        <el-button @click="showPasswordDialog = false">取 消</el-button>
        <el-button type="primary" @click="submit">确 定</el-button>
      </div>
    </el-dialog>
  </el-container>
</template>

<script>
  import router from '../../router'
  import Password from '../user/Password'

  export default {
    components: {
      Password
    },
    data () {
      return {
        activeIndex: '',
        showPasswordDialog: false,
        routes: router.options.routes,
        matchedRoutes: []
      }
    },
    created () {
      this.activeIndex = this.$route.matched[0].path
    },
    mounted () {
      this.matchedRoutes = this.$route.matched
      if (!this.$session.logged) {
        this.$router.replace('/')
      }
    },
    watch: {
      $route (to) {
        this.matchedRoutes = this.$route.matched
      }
    },
    methods: {
      showTopMenu (route) {
        if (route.hiddenchildren) {
          return !route.hidden
        } else {
          return !route.hidden && (!route.children || route.children.length === 0)
        }
      },
      handleCommand (command) {
        if (command === 'logout') {
          this.$confirm(`确定要退出登录吗?`, '退出提示', {
            dangerouslyUseHTMLString: true,
            customClass: 'mini-size-messagebox',
            type: 'warning'
          }).then(() => {
            this.$session.clear()
            this.$router.replace('/')
          })
        } else if (command === 'updatepassword') {
          this.showPasswordDialog = true
        }
      },
      submit () {
        this.$refs.password.submit(() => {
          this.showPasswordDialog = false
        })
      }
    }
  }
</script>
