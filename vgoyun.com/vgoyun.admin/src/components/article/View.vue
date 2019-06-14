<template>
  <div class="dialog-body" style="max-height: 600px; overflow-y: auto">
    <h2>{{model.title}}</h2>
    <div>
      <span>创建时间: {{model.created | datetimeFilter}}</span>
      <span style="margin-left: 1em;">{{model.author}}</span>
    </div>
    <div class="samllcontent">{{model.samllcontents}}</div>
    <div class="editor-content" style="margin-top: 1em;" v-html="model.contents"></div>
  </div>
</template>

<script>
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
    }
  }
</script>

<style lang="scss" soped>
  .samllcontent {
    background-color: #ddd;
    margin-top: 1em;
    padding: 1em;
  }
</style>
