<template>
  <el-card class="main-body">
    <div style="padding: 50px">
      <div style="color:red; font-size: 5em;">404</div>
      <h3>
        <span>糟糕！页面未找到</span>
        <router-link to="/" style="text-decoration: none">返回首页</router-link>
      </h3>
    </div>
  </el-card>
</template>

<script>
  export default {
  }
</script>

<style lang="scss" scoped>
  .main-body {
    margin: 0 auto;
    width: 450px;
    text-align: center;
    margin-top: calc((100vh - 300px) / 2);
  }
</style>
