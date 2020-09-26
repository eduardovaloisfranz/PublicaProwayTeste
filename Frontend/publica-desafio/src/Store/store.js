import Vue from "vue"
import Vuex from "vuex"

Vue.use(Vuex)
import { api } from "@/services"

export default new Vuex.Store({
    state: {
        Pessoa: {},
        Jogos: []
    },
    mutations: {
        MODIFICAR_PESSOA(state, payload){            
            state.Pessoa = payload;
        },
        ADICIONAR_JOGO(state, payload){
            state.Jogos.push(payload)
        },
        SETAR_JOGOS(state, payload){
            state.Jogos = payload;
        }
    },
    actions: {
        addGameToList(context, payload){           
            api
            .post("/api/Jogos", payload.obj)
              .then(res => {
                context.commit("ADICIONAR_JOGO", res.data)                
              })        
        },
        getAllGames(context, payload){
            api
              .get(`/api/Jogos/${payload.idx}`)
                .then(res => {                    
                    context.commit("SETAR_JOGOS", res.data)
                })
        }

    },
   

})