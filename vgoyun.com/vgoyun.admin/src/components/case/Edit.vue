<template>
  <el-form :model="model" label-width="120px" class="edit-form" :rules="rules" ref="form"
           v-if="dialogBodyVisible">
    <el-form-item label="标题" prop="title" required>
      <el-input v-model.trim="model.title" style="width: 75%" :maxlength="100" placeholder="请输入标题"></el-input>
    </el-form-item>
    <el-form-item label="图片" required>
      <el-upload :on-change="handleChange" action="" list-type="picture-card" :auto-upload="false" style="width: 75%"
                 :limit="1" :on-preview="handlePictureCardPreview" :on-remove="handleRemove" :file-list="fileList">
        <i class="el-icon-plus"></i>
        <div slot="tip" class="el-upload__tip">只能上传jpg、jpeg、png文件，且不超过1M</div>
      </el-upload>
    </el-form-item>
    <el-form-item label="排序值" prop="sort">
      <el-input-number v-model.trim="model.sort" style="width: 180px" :min="0" placeholder="请输入排序值"></el-input-number>
    </el-form-item>
    <el-form-item label="类型" prop="typeid">
      <el-select v-model="model.typeid" placeholder="请选择">
        <el-option label="请选择类型" :value="0"></el-option>
        <el-option v-for="item in types" :key="item.typeid" :label="item.text" :value="item.typeid"/>
      </el-select>
    </el-form-item>
    <el-form-item label="查看数" prop="seecount">
      <el-input-number v-model.trim="model.seecount" style="width: 180px" :min="0" placeholder="请输入查看数"></el-input-number>
    </el-form-item>
    <el-form-item label="点赞数" prop="prizecount">
      <el-input-number v-model.trim="model.prizecount" style="width: 180px" :min="0" placeholder="请输入点赞数"></el-input-number>
    </el-form-item>
    <el-form-item label="跳转地址" prop="link">
      <el-input v-model.trim="model.link" style="width: 75%" :maxlength="300" placeholder="请输入跳转地址"></el-input>
    </el-form-item>
  </el-form>
</template>

<script>
  import { AllowUploadPicExts } from '../../utils/consts'
  import Transform from '../../utils/transform'

  export default {
    props: {
      model: {
        type: Object,
        required: true,
        default: () => {}
      },
      dialogBodyVisible: {
        type: Boolean,
        required: true,
        default: false
      }
    },
    data () {
      return {
        dialogVisible: false,
        fileList: [],
        types: [],
        allowExts: AllowUploadPicExts,
        rules: {
          title: [
            { required: true, message: '请输入标题', trigger: 'blur' }
          ],
          typeid: [
            {
              required: true,
              message: '请选择类型',
              trigger: 'blur',
              transform: Transform.toInteger,
              validator (rule, value, callback) {
                if (value > 0) {
                  callback()
                } else {
                  callback(new Error('请选择类型'))
                }
              }
            }
          ],
          link: [
            { required: false, type: 'url', message: '跳转地址格式不正确', trigger: 'blur' }
          ]
        }
      }
    },
    mounted () {
      this.init()
    },
    watch: {
      dialogBodyVisible (val) {
        if (val) {
          this.init()
        }
      }
    },
    methods: {
      init () {
        this.$service.common.getTypeList(2).then(d => {
          this.types = d
        })
        if (this.model.id > 0) {
          this.fileList = [{ url: this.model.fullimgurl }]
        } else {
          this.fileList = []
        }
      },
      handleChange (file, fileList) {
        if (!this.allowExts.some(i => file.raw.name.lastIndexOf(i) > -1)) {
          this.$message.error('上传的图片格式不正确')
          this.fileList = []
        } else if (file.raw.size > 1024 * 1024) {
          this.$message.error('上传的图片大小不能超过1M')
          this.fileList = []
        } else {
          this.fileList = fileList
          const formData = new FormData()
          formData.append('file', this.fileList[0].raw)
          this.$service.common.uploadImage(formData).then(d => {
            this.model.imgurl = d.url
          }).catch(e => {
            this.fileList = []
            fileList = []
          })
        }
      },
      handleRemove (file, fileList) {
        this.fileList = []
      },
      handlePictureCardPreview (file) {
        this.$previewImage(file.url)
      },
      submit (callback) {
        this.$refs.form.validate(valid => {
          if (valid) {
            if (!this.model.imgurl) {
              this.$message.error('请选择上传的图片')
              return
            }
            if (!this.model.id) {
              this.$service.cases.add(this.model).then(() => {
                this.$message.success('添加成功')
                callback.call()
              })
            } else {
              this.$service.cases.update(this.model).then(() => {
                this.$message.success('更新成功')
                callback.call()
              })
            }
          }
        })
      }
    }
  }
</script>
