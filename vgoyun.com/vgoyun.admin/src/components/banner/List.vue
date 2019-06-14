<template>
  <div class="page">
    <div class="searchbar">
      <el-input v-model.trim="title" style="width: 15em;" placeholder="请输入标题" prefix-icon="el-icon-search"
                clearable @keyup.enter.native="search"></el-input>
      <el-select v-model="type" placeholder="请选择类型" style="margin-left: 1em;" clearable>
        <el-option label="首页" :value="1"></el-option>
      </el-select>
      <el-button type="primary" style="margin-left: 1em" icon="el-icon-search" @click="search">搜索</el-button>
      <el-button style="margin-left: 1em" icon="el-icon-refresh" @click="reset">重置</el-button>
      <el-button type="primary" icon="el-icon-plus" style="margin-left: 1em;" @click="openEditDialog()">添加</el-button>
    </div>
    <div class="datagrid">
      <el-table :data="list" style="width: 100%" stripe @sort-change="handleSortChange" ref="table"
                :default-sort="{ prop: 'created', order: 'descending'}" row-key="id" @row-dblclick="onRowDbClick">
        <el-table-column type="expand">
          <template slot-scope="props">
            <el-form label-position="left" inline class="table-expand">
              <el-form-item label="跳转链接">
                <a :href="props.row.link_url" target="_blank">{{props.row.link_url}}</a>
              </el-form-item>
            </el-form>
          </template>
        </el-table-column>
        <el-table-column label="图片">
          <template slot-scope="scope">
            <img :src="scope.row.full_image_url" style="height: 60px;" title="点击查看大图"
                 @click="$previewImage(scope.row.full_image_url)"/>
          </template>
        </el-table-column>
        <el-table-column prop="title" label="标题" sortable="custom"/>
        <el-table-column prop="sort" label="排序值" sortable="custom"/>
        <el-table-column prop="type" label="类型" sortable="custom">
          <template slot-scope="scope">
            <div>{{scope.row.type_text}}</div>
          </template>
        </el-table-column>
        <el-table-column prop="created" label="创建时间" sortable="custom">
          <template slot-scope="scope">
            <i class="el-icon-time"></i>
            <span style="margin-left: 10px">{{ scope.row.created | datetimeFilter}}</span>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="150">
          <template slot-scope="scope">
            <el-button size="mini" title="更新" icon="el-icon-edit" @click="openEditDialog(scope.row)"></el-button>
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

    <!-- edit dialog -->
    <el-dialog :title="!banner.id ? '添加Banner' : '编辑Banner'" :visible.sync="showEditDialog"
               :close-on-click-modal="false" top="8vh">
      <banner-edit :model="banner" :dialogBodyVisible="showEditDialog" ref="banneredit"></banner-edit>
      <div slot="footer" class="dialog-footer">
        <el-button @click="showEditDialog = false">取 消</el-button>
        <el-button type="primary" @click="submit">确 定</el-button>
      </div>
    </el-dialog>

  </div>
</template>

<script>
  import BannerEdit from './Edit'

  export default {
    components: {
      BannerEdit
    },
    data () {
      return {
        currentPage: 1,
        pageSize: 10,
        orderBy: '',
        list: [],
        total: 0,
        showEditDialog: false,
        title: '',
        type: '',
        banner: {}
      }
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
        this.$service.banner.getList(this.currentPage, this.pageSize, this.title, this.type, this.orderBy).then(data => {
          this.list = data.list
          this.total = data.count
        })
      },
      reset () {
        this.title = ''
        this.type = ''
        this.search()
      },
      search () {
        this.currentPage = 1
        this.getList()
      },
      openEditDialog (banner) {
        this.banner = {
          id: 0,
          title: '',
          image_url: '',
          link_url: '',
          type: 1,
          sort: 0
        }
        this.banner = Object.assign(this.banner, banner || {})
        this.showEditDialog = true
      },
      submit () {
        this.$refs.banneredit.submit(() => {
          this.showEditDialog = false
          this.getList()
        })
      },
      remove (banner) {
        this.$confirm(`确定要删除 <strong>${banner.title}</strong>  吗?`, '删除提示', {
          dangerouslyUseHTMLString: true,
          type: 'warning'
        }).then(() => {
          this.$service.banner.remove(banner.id).then(d => {
            this.$message.success('删除成功!')
            this.getList()
          })
        })
      }
    }
  }
</script>
