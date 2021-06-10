<template>
  <div>
    <h2 class="page-title">
      {{ $t("personForm") }}
    </h2>

    <div class="block">
      <el-form
        ref="form"
        :model="model"
        label-width="180px"
        :rules="rules"
        @submit.native.prevent="save"
      >
        <el-form-item v-bind:label="$t('name')" prop="name">
          <el-input
            v-model="model.name"
            ref="focus"
          />
        </el-form-item>

        <el-form-item v-bind:label="$t('role')" prop="role">
          <el-select
            v-model="model.role"
            filterable
          >
            <template v-if="options.roles">
              <el-option
                v-for="option in options.roles"
                :key="option"
                :label="option"
                :value="option"
              />
            </template>
          </el-select>
        </el-form-item>

        <el-form-item>
          <el-button type="primary" @click="save()">{{ $t("save") }}</el-button>
          <el-button @click="back()">{{ $t("cancel") }}</el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import validators from '@/common/utils/form/validators';

export default {
  name: 'app-person-form',

  props: {
    id: String,
  },

  data() {
    return {
      rules: {
        name: validators.string('Name', true, 3, 255),
        role: validators.generic('Role', true),
      },
    };
  },

  created() {
    this.$store.dispatch('person/tab1/loadOptions');

    if (this.id) {
      this.$store.dispatch('person/tab1/edit', this.id);
    } else {
      this.$store.dispatch('person/tab1/new');
    }
  },

  beforeRouteLeave(to, from, next) {
    this.$store.dispatch('person/tab1/clear');
    next();
  },

  computed: {
    ...mapGetters({
      model: 'person/tab1/model',
      options: 'person/tab1/options',
    }),
  },

  methods: {
    save() {
      this.$refs.form.validate((valid) => {
        if (!valid) {
          return;
        }

        this.$store.dispatch('person/tab1/save').then(() => {
          this.backToList();
        });
      });
    },

    back() {
      this.$router.push('/person/person');
    },

    backToList() {
      this.$router.push('/person/person');
    },
  },
};
</script>

<style>

</style>
