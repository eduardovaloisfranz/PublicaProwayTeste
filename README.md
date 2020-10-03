# Publica Proway Desafio

Banco de Dados: MySql.

Frontend: VueJS.

Backend: C# .Net Core 3.1.

Segue o tutorial para rodar a aplicação Direto do Visual Studio 2019 ou por Linha de Comando. Entretanto existe um passo que deve ser seguido em ambas versões
Na pasta Classe Util dentro da Pasta Util. é necessário editar a linha 96 e inserir a ApiKey que estará no email enviado e embaixo das respostas da questões teoricas. se eu colocar direto no projeto o sendgrid detecta a publicação irregular da key e deixa fora do ar o sistema de envio de emails.
![Image](https://i.gyazo.com/86ea84a6f8e1128ad73d1910dc253b9c.png)
Baixe o Xampp e execute o Processo MySQL
![Image](https://i.gyazo.com/dd805a8425e338247eeb325c12c4ad59.png)

Como utilizar a aplicação:
===Versão para compilar no Visual Studio 2019=====
1) Abra o projeto no Visual Studio 2019 
2) Abrir console do Nuget e digitar os seguintes comandos
3) add-migration inicio
4) update-database
![Image](https://i.gyazo.com/5d067b71795d328fa47ac13e562f9dd0.png)
assim criará o Banco de Dados e suas tabelas.
Backend ja é funcional.
5) Compile o Projeto para estar disponível os EndPoints da Aplicação
6) Verifique o arquivo services.js que se encontra /Frontend/publica-desafio/src/services.js
a baseURL é localhost:44372 conforme print.
![Image](https://i.gyazo.com/e1d3de9e17f4695eed0b7b3f3fd162a5.png)
=======
Versão para usar .Net Core CLI
vá a pasta PublicaProwayTeste\Backend\PublicaDesafioBackend\PublicaDesafioBackend

dentro dela digite estes comandos na linha de comando.
1) dotnet tool install --global dotnet-ef

2) dotnet ef migrations add Inicio

3) dotnet ef database update

4)dotnet run --project PublicaDesafioBackend.csproj

aparecerá uma mensagem de que aplicação estará rodando na porta 5001.

abra uma aba no navegador no localhost:5001 e confira se foi exibido a documentação gerada pelo Swagger
![Image](https://i.gyazo.com/5e8a1d0a4b619b6e2dd1e4734cdf85a7.png)

e então modificar o arquivo services.js que se encontra /Frontend/publica-desafio/src/services.js
alterar o localhost:44372 para 5001
![Image](https://i.gyazo.com/f6832c6e43411d7b7949789dcc98db9e.png)

para o frontend siga estas etapas:

1) Abra o CMD e vá até a pasta PublicaProwayTeste\Frontend\publica-desafio
2) npm install
3) npm run serve
e a aplicação estara disponivel no localhost:8080
![Image](https://i.gyazo.com/541ee32c57c8b47f24a5c436ad3894fa.png)
![Image](https://i.gyazo.com/c872e3b99b58bff07543c352b7165866.png)

Como utilizar a aplicação apos backend estar disponível e o frontend estar rodando na porta 8080.
1) Efetue seu cadastro em http://localhost:8080/cadastro
![Image](https://i.gyazo.com/e25afed27e94528be61c3d82a3536613.png)
2) Efetue Login em http://localhost:8080/login e será então redicionado para a Index
![Image](https://i.gyazo.com/210ba43aa5b44d30e2b1da320e2c58cf.png)
3) Na index terá a acesso a cadastrar novos jogos e editar seu perfil.
![Image](https://i.gyazo.com/9df06b33758e44042a60148dffe7b7fe.png)
![Image](https://i.gyazo.com/ec7b73c37edd75c3245672a8bba3ec3a.png)
4) Para apagar um Jogo clique nele na tabela e aparecerá um modal com detalhes e a opção de apagar.
![Image](https://i.gyazo.com/61cda6aa43f4c9b404465f4b54b9070d.png)

Como recuperar a Senha.
1) Clique na mensagem de Esqueci minha senha e Informe seu Email.
![Image](https://i.gyazo.com/8e735134bb4d996852a7bdfc6b0c5b38.png)
2) Vá no seu email e verifique se recebeu um email, com Titulo Recuperação de senha, caso nao tenha recebido cheque a caixa de Spam. Copie o codigo em Negrito
![Image](https://i.gyazo.com/73c790d97b1be5ba2bc6ce7d7dcd93d8.png)
3) Informe este codigo Copiado e cole no input que solicita o Token e clique em Mudar a Senha
![Image](https://i.gyazo.com/97ef4c58f4ac068714b6e0a6c7158c7f.png)
4) Você receberá uma nova Senha totalmente aleatoria para efetuar o seu login, os campos de input de email e senha ja serão setados com o email fornecido para verificação de senha e a nova senha, assim feche o modal e efetue o Login com a nova senha.
![Image](https://i.gyazo.com/7ed8e57c50a73db204b470c958222d55.png)


