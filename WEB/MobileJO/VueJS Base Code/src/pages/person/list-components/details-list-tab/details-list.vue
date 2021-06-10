<template>
  <div>
    <div class="mt-3">
      <h2 class="page-title">
      {{ $t("detailsTab") }}
      </h2>
    </div>
    <!-- table -->
    <TableWithOperations
      :storeName="storeName"
      :newRouter="newRouter"
      :editRouter="editRouter"
      :list="list"
      :cols="cols"
    >
    </TableWithOperations>
    <!-- table -->
    <!-- main buttons -->
    <div class="mt-3">
      <el-button @click="createDetail()" type="primary">{{ $t("new") }}</el-button>
      <el-button @click="back()" type="primary">{{ $t("back") }}</el-button>
    </div>
    <!-- main buttons -->
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import otherUtils from '../../../../common/utils/other-utils';
import TableWithOperations from '../../../../common/components/table-with-operations';

export default {
  name: 'app-details-list',

  components: {
    TableWithOperations,
  },

  computed: {
    ...mapGetters({
      list: 'person/tab2/list',
    }),
  },

  data() {
    return {
      cols: [
        {
          key: 'firstName',
          label: otherUtils.getMessage('firstName'),
          prop: 'fname',
          name: '1',
        },
        {
          key: 'lastName',
          label: otherUtils.getMessage('lastName'),
          prop: 'lname',
          name: '2',
        },
      ],
      storeName: 'person/tab2',
      newRouter: '/person/details/new',
      editRouter: 'detailsEdit',
    };
  },

  methods: {
    createDetail() {
      this.$router.push('/person/details/new');
    },

    back() {
      this.$router.push('/home');
    },

  },
};
</script>

<style>
</style>
