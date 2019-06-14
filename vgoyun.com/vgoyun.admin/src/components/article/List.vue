<template>
  <div class="page">
    <div class="searchbar">
      <el-input v-model.trim="title" style="width: 15em;" placeholder="请输入标题" prefix-icon="el-icon-search"
                clearable @keyup.enter.native="search"></el-input>
      <el-select v-model="type" placeholder="请选择类型" style="margin-left: 1em; width: 150px;" clearable>
        <el-option v-for="item in types" :key="item.typeid" :label="item.text" :value="item.typeid">
        </el-option>
      </el-select>
      <el-select v-model="ishot" style="margin-left: 1em; width: 150px;" placeholder="请选择是否热点" clearable>
        <el-option label="是" :value="1"/>
        <el-option label="否" :value="0"/>
      </el-select>
      <el-select v-model="isnew" style="margin-left: 1em; width: 150px;" placeholder="请选择是否最新" clearable>
        <el-option label="是" :value="1"/>
        <el-option label="否" :value="0"/>
      </el-select>
      <el-select v-model="isshow" style="margin-left: 1em; width: 150px;" placeholder="请选择是否推荐" clearable>
        <el-option label="是" :value="1"/>
        <el-option label="否" :value="0"/>
      </el-select>
      <el-button type="primary" style="margin-left: 1em" icon="el-icon-search" @click="search">搜索</el-button>
      <el-button style="margin-left: 1em" icon="el-icon-refresh" @click="reset">重置</el-button>
      <el-button type="primary" icon="el-icon-plus" style="margin-left: 1em;" @click="$router.push('/article/edit')">
        添加
      </el-button>
    </div>
    <div class="datagrid">
      <el-table :data="list" style="width: 100%" stripe @sort-change="handleSortChange" ref="table"
                :default-sort="{ prop: 'created', order: 'descending'}" row-key="id" @row-dblclick="onRowDbClick">
        <el-table-column type="expand">
          <template slot-scope="props">
            <el-form label-position="left" inline class="table-expand">
              <el-form-item label="简介" width="100%">
                {{props.row.samllcontents}}
              </el-form-item>
            </el-form>
          </template>
        </el-table-column>
        <el-table-column label="图片">
          <template slot-scope="scope">
            <img :src="scope.row.fullimgurl" style="height: 60px;" title="点击查看大图"
                 @click="$previewImage(scope.row.fullimgurl)"/>
          </template>
        </el-table-column>
        <el-table-column prop="title" label="标题" sortable="custom"/>
        <el-table-column prop="author" label="作者" sortable="custom"/>
        <el-table-column prop="sort" label="排序值" sortable="custom"/>
        <el-table-column prop="typeid" label="类型" sortable="custom">
          <template slot-scope="scope">
            <div>{{scope.row.typetext}}</div>
          </template>
        </el-table-column>
        <el-table-column prop="seecount" label="查看数" sortable="custom"/>
        <el-table-column label="热点" width="100px">
          <template slot-scope="scope">
            <el-tag :type="scope.row.ishot ? 'success' : 'danger'" close-transition>
              {{scope.row.ishot ? '是' : '否'}}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="最新" width="100px">
          <template slot-scope="scope">
            <el-tag :type="scope.row.isnew ? 'success' : 'danger'" close-transition>
              {{scope.row.isnew ? '是' : '否'}}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="推荐" width="100px">
          <template slot-scope="scope">
            <el-tag :type="scope.row.isshow ? 'success' : 'danger'" close-transition>
              {{scope.row.isshow ? '是' : '否'}}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="created" label="创建时间" sortable="custom">
          <template slot-scope="scope">
            <i class="el-icon-time"></i>
            <span style="margin-left: 10px">{{ scope.row.created | datetimeFilter}}</span>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="200">
          <template slot-scope="scope">
            <el-button size="mini" @click="openViewDialog(scope.row)">预览</el-button>
            <el-button size="mini" title="更新" icon="el-icon-edit"
                       @click="$router.push('/article/edit?id=' + scope.row.id)"></el-button>
            <el-button size="mini" title="删除" type="danger" icon="el-icon-delete"
                       @click="remove(scope.row)"></el-button>
          </template>
        </el-table-column>
      </el-table>
    </div>
    <div class="pagination" v-show="total > 0">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page="currentPage"
                     :page-sizes="[10, 20, 30]" :page-size="pageSize"
                     layout="total, sizes, prev, pager, next, jumper"
                     :total="total">
      </el-pagination>
    </div>

    <!-- view dialog-->
    <el-dialog title="资讯预览" :visible.sync="showViewDialog" width="900px" top="10vh">
      <article-view :model="article" :dialogBodyVisible="showViewDialog"></article-view>
    </el-dialog>
  </div>
</template>

<script>
  import ArticleView from './View'

  export default {
    components: {
      ArticleView
    },
    data () {
      return {
        types: [],
        currentPage: 1,
        pageSize: 10,
        orderBy: '',
        list: [],
        total: 0,
        showViewDialog: false,
        title: '',
        type: '',
        ishot: '',
        isnew: '',
        isshow: '',
        article: {}
      }
    },
    created () {
      this.$service.common.getTypeList(1).then(d => {
        this.types = d
      })
    },
    methods: {
      handleSortChange (target) {
        if (target.column) {
          let order = 'DESC'
          if (target.order === 'ascending') {
            order = 'ASC'
          }
          this.orderBy = target.prop + '_' + order
        } else {
          this.orderBy = 'created_DESC'
        }
        this.getList()
      },
      handleSizeChange (pageSize) {
        this.pageSize = pageSize
        this.currentPage = 1
        this.getList()
      },
      handleCurrentChange (currentPage) {
        this.currentPage = currentPage
        this.getList()
      },
      onRowDbClick (row, event) {
        this.$refs.table.toggleRowExpansion(row)
      },
      getList () {
        this.$service.article.getList(this.currentPage, this.pageSize, this.title, this.type, this.ishot, this.isnew, this.isshow, this.orderBy).then(data => {
          this.list = data.list
          this.total = data.count
        })
      },
      reset () {
        this.title = ''
        this.type = ''
        this.ishot = ''
        this.isnew = ''
        this.isshow = ''
        this.search()
      },
      search () {
        this.currentPage = 1
        this.getList()
      },
      openViewDialog (article) {
        this.article = article
        this.showViewDialog = true
      },
      submit () {
        this.$refs.articleedit.submit(() => {
          this.showEditDialog = false
          this.getList()
        })
      },
      remove (article) {
        this.$confirm(`确定要删除 <strong>${article.title}</strong>  吗?`, '删除提示', {
          dangerouslyUseHTMLString: true,
          type: 'warning'
        }).then(() => {
          this.$service.article.remove(article.id).then(d => {
            this.$message.success('删除成功!')
            this.getList()
          })
        })
      }
    }
  }
</script>
