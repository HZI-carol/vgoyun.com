<template>
  <div class="page">
    <div class="searchbar">
      <el-input v-model.trim="keyword" style="width: 15em; margin-right: 1em" placeholder="请输入关键字"
                prefix-icon="el-icon-search"
                clearable @keyup.enter.native="search"></el-input>
      <el-checkbox-group v-model="intentions" @change="handleChange">
        <el-checkbox-button :label="1" :value="1">兼职做</el-checkbox-button>
        <el-checkbox-button :label="2" :value="2">想创业</el-checkbox-button>
        <el-checkbox-button :label="3" :value="3">看看而已</el-checkbox-button>
        <el-checkbox-button :label="4" :value="4">官网联系我们</el-checkbox-button>
      </el-checkbox-group>
      <el-button type="primary" style="margin-left: 1em" icon="el-icon-search" @click="search">搜索</el-button>
      <el-button style="margin-left: 1em" icon="el-icon-refresh" @click="reset">重置</el-button>
    </div>
    <div class="datagrid">
      <el-table :data="list" style="width: 100%" stripe @sort-change="handleSortChange" ref="table">
        <el-table-column prop="name" label="姓名"/>
        <el-table-column prop="phone" label="手机号" sortable="custom"/>
        <el-table-column prop="intention_text" label="意向度"/>
        <el-table-column prop="remark" label="备注"/>
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
        keyword: '',
        intentions: [],
        intention: ''
      }
    },
    created () {
      this.getList()
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
        this.$service.intention.getList(this.currentPage, this.pageSize, this.keyword, this.intention, this.orderBy).then(data => {
          this.list = data.list
          this.total = data.count
        })
      },
      handleChange (val) {
        this.intention = val.join(',')
        this.search()
      },
      search () {
        this.currentPage = 1
        this.getList()
      },
      reset () {
        this.keyword = ''
        this.intention = ''
        this.intentions = []
        this.search()
      }
    }
  }
</script>
