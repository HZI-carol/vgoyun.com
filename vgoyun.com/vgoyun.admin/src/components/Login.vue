<template>
  <div class="login-body">
    <el-card class="login">
      <div class="title">系统后台登录</div>
      <el-form :model="user" label-width="75px" style="margin-top: 2em" :rules="rules" ref="form">
        <el-form-item label="用户名" prop="username">
          <el-input v-model.trim="user.username" :maxlength="30" placeholder="请输入用户名"></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input v-model.trim="user.password" type="password" :maxlength="18" placeholder="请输入密码"></el-input>
        </el-form-item>
        <el-form-item label="验证码" prop="vcode">
          <div style="display: flex">
            <el-input v-model.trim="user.vcode" style="width: 145px" ref="input" :maxlength="5" placeholder="请输入验证码"
                      @click.native.enter="login"/>
            <img :src="vcodeUrl" @click="changeCode" class="vcode"/>
          </div>
        </el-form-item>
        <el-form-item>
          <el-button class="submit" size="medium" type="primary" style="width: 100%;" @click="login">登录</el-button>
        </el-form-item>
      </el-form>
    </el-card>
    <div class="footer">© 2017 - {{new Date().getFullYear()}} vgoyun.com</div>
  </div>
</template>

<script>
  export default {
    data () {
      return {
        user: {
          username: '',
          password: '',
          vcode: ''
        },
        vcodeUrl: '',
        rules: {
          username: [
            { required: true, message: '请输入用户名', trigger: 'blur' }
          ],
          password: [
            { required: true, message: '请输入密码', trigger: 'blur' }
          ],
          vcode: [
            { required: true, message: '请输入验证码', trigger: 'blur' }
          ]
        }
      }
    },
    mounted () {
      if (this.$session.token) {
        this.$router.push('index')
      }
      this.changeCode()
    },
    methods: {
      changeCode () {
        this.vcodeUrl = `${this.$http.defaults.baseURL}api/admin/vcode?sid=${this.$session.sessionId}&_=${new Date().getTime()}`
        this.user.vcode = ''
      },
      login () {
        this.$refs.form.validate(valid => {
          if (valid) {
            this.user.sid = this.$session.sessionId
            this.$service.user.login(this.user).then(data => {
              this.$session.set(data)
              this.$router.push('index')
            }).catch(res => {
              this.changeCode()
            })
          }
        })
      }
    }
  }
</script>

<style lang="scss" scoped>
  .login {
    margin: 0 auto;
    width: 360px;
    padding: 20px;
    margin-top: calc((100vh - 440px) / 2);

    .title {
      font-size: 1.5em;
      text-align: center;
      color: #2196f3;
    }

    .vcode {
      position: relative;
      top: 5px;
      height: 30px;
      cursor: pointer;
      margin-left: 1em;
    }

    .submit {
      margin-top: 1em;
    }
  }

  .footer {
    position: fixed;
    width: 100%;
    line-height: 3em;
    font-size: 13px;
    bottom: 0;
    text-align: center;
  }
</style>
