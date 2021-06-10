<template>
    <v-dialog v-model="dialog" persistent :max-width="options.width" @keypress.esc="cancel" v-bind:style="{ zIndex: options.zIndex }">
        <v-card>
            <v-toolbar class="modal-nav" dark :color="options.color" dense flat>
                <v-toolbar-title class="white--text">{{ title }}</v-toolbar-title>
            </v-toolbar>
            <v-card-text v-show="!!message">{{ message }}</v-card-text>
            <v-card-actions class="pt-0">
                <v-spacer></v-spacer>
                <v-btn color="darken-1" flat="flat" @click.native="ok">OK</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
    export default {
        data: () => ({
            dialog: false,
            resolve: null,
            reject: null,
            message: null,
            title: null,
            options: {
                color: '#ffff',
                width: 500,
                zIndex: 100
            }
        }),
        methods: {
            open(title, message, options) {
                this.dialog = true
                this.title = title
                this.message = message
                this.options = Object.assign(this.options, options)
                return new Promise((resolve, reject) => {
                    this.resolve = resolve
                    this.reject = reject
                })
            },
            ok() {
                this.resolve(true)
                this.dialog = false 
            },
            cancel() {
                this.dialog = false
            }
        }
    }
</script>
