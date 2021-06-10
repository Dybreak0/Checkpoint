<template>
    <v-combobox v-model="chips"
                label=""
                chips
                solo
                :rules="rules"
                :disabled="disabled"
                class="box"
                color="red"
                multiple
                maxlength="50">
        <template v-slot:selection="data">
            <v-chip :selected="data.selected"
                    close
                    :color="getColor(data.item)"
                    @input="remove(data.item)">
                <strong>{{ data.item }}</strong>&nbsp;
            </v-chip>
        </template>

    </v-combobox>

</template>

<script>
    import validators from '@/common/utils/form/validators';
    import constants from '../../../common/utils/constants';
    export default {
        data() {
            return {
                color: '',
                items: [],
                chips: [],
                disabled: true,
                emails: [],
                rules: [
                    v => {
                        if (!v || v.length < 1)
                            return constants.fillRequireFieldsError;
                        else if (v.length > constants.maxChips){
                            return constants.maxCharsReached;
                        }
                        else if (v.length > 0) {
                            var result = false
                            for (let i = 0; i < v.length; i++) {
                                if (!((validators.emailtextFormat).test(v[i]))) {
                                    result = true
                                }
                                else if (!((validators.emailFormat).test(v[i]))) {
                                    result = true
                                }
                            }
                            if (result) { return constants.invalidEmailError }
                            else {return true }
                        }
                        else return true;
                    }
                ],
            }
        },

        props: {
            list: Array
        },

        methods: {
            getColor(v) {
                if (!((validators.emailtextFormat).test(v))) {
                    return constants.invalid_email_color;
                }
                else if (!((validators.emailFormat).test(v))) {
                    return constants.invalid_email_color;
                } else {
                    return constants.valid_email_color;
                }
            },
            remove(item) {
                this.chips.splice(this.chips.indexOf(item), 1)
                this.chips = [...this.chips]
                this.adjuster++;
            },
            getValues() {
                var emails = [];
                for (let i = 0; i < this.chips.length; i++) {
                    var temp = ({ type_id: constants.toID, email_address: this.chips[i] });
                    emails.push(temp);
                }

                return emails;
            },
            toggleEnable() {
                this.disabled = this.disabled ? false : true;
            },

        },
        created() {
            this.items = this.$props.list;
            if (this.items !== undefined){
                for (let i = 0; i < this.items.length; i++) {
                    if (this.items[i].type_id === constants.toID)
                        this.chips.push(this.items[i].email_address);
                }
            }
        },

    }
</script>

<style>
</style>
