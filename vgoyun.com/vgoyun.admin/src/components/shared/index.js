import ImageDialog from './ImageDialog'

let $previewImage

export default {
  install (Vue, options) {
    if (!$previewImage) {
      const Dialog = Vue.extend(ImageDialog)
      const $imageDialog = new Dialog({
        el: document.createElement('div')
      })
      document.body.appendChild($imageDialog.$el)
      // set preview image function
      $previewImage = (url) => {
        $imageDialog.dialogImageUrl = url
        $imageDialog.dialogVisible = true
      }
    }

    Vue.mixin({
      created () {
        // add image preview dialog function
        this.$previewImage = $previewImage
      }
    })
  }
}
