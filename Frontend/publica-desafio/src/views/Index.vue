<template>
<div v-if="podeVisualizar === true">
    <b-container fluid>
      <b-row>
        <b-col sm="12" md="12">          
            <h3 class="text-center p-3 lead h3">Ola <i class="text-info">{{Pessoa.nomeCompleto}}</i>, segue a lista de seus Últimos Jogos</h3>
        </b-col>
      </b-row>
      <b-row>
        <b-col sm="12" lg="12">
          <div class="d-flex justify-content-end pb-2">
            <b-button variant="outline-success" @click="handleCadastrarPlacar">Cadastrar novo Placar</b-button>
          </div>
        </b-col>
        <b-col sm="12" md="12">  
            <div v-if="possuiJogos">           
            <tabela :listaDeJogos="Jogos"/>              
            </div>
            <div v-else>
                <h2 class="h2 text-center text-danger">Você não possui nenhum jogo cadastrado, por favor adicione clicando no botão adicionar para popular esta lista</h2>
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
<div v-else>
    <b-container>
        <b-row>
            <b-col sm="12" md="12" lg="12">
                <h1 class="h1 text-center text-danger lead">Você não Possui permissão necessária para visualizar esta página, por favor efetue o login</h1>
                <h2 class="h2 text-center text-info lead">Aguarde aqui e será redicionado para efetuar o login</h2>                                
            </b-col>
        </b-row>
    </b-container>
</div>
</template>

<script>
import tabela from "@/components/tabela.vue"
import {mapState} from "vuex"
export default {  
  name: 'Index',
  components: {
    Tabela: tabela
  },
  data(){
    return {     
      showAddGame: false,
      placarJogo: 0
    }
  },
  methods: {    
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
        let objX = {
          placar: this.placarJogo,
          pessoaID: this.Pessoa.id
        }
        
        this.$store.dispatch("addGameToList", { obj: objX})
        setTimeout(() => {
            this.placarJogo = 0;
            this.showAddGame = false;
        }, 1325)      
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
    fetchGamesBackend(){        
        this.$store.commit("MODIFICAR_PESSOA", JSON.parse(sessionStorage.getItem("Pessoa")))          
        this.$store.dispatch("getAllGames", { idx: this.Pessoa.id})
    }
  },
  created(){    
    if(this.podeVisualizar === true){
      this.fetchGamesBackend();    
    }
    if(this.podeVisualizar === false) {
        setTimeout(() => {
            this.$router.push("/login")
        }, 5000)
    }
  },
  computed: {
    ...mapState(["Pessoa", "Jogos"]),

    jogoIsValido(){
       return this.placarJogo > 0 && this.placarJogo <= 1000
    },
    podeVisualizar(){
        return sessionStorage.getItem("token") === null ? false : true
    },
    possuiJogos(){     
     if(this.Jogos.length === 0)
      {
        return false;
      }else{
        return true;
      }
      
    }
  
    
    
  }
}
</script>

<style>
</style>