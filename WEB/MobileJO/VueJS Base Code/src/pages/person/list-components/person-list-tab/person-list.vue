<template>
  <div>
    <div class="mt-3">
      <h2 class="page-title">
        {{ $t("peopleTab") }}
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
      <el-button @click="create()" type="primary">{{ $t("new") }}</el-button>
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
  name: 'app-person-tab1',

  components: {
    TableWithOperations,
  },

  computed: {
    ...mapGetters({
      list: 'person/tab1/list',
    }),
  },

  data() {
    return {
      cols: [
        {
          key: 'name',
          label: otherUtils.getMessage('name'),
          prop: 'name',
          name: '1',
        },
        {
          key: 'role',
          label: otherUtils.getMessage('role'),
          prop: 'role',
          name: '2',
        },
      ],
      storeName: 'person/tab1',
      newRouter: '/person/new',
      editRouter: 'personEdit',
    };
  },

  methods: {
    create() {
      this.$router.push('/person/new');
    },

    back() {
      this.$router.push('/home');
    },
  },
};
</script>
<style>
</style>
