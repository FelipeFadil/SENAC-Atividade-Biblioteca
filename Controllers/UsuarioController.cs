using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult ListaDeUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View(new UsuarioService().Listar());
        }

        public IActionResult EditarUsuario(int id)
        {
            Usuario u = new UsuarioService().Listar(id);

            return View(u);
        }

        [HttpPost]
        public IActionResult EditarUsuario(Usuario userEditado)
        {
            UsuarioService us = new UsuarioService();
            us.EditarUsuario(userEditado);

            return RedirectToAction("ListaDeUsuarios");
        }

        public IActionResult RegistrarUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarUsuarios(Usuario novoUser)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            novoUser.Senha = Criptografo.TextoCriptografado(novoUser.Senha);

            UsuarioService us = new UsuarioService();
            us.IncluirUsuario(novoUser);

            return RedirectToAction("CadastroRealizado");
        }

        public IActionResult ExcluirUsuario(int id)
        {
            return View(new UsuarioService().Listar(id));
        }

        [HttpPost]
        public IActionResult ExcluirUsuario(string decisao, int id)
        {
            if(decisao=="EXCLUIR")
            {
                ViewData["Mensagem"] = "Exclusão do usuário "+new UsuarioService().Listar(id).Nome+" realizada com sucesso.";
                new UsuarioService().ExcluirUsuario(id);
                return View("ListaDeUsuarios", new UsuarioService().Listar());
            }
            else
            {
                ViewData["Mensagem"] = "Exclusão cancelada.";
                return View("ListaDeUsuarios", new UsuarioService().Listar());
            }
        }

        public IActionResult CadastroRealizado()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View();
        }

        public IActionResult NeedAdmin()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}