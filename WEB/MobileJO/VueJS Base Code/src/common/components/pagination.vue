<template>
    <ul class="pagination">
        <li class="pagination-item">
            <v-btn 
                class="pagination-button"
                flat
                icon
                @click="onClickFirstPage" 
                :disabled="isInFirstPage" 
                aria-label="Go to first page">
                    <v-icon>first_page</v-icon>
            </v-btn>
        </li>
        <li class="pagination-item">
            <v-btn  
                class="pagination-button"
                flat
                icon
                @click="onClickPreviousPage"
                :disabled="isInFirstPage"
                aria-label="Go to previous page">
                    <v-icon>chevron_left</v-icon>
            </v-btn>
        </li>
        <li v-for="page in pages" class="pagination-item" :key="page.name">
            <v-btn 
                class="pagination-button"
                fab
                small 
                @click="onClickPage(page.name)"
                :class="{ active: isPageActive(page.name) }"
                :aria-label="`Go to page number ${page.name}`">
                    {{ page.name }}
            </v-btn>
        </li>
        <li class="pagination-item">
            <v-btn 
                class="pagination-button"
                flat
                icon
                @click="onClickNextPage"
                :disabled="isInLastPage"
                aria-label="Go to next page">
                    <v-icon>chevron_right</v-icon>
            </v-btn>
        </li>
        <li class="pagination-item">
            <v-btn 
                class="pagination-button"
                flat
                icon 
                @click="onClickLastPage"
                :disabled="isInLastPage"
                aria-label="Go to last page">
                    <v-icon>last_page</v-icon>
            </v-btn>
        </li>
    </ul>
</template>

<script>
    export default {
        name: 'pagination',
        template: '#pagination',
        props: {
            maxVisibleButtons: {
                type: Number,
                required: false,
                default: 3                
            },
            totalPages: {
                type: Number,
                required: true
            },
            total: {
                type: Number,
                required: false
            },
            perPage: {
                type: Number,
                required: false
            },
            currentPage: {
                type: Number,
                required: true
            },
        },
        computed: {
            startPage() {
                if (this.totalPages <= this.maxVisibleButtons) {
                   return 1; 
                } else {
                    if (this.currentPage === 1) {
                        return 1;
                    }

                    if (this.currentPage === this.totalPages){
                        return this.totalPages - this.maxVisibleButtons + 1
                    }

                    if (this.totalPages - this.currentPage <= 2 && this.maxVisibleButtons > 3) {
                        return this.totalPages - this.maxVisibleButtons + 1
                    }
                    
                    return this.currentPage - 1;
                }
            },
            endPage() {                
                return Math.min(this.startPage + (this.maxVisibleButtons - 1), this.totalPages);
            },
            pages() {
                const range = [];

                for (let i = this.startPage; i <= this.endPage; i++) {
                    range.push({
                        name: i
                    });
                }

                return range;
            },
            isInFirstPage() {
                return this.currentPage === 1;
            },
            isInLastPage() {
                return this.currentPage === this.totalPages;
            },
        },
        methods: {
            onClickFirstPage() {
                this.$emit('pagechanged', 1);
            },
            onClickPreviousPage() {
                this.$emit('pagechanged', this.currentPage - 1);
            },
            onClickPage(page) {
                this.$emit('pagechanged', page);
            },
            onClickNextPage() {
                this.$emit('pagechanged', this.currentPage + 1);
            },
            onClickLastPage() {
                this.$emit('pagechanged', this.totalPages);    
            },
            isPageActive(page) {
                return this.currentPage === page;
            },
        }
    };
</script>

<style>

</style>