<template>
  <b-row class="pt-3 justify-content-center align-items-center">
    <b-col cols="12" sm="8" md="4" lg="4">
      <h2 class="h2 text-center text-monospace">Bem vindo, registre-se</h2>
      <b-form>
        <b-form-group>
          <b-form-input
            id="input-nome"
            v-model="user.nomeCompleto"
            type="text"
            required
            placeholder="Informe seu nome completo"
          ></b-form-input>
        </b-form-group>     
        <b-form-group>
          <b-form-input
            id="input-email"
            v-model="user.email"
            type="email"
            required
            placeholder="Informe o email"
          ></b-form-input>
        </b-form-group>        
        <b-form-group>
          <b-form-input
            id="input-senha"
            v-model="user.senha"
            type="password"
            required
            placeholder="Informe sua senha"
          ></b-form-input>
        </b-form-group>
        <div class="d-flex justify-content-between">
          <b-button
            :disabled="validarLogin"
            variant="success"
            @click.prevent="registrar"
          >Registrar-se</b-button>
          <b-button variant="success" @click.prevent="resetDados">Limpar dados</b-button>
        </div>
      </b-form>
    </b-col>
  </b-row>
</template>

<script>
import { api } from "@/services";
export default {
  data() {
    return {
      user: {
        nomeCompleto: "",        
        email: "",
        senha: "",       
      }
    };
  },
  methods: {
    registrar() {      
      api
        .post("/api/Pessoa/CriarConta", this.user)
        .then(() => {
          this.$router.push("/login");
        })
        .catch(err => console.log("Erro ao efetuar o Cadastro" + err));
    },
    resetDados() {
      this.user.nomeCompleto = "";     
      this.user.email = "";
      this.user.senha = "";     
    },
    nomeIsValido() {
      if (this.user.nomeCompleto.length < 3 || this.user.nomeCompleto.length > 50) {
        return false;
      } else {
        return true;
      }
    },    
    emailIsValido() {
      if (this.user.email.length < 5 || this.user.email.length > 50) {
        return false;
      } else {
        return true;
      }

      },
      senhaIsValida(){
        if(this.user.senha.length < 3) {
          return false;
        }else{
          return true;
        }
    },    
  },
  computed: {
    validarLogin() {
      if (
        !this.nomeIsValido() ||         
        !this.emailIsValido() ||
        !this.senhaIsValida()
        
      ) {        
        return true;
      } else {
        return false;
      }
    }
  }
};
</script>