<template>
  <div>
    <h2 class="page-title">
      {{ $t("detailsForm") }}
    </h2>

    <div class="block">
      <el-form
        ref="form"
        :model="model"
        label-width="180px"
        :rules="rules"
        @submit.native.prevent="save"
      >
        <el-form-item v-bind:label="$t('lastName')" prop="lname">
          <el-input
            v-model="model.lname"
            ref="focus"
          />
        </el-form-item>

        <el-form-item v-bind:label="$t('firstName')" prop="fname">
          <el-input
            v-model="model.fname"
          />
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
  name: 'person-details-form',

  props: {
    id: String,
  },

  data() {
    return {
      rules: {
        lname: validators.string('Last Name', true, 3, 255),
        fname: validators.string('First Name', true, 3, 255),
      },
    };
  },

  created() {
    if (this.id) {
      this.$store.dispatch('person/tab2/edit', this.id);
    } else {
      this.$store.dispatch('person/tab2/new');
    }
  },

  beforeRouteLeave(to, from, next) {
    this.$store.dispatch('person/tab2/clear');
    next();
  },

  computed: {
    ...mapGetters({
      model: 'person/tab2/model',
    }),
  },

  methods: {
    save() {
      this.$refs.form.validate((valid) => {
        if (!valid) {
          return;
        }

        this.$store.dispatch('person/tab2/save').then(() => {
          this.back();
        });
      });
    },

    back() {
      this.$router.push('/person/details');
    },
  },
};
</script>

<style>

</style>
