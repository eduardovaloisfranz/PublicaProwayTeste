import Vue from "vue"
import Vuex from "vuex"

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        Pessoa: null,
        Jogos: null
    },
    mutations: {
        MODIFICAR_PESSOA(state, payload){
            state.pessoa = payload;
        },
        ADICIONAR_JOGO(state, payload){
            state.Jogos.push(payload)
        },
        SETAR_JOGOS(state, payload){
            state.jogos = payload;
        }
    }

})