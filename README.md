# Publica Proway Desafio

Banco de Dados: MySql.

Frontend: VueJS.

Backend: C# .Net Core 3.1.

Como utilizar a aplicação:
===Versão para compilar no Visual Studio 2019=====
1) Instale o Xampp na sua máquina e iniciar o processo MySQL
2) Abra o projeto no Visual Studio 2019 
3) Abrir console do Nuget e digitar os seguintes comandos
4) add-migration inicio
5) update-database
assim criará o Banco de Dados e suas tabelas.
Backend ja é funcional.
6) Compile o Projeto para estar disponível os EndPoints da Aplicação
7) Verifique o arquivo services.js que se encontra /Frontend/publica-desafio/src/services.js
a baseURL é localhost:44372 conforme print.
![image](https://i.gyazo.com/e1d3de9e17f4695eed0b7b3f3fd162a5.png)
=======
Versão para usar .Net Core CLI
Instale o Xampp na sua máquina e iniciar o processo MySQL

vá a pasta PublicaProwayTeste\Backend\PublicaDesafioBackend\PublicaDesafioBackend

dentro dela digite estes comandos na linha de comando.
1) dotnet tool install --global dotnet-ef

2) dotnet ef migrations add Inicio

3) dotnet ef database update

3)dotnet run --project PublicaDesafioBackend.csproj

aparecerá uma mensagem de que aplicação estará rodando na porta 5000.

2)add-migration inicio

3)update-database 

e então modificar o arquivo services.js que se encontra /Frontend/publica-desafio/src/services.js
alterar o localhost:44372 para 5001

abra uma aba no navegador no localhost:5000 e confira se foi exibido a documentação gerada pelo Swagger
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

