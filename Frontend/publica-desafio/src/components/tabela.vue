<template>
    <div>
         <b-row>
        <b-col xs="12" sm="12" md="6" lg="6" class="my-1">       
          <b-form-group
            label="Filtro"
            label-cols-sm="3"
            label-align-sm="right"
            label-size="sm"
            label-for="filterInput"
            class="mb-0"
          >
            <b-input-group size="sm">
              <b-form-input
                v-model="filtro"
                type="search"
                id="filterInput"
                placeholder="Digite algo para Filtrar"
              ></b-form-input>
            </b-input-group>
          </b-form-group>
        </b-col>

        <b-col xs="12" sm="12" md="6" lg="6" class="my-1">
          <b-form-group
            label="Por página"
            label-cols-sm="6"
            label-cols-md="4"
            label-cols-lg="3"
            label-align-sm="right"
            label-size="sm"
            label-for="perPageSelect"
            class="mb-0"
          >
            <b-form-select v-model="porPagina" id="perPageSelect" size="sm" :options="pageOptions"></b-form-select>
          </b-form-group>
        </b-col>

        <b-col xs="12" sm="12" md="12" lg="12" class="my-1">
          <b-pagination
            v-model="paginaAtual"
            :total-rows="totalRegistros"
            :per-page="porPagina"
            align="fill"
            size="sm"
            class="my-0"
          ></b-pagination>
        </b-col>
      </b-row>
      <b-row>
        <b-col xs="12" sm="12" md="12" lg="12" class="mt-3">
          <b-table
            striped
            hover
            show-empty
            small
            :current-page="paginaAtual"
            stacked="md"
            :busy="isBusy"
            :items="listaDeJogos"
            :per-page="porPagina"
            :filter="filtro"     
            :fields="fields"
            :sort-by.sync="sortBy"
            :sort-desc.sync="sortDesc"
            sort-icon-left   
          >
            <template v-slot:table-busy>
              <div class="text-center text-danger my-2">
                <b-spinner class="align-middle"></b-spinner>
                <strong>Carregando...</strong>
              </div>
            </template>
          </b-table>
        </b-col>
      </b-row>
    </div>
</template>

<script>
export default {
    props: {
        listaDeJogos: {
            type: Array,
            required: true
        }
    },
    data(){
        return {
            filtro: null,
            porPagina: 5,
            pageOptions: [5, 10, 15],
            paginaAtual: 1,
             sortBy: '',
            sortDesc: false,
            fields: [
                {key: "id", sortable: true},
                {key: "placar", sortable: true},
                {key: "minimoTemporada", sortable: true},
                {key: "maximoTemporada", sortable: true},
                {key:"quebraRecordeMin", sortable: true},
                {key:"quebraRecordeMax", sortable: true}
            ]
            
        }
    },
    computed: {
      totalRegistros() {
      return this.listaDeJogos.length;
    },
    isBusy() {
      return this.listaDeJogos.length === 0 ? true : false;
    },
    }
}
</script>

<style>

</style>