<template>
  <quill-editor v-model="content" :class="className" :options="editorOption" @ready="onEditorReady($event)"/>
</template>

<script>
  import { container, QuillWatch } from 'quill-image-extend-module'
  // 允许上传的图片类型
  const ImageTypes = ['image/png', 'image/jpg', 'image/jpeg', 'image/gif', 'image/bmp']
  // 最大允许上传的图片大小
  const MaxFileSize = 5

  export default {
    props: {
      className: {
        type: String,
        required: false
      }
    },
    data () {
      return {
        content: '',
        editorOption: {
          modules: {
            toolbar: {
              container: container,
              handlers: {
                'image': function () {
                  QuillWatch.emit(this.quill.id)
                }
              }
            },
            imageResize: {
              displayStyles: {
                backgroundColor: 'black',
                border: 'none',
                color: 'white'
              },
              modules: ['Resize', 'DisplaySize', 'Toolbar']
            },
            ImageExtend: {
              name: 'img',
              size: MaxFileSize, // 单位为M, 1M = 1024KB
              action: '',
              change: (xhr, formData) => {
                const img = formData.get('img')
                if (!ImageTypes.includes(img.type)) {
                  this.$message.error('上传的图片格式不正确')
                  throw new Error('上传的图片格式不正确')
                }
                formData.append('token', this.$session.token)
              },
              sizeError: () => {
                this.$message.error(`上传的图片大小不能超过${MaxFileSize}M`)
              },
              error: () => {
                this.$message.error('上传出错，请稍后再试~')
              },
              response: (res) => {
                if (res.code !== 200) {
                  return ''
                }
                return res.data.fullurl
              }
            }
          }
        }
      }
    },
    created () {
      // 设置上传地址
      this.editorOption.modules.ImageExtend.action = this.$http.defaults.baseURL + 'api/admin/editor/image/upload'
    },
    methods: {
      onEditorReady (e) {
        this.$emit('ready', e)
      },
      setContent (content) {
        this.content = content
      },
      getContent () {
        return this.content
      }
    }
  }
</script>

<style>
  .ql-toolbar.ql-snow {
    line-height: 24px !important;
  }
</style>
