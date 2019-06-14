<template>
  <div class="page">
    <div class="searchbar">
      <el-date-picker v-model="created" type="date" value-format="yyyy-MM-dd"
                      placeholder="请选择创建日期"/>
      <el-button type="primary" style="margin-left: 1em" icon="el-icon-search" @click="search">搜索</el-button>
    </div>
    <div class="datagrid">
      <el-table :data="list" style="width: 100%" stripe @sort-change="handleSortChange" ref="table"
                :default-sort="{ prop: 'created', order: 'descending'}">
        <el-table-column prop="ipaddress" label="IP地址" sortable="custom"/>
        <el-table-column prop="contents" label="内容"/>
        <el-table-column prop="created" label="创建时间" sortable="custom">
          <template slot-scope="scope">
            <i class="el-icon-time"></i>
            <span style="margin-left: 10px">{{ scope.row.created | datetimeFilter}}</span>
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

  </div>
</template>

<script>
  export default {
    data () {
      return {
        currentPage: 1,
        pageSize: 10,
        orderBy: '',
        list: [],
        total: 0,
        created: ''
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
      getList () {
        this.$service.common.getCommentList(this.currentPage, this.pageSize, this.created, this.orderBy).then(data => {
          this.list = data.list
          this.total = data.count
        })
      },
      search () {
        this.currentPage = 1
        this.getList()
      }
    }
  }
</script>
