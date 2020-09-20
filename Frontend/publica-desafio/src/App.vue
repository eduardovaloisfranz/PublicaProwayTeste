<template>
<div>
    <b-container fluid>
      <b-row>
        <b-col sm="12" md="12">
            <h3 class="text-center p-3 lead h3">Ola Maria, segue a lista de seus Últimos Jogos</h3>
        </b-col>
      </b-row>
      <b-row>
        <b-col sm="12" lg="12">
          <div class="d-flex justify-content-end pb-2">
            <b-button variant="outline-success" @click="handleCadastrarPlacar">Cadastrar novo Placar</b-button>
          </div>
        </b-col>
        <b-col sm="12" md="12">         
            <div v-if="loading === true" class="d-flex justify-content-center">
              <div>
                <b-spinner style="width: 3rem; height: 3rem;" label="Large Spinner" variant="danger"></b-spinner>
                <p class="p text-info pt-2">Carregando...</p>
              </div>
            </div>
            <div v-else>
            <tabela :listaDeJogos="listOfGames"/>              
            </div>
        </b-col>
      </b-row>      
    </b-container>
    <b-modal title="Cadastrar novo Jogo" v-model="showAddGame" hide-footer centered>  
      
      <label for="input-NovoJogo">Informe o Seu Placar:</label>
          <b-form-input
            id="input-NovoJogo"
            v-model.number="placarJogo"
            :state="jogoIsValido"
            aria-describedby="input-live-help input-live-feedback"
            placeholder="Informe o seu Placar"
          ></b-form-input>
    <div class="d-flex justify-content-between pt-3">
        <b-button variant="danger" @click="handleFecharModal">Cancelar</b-button>        
        <b-button variant="outline-success" :disabled="!jogoIsValido" @click="handleAddGame">Cadastrar Novo Jogo</b-button>
      </div>  
    </b-modal>
</div>
</template>

<script>
import tabela from "@/components/tabela.vue"
import {api} from "@/services"
export default {  
  name: 'App',
  components: {
    Tabela: tabela
  },
  data(){
    return {
      listOfGames: null,
      loading: true,
      showAddGame: false,
      placarJogo: 0
    }
  },
  methods: {
    getAllGames(){
      setTimeout(()=> {   
        api
        .get("/api/jogos")
          .then(r => {
            this.loading = false;
            this.listOfGames = r.data
          })
      }, 1400)        
    },
    handleCadastrarPlacar(){
        this.showAddGame = true;
    },
    handleFecharModal(){
      this.showAddGame = false;
      this.placarJogo = 0;
    },
    async handleAddGame(){
      let res = "";
          await this.$bvModal
            .msgBoxConfirm(
              "Você realmente tem certeza que deseja adicionar este placar?",
              this.estiloConfirm("Confirmação", "info")
          )
          .then(r => (res = r));
      if(res === true){
        let obj = {
          placar: this.placarJogo
        }
        api
          .post("/api/Jogos", obj)
            .then(res => {
              this.listOfGames.push(res.data);
              this.placarJogo = 0;
              setTimeout(() => {
                this.showAddGame = false;
              }, 1200)
            })
      }
    },
     estiloConfirm(title, variant) {
      return {
        title: title,
        size: "sm",
        buttonSize: "sm",
        okVariant: variant || "info",
        okTitle: "Sim",
        cancelTitle: "Não",
        footerClass: "p-2",
        hideHeaderClose: false,
        centered: true
      };
    },
    estiloMsgBox(title, variant) {
      return {
        title: title,
        size: "sm",
        buttonSize: "sm",
        okVariant: variant || "success",
        headerClass: "p-2 border-bottom-0",
        footerClass: "p-2 border-top-0",
        centered: true
      };
    }
  },
  created(){
    this.getAllGames();
  },
  computed: {
    jogoIsValido(){
       return this.placarJogo > 0 && this.placarJogo <= 1000
    }
  }
}
</script>

<style>

</style>
