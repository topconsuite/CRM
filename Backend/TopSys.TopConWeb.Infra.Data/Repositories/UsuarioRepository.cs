using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using Dapper;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        private const string _produtoAcessoAplicacao = "TopConWeb";
        
        public UsuarioRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        //TODO: ANALISAR A QUESTÃO DOS SPECS
        public Usuario ObterPorIdSenha(string id, string senha)
        {
            var usuario = _context.Usuarios
                .Where(t => t.Id == id && t.Senha == senha)
                .FirstOrDefault();

            if (usuario == null) return null;

            IDictionary<string, string> direitos = ObterDireitosPorId(id);
            return new Usuario(usuario.Id, usuario.Senha, direitos);
        }

        public void CadastrarSenha(string id, string novaSenha)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("UPDATE usr_usuario SET");
            sqlComando.Append($" senha = '{novaSenha}'");
            sqlComando.Append($" WHERE id='{id}'");

            _context.Database.Connection.Execute(sqlComando.ToString());
        }

        public IDictionary<string, string> ObterDireitosPorId(string id)
        {
            // var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AppCnnStr"].ConnectionString;
            // MySqlConnection conn = new MySqlConnection(connectionString);
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT ");
            sqlComando.Append(" aplicacao,CAST(CONCAT(dir_inc,dir_alt,dir_exc,dir_rel) as char) direitos");
            sqlComando.Append(" FROM (");
            sqlComando.Append(" SELECT CAST(CONCAT(gu.aplicativo,LPAD(p.num,4,'0')) as char) aplicacao");
            sqlComando.Append(" ,IF(IFNULL(IFNULL(IFNULL(t.acesso,u.acesso),g.acesso),'N')='S','C','') acesso");
            sqlComando.Append(" ,IF(IFNULL(IFNULL(IFNULL(t.dir_inc,u.dir_inc),g.dir_inc),'N')='S','I','') dir_inc");
            sqlComando.Append(" ,IF(IFNULL(IFNULL(IFNULL(t.dir_inc,u.dir_alt),g.dir_alt),'N')='S','A','') dir_alt");
            sqlComando.Append(" ,IF(IFNULL(IFNULL(IFNULL(t.dir_inc,u.dir_exc),g.dir_exc),'N')='S','E','') dir_exc");
            sqlComando.Append(" ,IF(IFNULL(IFNULL(IFNULL(t.dir_inc,u.dir_rel),g.dir_rel),'N')='S','R','') dir_rel");
            sqlComando.Append(" FROM usr_dir_usuario gu");
            sqlComando.Append(" LEFT JOIN usr_programa p ON gu.Aplicativo=p.Aplicativo");
            sqlComando.Append(" LEFT JOIN usr_dir_temp_usr t ON gu.usuario=t.usuario AND gu.aplicativo=t.aplicativo AND p.num=t.num_prog");
            sqlComando.Append("  AND CURDATE()>=t.dt_ini_val AND CURDATE()<=t.dt_fim_val");
            sqlComando.Append(" LEFT JOIN usr_dir_grupou u ON gu.usuario=u.usuario AND gu.aplicativo=u.sigla AND p.num=u.num_prog");
            sqlComando.Append(" LEFT JOIN usr_dir_grupo g ON gu.grupo=g.nome_grupo AND gu.aplicativo=g.sigla AND p.num=g.num_prog");
            sqlComando.Append(" WHERE gu.usuario= @ID_USUARIO");
            sqlComando.Append(" HAVING acesso='C'");
            sqlComando.Append(" AND aplicacao IN(");
            sqlComando.Append("'WEB6998','WEB6118','WEB6127','COM6101','COM6124','COM6220','CON6157','WEB6309', 'WEB6310'");
            sqlComando.Append(",'WEB6156', 'CON6207','WEB6272','WEB6268','CON0028','CON0002','WEB6149','CON0012','CON6105'");
            sqlComando.Append(",'CON6980','CON6981','CON6982','CON6983','CON6984','CON6985','CON6986','CON6987','CON6295','CON7036'");
            sqlComando.Append(",'WEB0001','WEB0002','WEB6002','WEB6003','WEB6004','WEB6005', 'WEB6006', 'WEB6007', 'WEB6101', 'WEB6102', 'WEB6103', 'WEB6104', 'WEB6105', 'WEB6106', 'WEB6107', 'WEB6108', 'WEB6109','WEB6201','WEB6301','WEB6001'");
            sqlComando.Append(",'WEB6999','WEB7000','WEB7001', 'CON6147', 'CON0036', 'WEB6008', 'WEB6009', 'WEB6010', 'WEB6110', 'WEB7002', 'WEB7003', 'WEB7004', 'WEB7005', 'WEB7006', 'WEB7007', 'WEB7008', 'WEB7009', 'WEB7010', 'WEB7011', 'WEB7012'");
            sqlComando.Append(")");
            sqlComando.Append(") a");

            var aplicativos = _context.Database.Connection.Query(sqlComando.ToString(), new { ID_USUARIO = id }).ToList();
            var result = new Dictionary<string, string>();

            foreach(var aplicacao in aplicativos)
            {
                
                var aplicacaoNome = (string)aplicacao.aplicacao;
                var direito = (string)aplicacao.direitos;

                var alreadyExists = result.ContainsKey(aplicacaoNome);

                if (alreadyExists)
                    result[aplicacaoNome] = $"{result[aplicacaoNome]}{direito}";
                else
                    result.Add(aplicacaoNome, direito);

            }

            return result;
        }

        public IDictionary<string, string> ObterClaimsVendedores(Usuario usuario)
        {
            //bool _hasDireitoPorGrupo = usuario.Direitos["WEB0001"].Equals("A");
            //bool _hasDireitoGeral = usuario.Direitos["WEB0002"].Equals("A");
            string _direitoPorGrupo;
            string _direitoGeral;
            bool _hasDireitoAcessoPorGrupo = usuario.Direitos.TryGetValue("WEB0001", out _direitoPorGrupo);
            bool _hasDireitoAcessoGeral = usuario.Direitos.TryGetValue("WEB0002", out _direitoGeral);

            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("(SELECT");
            sqlComando.Append(" 'vendedoresVinculados' chave");
            sqlComando.Append(", CONVERT(group_concat(cod) USING latin1) vendedores");
            sqlComando.Append(" FROM con_vendedor");
            sqlComando.Append(" WHERE usuario_sist=@ID_USUARIO)");

            if (_hasDireitoAcessoGeral)
            {
                sqlComando.Append(" UNION (SELECT 'vendedoresPermitidos' chave,'*' vendedores)");
            }
            else if (_hasDireitoAcessoPorGrupo)
            {
                sqlComando.Append(" UNION (SELECT");
                sqlComando.Append(" 'vendedoresPermitidos' chave");
                sqlComando.Append(", CONVERT(group_concat(cod) USING latin1) vendedores");
                sqlComando.Append(" FROM con_vendedor");
                sqlComando.Append(" WHERE usuario_sist IN(");
                sqlComando.Append(" SELECT u.Usuario");
                sqlComando.Append(" FROM usr_dir_usuario g");
                sqlComando.Append(" INNER JOIN usr_dir_usuario u");
                sqlComando.Append(" ON g.Grupo=u.Grupo");
                sqlComando.Append(" AND g.Aplicativo=u.Aplicativo");
                sqlComando.Append(" WHERE g.Usuario=@ID_USUARIO");
                sqlComando.Append(" AND g.Aplicativo='WEB'");
                sqlComando.Append("))");
            }
            else
            {
                sqlComando.Append(" UNION (SELECT");
                sqlComando.Append(" 'vendedoresPermitidos' chave");
                sqlComando.Append(", CONVERT(group_concat(cod) USING latin1) vendedores");
                sqlComando.Append(" FROM con_vendedor");
                sqlComando.Append(" WHERE usuario_sist=@ID_USUARIO)");
            }
            
            var result = _context.Database.Connection.Query(sqlComando.ToString(), new { ID_USUARIO = usuario.Id })
                .ToDictionary(row => (string)row.chave, row => (string)row.vendedores ?? "0");
            return result;
        }

        public float? ObterPercentualDescontoLimitePorId(string id)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append("SELECT");
            sqlComando.Append(" pct_desc");
            sqlComando.Append(" FROM topsys.con_desc_usuario");
            sqlComando.Append(" WHERE");
            sqlComando.Append(" usuario=@ID_USUARIO");

            var result = _context.Database.Connection.Query<float?>(sqlComando.ToString(), new { ID_USUARIO = id })
                .FirstOrDefault();
            
            return result;
        }

        public void GravaAcessoAplicacao(AcessoAplicacao acessoAplicacao)
        {
            StringBuilder sqlComando = new StringBuilder();

            sqlComando.Append($"INSERT INTO topsys.usr_acesso_aplicacao ");
            sqlComando.Append($" SET produto='{ _produtoAcessoAplicacao}',");
            sqlComando.Append($" cliente_codigo=@{nameof(AcessoAplicacao.ClienteCodigo)},");
            sqlComando.Append($" cliente_nome=@{nameof(AcessoAplicacao.ClienteNome)},");
            sqlComando.Append($" usuario=@{nameof(AcessoAplicacao.UsuarioId)},");
            sqlComando.Append($" email=@{nameof(AcessoAplicacao.UsuarioEmail)},");
            sqlComando.Append($" local='Matriz',");
            sqlComando.Append($" banco_ip=@@hostname,");
            sqlComando.Append($" banco_porta=@@port,");
            sqlComando.Append($" aplicativo=@{nameof(AcessoAplicacao.Aplicativo)},");
            sqlComando.Append($" codigo_programa=@{nameof(AcessoAplicacao.Programa)},");
            sqlComando.Append($" data=CURDATE(),");
            sqlComando.Append($" hora_primeiro_acesso=CURTIME(),");
            sqlComando.Append($" hora_ultimo_acesso=CURTIME(),");
            sqlComando.Append($" quantidade_acessos=1");
            sqlComando.Append($" ON DUPLICATE KEY UPDATE");
            sqlComando.Append($" cliente_nome=@{nameof(AcessoAplicacao.ClienteNome)},");
            sqlComando.Append($" email=@{nameof(AcessoAplicacao.UsuarioEmail)},");
            sqlComando.Append($" local='Matriz',");
            sqlComando.Append($" hora_ultimo_acesso=CURTIME(),");
            sqlComando.Append($" quantidade_acessos=quantidade_acessos+1;");

            _context.Database.Connection.Execute(sqlComando.ToString(), acessoAplicacao);
        }

        public Dictionary<string, string> ListarUsuariosComGrupos()
        {

            var sql = new StringBuilder();

            sql.AppendLine("SELECT DISTINCT(Usuario) Usuario, grupo Grupo");
            sql.AppendLine("FROM usr_dir_usuario WHERE ");
            sql.AppendLine("Aplicativo = 'WEB'");

            var usuarios = _context.Connection.Query(sql.ToString());
            var resultado = new Dictionary<string, string>();

            usuarios.ToList().ForEach((usuario) =>
            {
                resultado.Add((string)usuario.Usuario, (string)usuario.Grupo);
            });

            return resultado;
        }

    }
}
