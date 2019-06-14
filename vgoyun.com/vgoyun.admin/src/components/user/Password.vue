<template>
  <div class="dialog-body">
    <el-form :model="model" label-width="100px" class="edit-form" :rules="rules" ref="form"
             v-if="dialogBodyVisible">
      <el-form-item label="旧密码" prop="oldpassword">
        <el-input v-model.trim="model.password" type="password" :maxlength="18" placeholder="请输入旧密码"></el-input>
      </el-form-item>
      <el-form-item label="新密码" prop="newpassword">
        <el-input v-model.trim="model.newpassword" type="password" :maxlength="18" placeholder="请输入新密码"></el-input>
      </el-form-item>
      <el-form-item label="确认新密码" prop="renewpassword">
        <el-input v-model.trim="model.renewpassword" type="password" :maxlength="18" placeholder="请再次输入新密码"></el-input>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
  export default {
    props: {
      dialogBodyVisible: {
        type: Boolean,
        required: true,
        default: false
      }
    },
    data () {
      return {
        dialogVisible: false,
        model: {
          oldpassword: '',
          newpassword: ''
        },
        rules: {
          password: [
            { required: true, message: '请输入旧密码', trigger: 'blur' }
          ],
          newpassword: [
            { required: true, message: '请输入新密码', trigger: 'blur' }
          ],
          renewpassword: [
            { required: true, message: '请再次输入新密码', trigger: 'blur' }
          ]
        }
      }
    },
    watch: {
      dialogBodyVisible () {
        this.model = {
          oldpassword: '',
          newpassword: '',
          renewpassword: ''
        }
      }
    },
    methods: {
      submit (callback) {
        if (this.model.newpassword !== this.model.renewpassword) {
          this.$message.error('两次密码输入不一致')
          return
        }
        this.$refs.form.validate(valid => {
          if (valid) {
            this.$service.user.updatePassword(this.model).then(() => {
              this.$message.success('修改成功')
              callback.call()
            })
          }
        })
      }
    }
  }
</script>
