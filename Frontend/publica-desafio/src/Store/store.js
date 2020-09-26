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
        },
        DELETE_GAME(state, payload){
            let idx = state.Jogos.findIndex(el => el.id === payload.idx)
            state.Jogos.splice(idx, 1)
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
        },
        deleteGame(context, payload){
            api
              .delete(`/api/Jogos/${payload.idx}`)
                .then(() => {
                    context.commit("DELETE_GAME", payload)
                })            
        },
        editUser(context, payload){
            console.log(payload.obj)
            api
              .put(`/api/Pessoa/${payload.obj.id}`, payload.obj)
                .then(() => {
                    sessionStorage.setItem("Pessoa", JSON.stringify(payload.obj))
                    context.commit("MODIFICAR_PESSOA", payload.obj)
                })
        }
    },
   

})