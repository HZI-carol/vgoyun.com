// vue.config.js 配置文档地址：https://cli.vuejs.org/zh/config
const webpack = require('webpack')

module.exports = {
  publicPath: process.env.NODE_ENV === 'production'
    ? '/admin/'
    : '/',
  outputDir: 'dist',
  // 静态资源
  assetsDir: 'static',
  // 指定生成的 index.html 的输出路径
  // indexPath: 'index.html',
  runtimeCompiler: true,
  productionSourceMap: false,
  configureWebpack: {
    plugins: [
      new webpack.ProvidePlugin({
        'window.Quill': 'quill/dist/quill.js',
        'Quill': 'quill/dist/quill.js'
      }),
    ]
  },
  pages: {
    index: {
      // page 的入口
      entry: 'src/main.js',
      // 模板来源
      template: process.env.NODE_ENV === 'production'
        ? 'public/index.html'
        : 'public/index.html',
      // 在 dist/index.html 的输出
      filename: 'index.html',
      // 当使用 title 选项时，
      // template 中的 title 标签需要是 <title><%= htmlWebpackPlugin.options.title %></title>
      // title: 'Index Page',
      // 在这个页面中包含的块，默认情况下会包含
      // 提取出来的通用 chunk 和 vendor chunk。
      chunks: ['chunk-vendors', 'chunk-common', 'index']
    }
  }
}
