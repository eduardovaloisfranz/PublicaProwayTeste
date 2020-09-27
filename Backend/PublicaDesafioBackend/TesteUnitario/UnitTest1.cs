using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicaDesafioBackend.Models;
using PublicaDesafioBackend.Util;

namespace TesteUnitario
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestUserIsValid()
        {
            Pessoa pessoa = new Pessoa();
            pessoa.NomeCompleto = "";
            pessoa.Email = "";
            pessoa.Senha = "";
            bool resultado = Util.UserIsValid(pessoa);           
            Assert.IsFalse(resultado);
        }       
    }
}
