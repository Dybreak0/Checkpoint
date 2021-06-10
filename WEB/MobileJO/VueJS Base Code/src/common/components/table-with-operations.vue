<template>
  <div>
    <!-- table -->
    <el-table
      :data="list"
      empty-text="There are no people yet"
      style="width: 100%"
      v-loading.fullscreen.lock="fullscreenLoading"
    >
      <el-table-column
        v-for="col in cols"
        :key="col.key"
        :prop="col.prop"
        v-bind:label="col.label"
      >
      </el-table-column>

      <el-table-column
        fixed="right"
        v-bind:label="$t('operations')"
        width="120"
      >
        <template slot-scope="scope">
          <el-button type="text" @click="edit(scope.row.id)" size="small">{{ $t("edit") }}</el-button>
          <el-button type="text" @click="destroy(scope.row.id)" size="small">{{ $t("delete") }}</el-button>
        </template>
      </el-table-column>

    </el-table>
    <!-- table -->
  </div>
</template>

<script>
import otherUtils from '../utils/other-utils';

export default {
  name: 'app-person-table',

  props: {
    storeName: String,
    newRouter: String,
    editRouter: String,
    list: Array,
    cols: Array,
  },

  data() {
    return {
      fullscreenLoading: false,
    };
  },

  created() {
    this.fullscreenLoading = true;
    this.$store.dispatch(`${this.storeName}/list`).then(() => {
      setTimeout(() => {
        this.fullscreenLoading = false;
      }, 1000);
    });
  },

  beforeRouteLeave(to, from, next) {
    this.$store.dispatch(`${this.storeName}/clear`);
    next();
  },

  methods: {
    create() {
      this.$router.push(this.newRouter);
    },

    edit(id) {
      this.$router.push({ name: this.editRouter, params: { id } });
    },

    destroy(id) {
      this.$confirm(otherUtils.getMessage('confirmMsg'), otherUtils.getMessage('confirm'), {
        confirmButtonText: otherUtils.getMessage('yes'),
        cancelButtonText: otherUtils.getMessage('no'),
        type: 'warning',
      }).then(() => {
        this.fullscreenLoading = true;
        this.$store.dispatch(`${this.storeName}/destroy`, id).then(() => {
          setTimeout(() => {
            this.fullscreenLoading = false;
          }, 1000);
        });
      }).catch(() => {});
    },

    back() {
      this.$router.push('/home');
    },
  },
};
</script>

<style>

</style>
