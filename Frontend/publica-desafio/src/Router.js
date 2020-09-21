import Vue from 'vue'
import VueRouter from 'vue-router'
import Index from "@/views/Index"
import LoginPage from "@/views/LoginPage"
import Cadastro from "@/views/Cadastro"

Vue.use(VueRouter)

export default new VueRouter({
    mode: 'history',
    routes: [
        {
            name: "Index",
            path: "/",
            component: Index
        },
        {
            name: "Login",
            path: "/Login",
            component: LoginPage
        },
        {
            name: "Cadastro",
            path: "/Cadastro",
            component: Cadastro
        }
    ]    
})
