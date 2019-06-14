<template>
  <el-dialog :visible.sync="dialogVisible" append-to-body :show-close="false"
             custom-class="image-preview-dialog" fullscreen top="0">
    <div class="close">
      <i class="el-icon-error" @click="dialogVisible = false"></i>
    </div>
    <div class="image-panel" @click="dialogVisible = false">
      <img :src="dialogImageUrl"/>
    </div>
  </el-dialog>
</template>

<script>
  export default {
    data () {
      return {
        dialogVisible: false,
        dialogImageUrl: ''
      }
    }
  }
</script>

<style lang="scss">

  // 图片预览弹窗样式
  .image-preview-dialog {
    background: transparent !important;
    box-shadow: none !important;

    .el-dialog__header {
      display: none;
    }

    .el-dialog__body {
      padding: 0;
    }

    .close {
      text-align: right;

      i {
        font-size: 36px;
        color: rgba(255, 255, 255, 0.6);
        cursor: pointer;
        margin-top: 9px;
        margin-right: 10px;
      }
    }

    .image-panel {
      display: flex;
      align-items: center;
      justify-content: center;
      min-height: calc(100vh - 45px);

      img {
        max-width: 90vw;
        max-height: calc(90vh - 45px)
      }
    }
  }
</style>
