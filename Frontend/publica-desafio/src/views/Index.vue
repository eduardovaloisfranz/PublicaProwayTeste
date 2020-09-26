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
              <p class="p text-left text-success">Clique no registro para obter detalhes</p>         
            <tabela :listaDeJogos="Jogos" @click-tabela="handleClick($event)"/>              
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
    <b-modal v-model="showInfoGame" centered title="Detalhes Sobre o jogo" hide-footer>
       <b-card
        border-variant="primary"
        :header="'Placar Jogo: ' + selectedGame.placar"
        header-bg-variant="primary"
        header-text-variant="white"
        align="center"
      >
        <b-card-text>ID Jogo: <i class="text-danger">{{selectedGame.id}}</i></b-card-text>
        <b-card-text>Minimo Temporada: <i class="text-success">{{selectedGame.minimoTemporada}}</i></b-card-text>
        <b-card-text>Máximo Temporada: <i class="text-success">{{selectedGame.maximoTemporada}}</i></b-card-text>
        <b-card-text>Quebra Recorde Mínimo: <i class="text-success">{{selectedGame.quebraRecordeMin}}</i></b-card-text>
        <b-card-text>Quebra Recorde Máximo: <i class="text-success">{{selectedGame.quebraRecordeMax}}</i></b-card-text>      
      </b-card>
        <div class="d-flex justify-content-between pt-3">
            <b-button variant="danger" @click="handleDeleteGame">Excluir Jogo</b-button>
            <b-button variant="warning" @click="showInfoGame = false">Fechar Modal</b-button>
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
      placarJogo: 0,
      showInfoGame: false,
      selectedGame: {}
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
        this.makeToast(
          "success",
          "Ação concluída",
          "Registro adicionado com Sucesso"
        );
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
    },
    makeToast(variant = null, title, corpo) {
      this.$bvToast.toast(corpo, {
        title: `${title}`,
        variant: variant,
        solid: true
      });
    },   
    fetchGamesBackend(){        
        this.$store.commit("MODIFICAR_PESSOA", JSON.parse(sessionStorage.getItem("Pessoa")))          
        this.$store.dispatch("getAllGames", { idx: this.Pessoa.id})
    },
    handleClick(evt){
      this.selectedGame = evt;
      this.showInfoGame = true;      
    },
    async handleDeleteGame(){      
      let res = "";
          await this.$bvModal
            .msgBoxConfirm(
              "Você realmente tem certeza que deseja remover este placar",
              this.estiloConfirm("Esta ação é Irreversível", "danger")
          )
          .then(r => (res = r));
      if(res === true){
        this.$store.dispatch("deleteGame", { idx: this.selectedGame.id})
        this.makeToast(
          "success",
          "Ação concluída",
          "Registro apagado com Sucesso"
        );
        
      }else{
        this.$bvModal
          .msgBoxOk(
            "Placar não foi apagado",
            this.estiloMsgBox("Registro mantido", "success")
          )
          .then(() => {
            this.selectedGame = {}
            setTimeout(() => {
              this.showInfoGame = false;              
            }, 1350)
          });
      }
      
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