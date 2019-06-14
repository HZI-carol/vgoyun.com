<template>
  <div class="page">
    <el-form :model="model" label-width="150px" class="edit-form" :rules="rules" ref="form">
      <el-form-item label="标题" prop="title" required>
        <el-input v-model.trim="model.title" style="width: 75%" :maxlength="100" placeholder="请输入标题"></el-input>
      </el-form-item>
      <el-form-item label="发布日期" prop="created">
        <el-date-picker v-model="model.created" type="date" placeholder="请选择发布日期"/>
      </el-form-item>
      <el-form-item label="图片" required>
        <el-upload :on-change="handleChange" action="" list-type="picture-card" :auto-upload="false" style="width: 75%"
                   :limit="1" :on-preview="handlePictureCardPreview" :on-remove="handleRemove" :file-list="fileList">
          <i class="el-icon-plus"></i>
          <div slot="tip" class="el-upload__tip" style="line-height: 21px">
            <div style="color: #f56c6c">注意：建议上传的图片为：390 * 230 等比例的图片</div>
            <div>只能上传jpg、jpeg、png文件，且不超过1M</div>
          </div>
        </el-upload>
      </el-form-item>
      <el-form-item label="作者" prop="author">
        <el-input v-model.trim="model.author" style="width: 75%" :maxlength="100" placeholder="请输入作者"></el-input>
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
        <el-input-number v-model="model.seecount" style="width: 180px" :min="0" placeholder="请输入查看数"></el-input-number>
      </el-form-item>
      <el-form-item label="是否热点">
        <el-radio-group v-model="model.ishot">
          <el-radio-button :label="true">是</el-radio-button>
          <el-radio-button :label="false">否</el-radio-button>
        </el-radio-group>
      </el-form-item>
      <el-form-item label="是否最新">
        <el-radio-group v-model="model.isnew">
          <el-radio-button :label="true">是</el-radio-button>
          <el-radio-button :label="false">否</el-radio-button>
        </el-radio-group>
      </el-form-item>
      <el-form-item label="是否推荐">
        <el-radio-group v-model="model.isshow">
          <el-radio-button :label="true">是</el-radio-button>
          <el-radio-button :label="false">否</el-radio-button>
        </el-radio-group>
      </el-form-item>
      <el-form-item label="简介" prop="samllcontents">
        <el-input type="textarea" v-model.trim="model.samllcontents" :maxlength="1000" placeholder="请输入简介"
                  :rows="5" style="width: 75%"></el-input>
      </el-form-item>
      <el-form-item label="内容" prop="contents" style="height: 600px; overflow-y: auto">
        <editor style="width: 1150px; height: 550px" ref="editor" @ready="editorReady"></editor>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submit">提交</el-button>
        <el-button @click="$router.replace('/article/index')">取消</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
  import { AllowUploadPicExts } from '../../utils/consts'
  import Transform from '../../utils/transform'
  import Editor from '../quill/Editor'

  export default {
    components: {
      Editor
    },
    data () {
      return {
        model: {},
        fileList: [],
        types: [],
        allowExts: AllowUploadPicExts,
        rules: {
          title: [
            { required: true, message: '请输入标题', trigger: 'blur' }
          ],
          author: [
            { required: true, message: '请输入作者', trigger: 'blur' }
          ],
          samllcontents: [
            { required: true, message: '请输入简介', trigger: 'blur' }
          ],
          created: [
            {
              required: true,
              type: 'date',
              message: '请选择发布日期',
              trigger: 'blur',
              transform: Transform.toDate
            }
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
          ]
        }
      }
    },
    created () {
      this.$service.common.getTypeList(1).then(d => {
        this.types = d
      })
      this.model = {
        id: this.$route.query.id || 0,
        title: '',
        author: '',
        imgurl: '',
        samllcontents: '',
        contents: '',
        typeid: 0,
        sort: 0,
        seecount: 0,
        ishot: false,
        isnew: false,
        isshow: false,
        created: new Date()
      }
    },
    methods: {
      editorReady () {
        if (this.model.id > 0) {
          this.$service.article.get(this.model.id).then(data => {
            this.model = Object.assign(this.model, data)
            this.fileList = [{ url: this.model.fullimgurl }]
            // set content
            this.$refs.editor.setContent(this.model.contents)
          })
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
      submit () {
        this.$refs.form.validate(valid => {
          this.model.contents = this.$refs.editor.getContent()
          if (!this.model.contents) {
            this.$message.error('请输入资讯内容')
            return
          }
          if (valid) {
            if (!this.model.imgurl) {
              this.$message.error('请选择上传的图片')
              return
            }
            if (!this.model.id) {
              this.$service.article.add(this.model).then(() => {
                this.$message.success('添加成功')
                this.$router.replace('/article/index')
              })
            } else {
              this.$service.article.update(this.model).then(() => {
                this.$message.success('更新成功')
                this.$router.replace('/article/index')
              })
            }
          }
        })
      }
    }
  }
</script>
