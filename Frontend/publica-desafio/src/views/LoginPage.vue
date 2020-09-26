<template>
  <b-container>
    <b-row class="pt-3 d-flex justify-content-center align-items-center">
      <b-col cols="12" sm="8" md="4" lg="4">
        <h2 class="h2 text-center text-monospace">Por favor, faça login</h2>
        <b-form>
          <b-form-group>
            <b-form-input
              id="input-1"
              v-model="user.email"
              type="email"
              required
              placeholder="Informe o email"
            ></b-form-input>
          </b-form-group>
          <b-form-group>
            <b-form-input
              id="input-2"
              v-model="user.senha"
              type="password"
              required
              placeholder="Informe sua senha"
            ></b-form-input>
          </b-form-group>
          <p
            class="p text-capitalize text-monospace text-danger text-right"
            @click.prevent="handleEsquecimentoSenha"
          >Esqueci minha Senha</p>
          <div class="d-flex justify-content-between">
            <b-button
              variant="outline-success"
              @click.prevent="efetuarLogin"
              :disabled="!loginIsValido"
              type="submit"
            >Login</b-button>
            <b-button @click.prevent to="cadastro" variant="outline-info">Criar Conta</b-button>
          </div>
        </b-form>
      </b-col>
    </b-row>
    <b-modal v-model="modalRecuperacaoSenha" title="Recuperação de Senha" hide-footer centered>
      <section v-if="tokenRecoveryPassword === null">
        <b-form-group>
          <p
            class="p text-center text-info"
          >Informe seu email para receber um código de acesso no seu email cadastrado</p>
          <b-form-input
            id="input-1"
            v-model="emailRecuperacaoSenha"
            type="email"
            required
            placeholder="Informe o email"
          ></b-form-input>
        </b-form-group>
        <div class="d-flex justify-content-center">
          <b-button variant="success" @click="handleRecoveryPassword">Enviar Email!</b-button>
        </div>
      </section>
      <section v-else>
        <b-form-group>
          <p
            class="p text-center text-info"
          >Informe o token informado no seu email para continuarmos com a recuperacao de senha</p>
          <b-form-input
            id="input-1"
            v-model="tokenRecoveryPassword"
            type="text"
            required
            placeholder="Informe o Token"
          ></b-form-input>
        </b-form-group>
        <h4
          class="h4 text-center text-warning"
          v-if="senhaModificada === true"
        >Aviso: Sua senha sera alterada para: {{novaSenha}}</h4>
        <div class="d-flex justify-content-center">
          <b-button variant="success" @click.prevent="handleToken">Mudar a senha</b-button>
        </div>
      </section>
    </b-modal>
  </b-container>
</template>

<script>
//import axios from "axios";
import jwt from "jwt-decode";
import {api} from "@/services"
export default {
  name: "Login",
  data() {
    return {
      user: {
        email: "",
        senha: ""
      },
      tokenRecoveryPassword: null,
      emailRecuperacaoSenha: "",
      modalRecuperacaoSenha: false,
      senhaModificada: false,
      novaSenha: ""
    };
  },
  methods: {
    efetuarLogin() {     
        api
        .post("/api/Pessoa/Login", this.user)
            .then(res => {                
                sessionStorage.setItem("token", res.data[0]);
                this.$store.commit("SETAR_JOGOS", res.data[1])
                sessionStorage.setItem("Jogos", JSON.stringify(res.data[1]))
                this.$store.commit("MODIFICAR_PESSOA", res.data[2])
                sessionStorage.setItem("Pessoa", JSON.stringify(res.data[2]))
                this.$router.push({path: "/"})
            })
            .catch((err) => {                
                this.makeToast(
                        "danger",
                        "Erro ao Tentar Efetuar o Login",
                        `Verifique seus dados e tente novamente. Erro: ${err}`
                      );
            })
    },
    handleEsquecimentoSenha() {
      this.modalRecuperacaoSenha = true;
    },
    handleRecoveryPassword() {
        let obj = { email: this.emailRecuperacaoSenha }; 
        api
         .post("/api/Pessoa/recuperarSenha", obj)
            .then(() => {
                this.tokenRecoveryPassword = "";
            }).catch(err => {                
                this.makeToast(
                        "danger",
                        "Erro ao Tentar recuperar a Senha",
                        `Verifique seus dados e tente novamente. Erro: ${err}`
                      );
            })

    },
    handleToken() {     
      let obj = { token: this.tokenRecoveryPassword };
      let tokenDecoded = jwt(obj.token);
      if (
        tokenDecoded.exp < new Date().getTime() + 1 / 1000 &&
        tokenDecoded.email !== this.emailRecuperacaoSenha
      ) {
        this.makeToast(
                        "danger",
                        "Token Invalido",
                        `Token Encontra-se Expirado`
                      );
      } else { 
          api 
           .post("/api/pessoa/token", obj)
                .then(res => {
                    this.novaSenha = res.data;
                    this.senhaModificada = true;
                })
                .catch(err => {                    
                     this.makeToast(
                        "danger",
                        "Erro ao Tentar ao Modificar a senha",
                        `Verifique o token Fornecido e tente novamente. Erro: ${err}`
                      );
                })
      }
    },
      makeToast(variant = null, title, corpo) {
      this.$bvToast.toast(corpo, {
        title: `${title}`,
        variant: variant,
        solid: true
      });
    }
  },
  computed: {
    loginIsValido() {
      return (this.user.email.includes("@") &&
        this.user.email.includes(".com")) ||
        this.user.senha.length > 3
        ? true
        : false;
    }
  }
};
</script>