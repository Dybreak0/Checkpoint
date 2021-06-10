<template>
    <v-combobox v-model="chips"
                label=""
                chips
                solo
                :rules="rules"
                class="box"
                :disabled="disabled"
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
                chips: [],
                emails: [],
                disabled: true,
                rules: [
                    v => {
                        if (v.length > 0) {
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
                            else { return true }
                        }
                        else if (v.length > constants.maxChips) {
                            return constants.maxCharsReached;
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
            },
            getValues() {
                var emails = [];
                for (let i = 0; i < this.chips.length; i++) {
                    var temp = ({ type_id: constants.ccID, email_address: this.chips[i] });
                    emails.push(temp);
                }
                return emails;
            },
            toggleEnable() {
                this.disabled = this.disabled ? false : true;
            },

        },
        created() {
            if(this.$props.list !== undefined){
                for (let i = 0; i < (this.$props.list).length; i++) {
                    if(this.$props.list[i].type_id == constants.ccID)
                    this.chips.push(this.$props.list[i].email_address);
                }
            }
        },
    }
</script>
