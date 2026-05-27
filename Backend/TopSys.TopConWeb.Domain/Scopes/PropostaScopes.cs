using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.SharedKernel.Validation;

namespace TopSys.TopConWeb.Domain.Scopes
{
    public static class PropostaScopes
    {
        public static bool AdicionarPropostaScopeIsValid(this Proposta proposta)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(proposta, "proposta", "Obrigatório informar o objeto proposta!"),
                AssertionConcern.AssertNotNull(proposta.Obra, "obra", "Obrigatório informar o objeto proposta.obra!"),
                AssertionConcern.AssertIsGreaterThan(proposta.VendedorCodigo ?? 0, 0, "vendedor", "Obrigatório informar o objeto proposta.vendedor!")
            );
        }

        public static bool AdicionarPropostaScopeIsValid(this PropostaVersao proposta)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(proposta, "proposta", "Obrigatório informar o objeto proposta!"),
                AssertionConcern.AssertNotNull(proposta.Obra, "obra", "Obrigatório informar o objeto proposta.obra!"),
                AssertionConcern.AssertIsGreaterThan(proposta.VendedorCodigo ?? 0, 0, "vendedor", "Obrigatório informar o objeto proposta.vendedor!")
            );
        }

        public static bool PropostaVendedorPermitidoScopeIsValid(this Proposta proposta, Expression<Func<IHasVendedor, bool>> filtroVendedores)
        {
            var lista = new List<Proposta>() { proposta }.AsQueryable();
            var propostaComVendedorValido = lista.Where(filtroVendedores).FirstOrDefault();

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(propostaComVendedorValido, "vendedor", "Vendedor não permitido para esse usuário!")
            );
        }

        public static bool PropostaVendedorPermitidoScopeIsValid(this PropostaVersao proposta, Expression<Func<IHasVendedor, bool>> filtroVendedores)
        {
            var lista = new List<PropostaVersao>() { proposta }.AsQueryable();
            var propostaComVendedorValido = lista.Where(filtroVendedores).FirstOrDefault();

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertNotNull(propostaComVendedorValido, "vendedor", "Vendedor não permitido para esse usuário!")
            );
        }

        public static bool PropostaVendedorExclusivoScopeIsValid(this Proposta proposta, Interveniente cliente)
        {
            if (cliente == null) return true;

            return cliente.VendedorCodigo == 0 || proposta.VendedorCodigo == cliente.VendedorCodigo;
        }

        public static bool PropostaVendedorExclusivoScopeIsValid(this PropostaVersao proposta, Interveniente cliente)
        {
            if (cliente == null) return true;

            return cliente.VendedorCodigo == 0 || proposta.VendedorCodigo == cliente.VendedorCodigo;
        }

        public static bool PropostaContratoIntervenienteIsValid(this Proposta proposta, Contrato contrato)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(proposta.IntervenienteCodigo ?? 0, contrato.IntervenienteCodigo ?? 0, "interveniente", "Não é permitido alterar cliente pois já existe contrato!")
            );
        }

        public static bool PropostaContratoIntervenienteIsValid(this PropostaVersao proposta, ContratoVersao contrato)
        {
            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertAreEquals(proposta.IntervenienteCodigo ?? 0, contrato.IntervenienteCodigo ?? 0, "interveniente", "Não é permitido alterar cliente pois já existe contrato!")
            );
        }

        public static bool PropostaInscricaoEstadualIsValid(this Proposta proposta, IIntervenienteService intervenienteService)
        {
            var ieClienteIsValid = true;
            if (proposta.IntervenienteTipo != "F")
            {
                ieClienteIsValid = intervenienteService.InscricaoEstadualEhValida(proposta.InscricaoEstadual, "");
            }

            var ieResponsavelSolidarioIsValid = true;
            if (proposta.UtilizaResponsavelSolidario && proposta.ResponsavelSolidario.IntervenienteTipo != "F")
            {
                ieResponsavelSolidarioIsValid = intervenienteService.InscricaoEstadualEhValida(proposta.ResponsavelSolidario.InscricaoEstadual, "");
            }

            var ieDadosFaturamentoIsValid = true;
            if (proposta.UtilizaDadosFaturamento && proposta.Faturamento.IntervenienteTipo != "F")
            {
                ieDadosFaturamentoIsValid = intervenienteService.InscricaoEstadualEhValida(proposta.Faturamento.InscricaoEstadual, "");
            }

            var ieDadosCobrancaIsValid = true;
            if (proposta.UtilizaDadosCobranca && proposta.Cobranca.IntervenienteTipo != "F")
            {
                ieDadosCobrancaIsValid = intervenienteService.InscricaoEstadualEhValida(proposta.Cobranca.InscricaoEstadual, "");
            }

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(ieClienteIsValid, "inscricaoEstadual", "Inscrição estadual inválida!"),
                AssertionConcern.AssertTrue(ieDadosFaturamentoIsValid, "dadosFaturamentoInscricaoEstadual", "Inscrição estadual inválida!"),
                AssertionConcern.AssertTrue(ieDadosFaturamentoIsValid, "dadosCobrançaInscricaoEstadual", "Inscrição estadual inválida!"),
                AssertionConcern.AssertTrue(ieDadosFaturamentoIsValid, "dadosResponsavelSolidarioInscricaoEstadual", "Inscrição estadual inválida!")
            );
        }

        public static bool PropostaInscricaoEstadualIsValid(this PropostaVersao proposta, IIntervenienteService intervenienteService)
        {
            var ieClienteIsValid = true;
            if (proposta.IntervenienteTipo != "F")
            {
                ieClienteIsValid = intervenienteService.InscricaoEstadualEhValida(proposta.InscricaoEstadual, "");
            }

            var ieResponsavelSolidarioIsValid = true;
            if (proposta.UtilizaResponsavelSolidario && proposta.ResponsavelSolidario.IntervenienteTipo != "F")
            {
                ieResponsavelSolidarioIsValid = intervenienteService.InscricaoEstadualEhValida(proposta.ResponsavelSolidario.InscricaoEstadual, "");
            }

            var ieDadosFaturamentoIsValid = true;
            if (proposta.UtilizaDadosFaturamento && proposta.Faturamento.IntervenienteTipo != "F")
            {
                ieDadosFaturamentoIsValid = intervenienteService.InscricaoEstadualEhValida(proposta.Faturamento.InscricaoEstadual, "");
            }

            var ieDadosCobrancaIsValid = true;
            if (proposta.UtilizaDadosCobranca && proposta.Cobranca.IntervenienteTipo != "F")
            {
                ieDadosCobrancaIsValid = intervenienteService.InscricaoEstadualEhValida(proposta.Cobranca.InscricaoEstadual, "");
            }

            return AssertionConcern.IsSatisfiedBy
            (
                AssertionConcern.AssertTrue(ieClienteIsValid, "inscricaoEstadual", "Inscrição estadual inválida!"),
                AssertionConcern.AssertTrue(ieDadosFaturamentoIsValid, "dadosFaturamentoInscricaoEstadual", "Inscrição estadual inválida!"),
                AssertionConcern.AssertTrue(ieDadosFaturamentoIsValid, "dadosCobrançaInscricaoEstadual", "Inscrição estadual inválida!"),
                AssertionConcern.AssertTrue(ieDadosFaturamentoIsValid, "dadosResponsavelSolidarioInscricaoEstadual", "Inscrição estadual inválida!")
            );
        }

        public static bool PropostaResponsavelSolidarioIsValid(this Proposta proposta, PropostaResponsavelSolidario responsavelSolidario)
        {
            var responsavelSolidarioIsValid = true;
            
            if (proposta.CpfCnpj.Equals(responsavelSolidario.CpfCnpj))
                    responsavelSolidarioIsValid = false;


            return AssertionConcern.IsSatisfiedBy
           (
               AssertionConcern.AssertTrue(responsavelSolidarioIsValid, "responsavelSolidario", "Responsável Solidário com mesmo dados do cliente!")
           );
        }

        public static bool PropostaResponsavelSolidarioIsValid(this PropostaVersao proposta, PropostaResponsavelSolidarioVersao responsavelSolidario)
        {
            var responsavelSolidarioIsValid = true;

            if (proposta.CpfCnpj.Equals(responsavelSolidario.CpfCnpj))
                responsavelSolidarioIsValid = false;


            return AssertionConcern.IsSatisfiedBy
           (
               AssertionConcern.AssertTrue(responsavelSolidarioIsValid, "responsavelSolidario", "Responsável Solidário com mesmo dados do cliente!")
           );
        }

        public static bool PropostaCobrancaIsValid(this Proposta proposta, PropostaCobranca propostaCobranca, bool utilizaDadosCobranca, bool utilizaEnderecoCobranca)
        {
            var cobracaEnderecoIsValid = true;
            var cobracaDadosIsValid = true;
            

            if (utilizaDadosCobranca)
            {
                if (proposta.CpfCnpj.Equals(propostaCobranca.CpfCnpj))
                    cobracaDadosIsValid = false;
            }

            if (utilizaEnderecoCobranca)
            {
                var propostaEndereco = $"{proposta.EnderecoCep}{proposta.EnderecoLogradouro}{proposta.EnderecoNumero}{proposta.EnderecoComplemento}";
                var cobrancaEnderecoEndereco = $"{propostaCobranca.EnderecoCep}{propostaCobranca.EnderecoLogradouro}{propostaCobranca.EnderecoNumero}{propostaCobranca.EnderecoComplemento}";

                if (propostaEndereco.Equals(cobrancaEnderecoEndereco))
                    cobracaEnderecoIsValid = false;
            }
            
            return AssertionConcern.IsSatisfiedBy
           (
               AssertionConcern.AssertTrue(cobracaEnderecoIsValid, "enderecoCobranca", "Endereço de cobrança é o mesmo do cliente!"),
               AssertionConcern.AssertTrue(cobracaDadosIsValid, "dadosCobranca", "O CPF/CNPJ de cobrança é o mesmo do cliente!")
           );
        }

        public static bool PropostaCobrancaIsValid(this PropostaVersao proposta, PropostaCobrancaVersao propostaCobranca, bool utilizaDadosCobranca, bool utilizaEnderecoCobranca)
        {
            var cobracaEnderecoIsValid = true;
            var cobracaDadosIsValid = true;


            if (utilizaDadosCobranca)
            {
                if (proposta.CpfCnpj.Equals(propostaCobranca.CpfCnpj))
                    cobracaDadosIsValid = false;
            }

            if (utilizaEnderecoCobranca)
            {
                var propostaEndereco = $"{proposta.EnderecoCep}{proposta.EnderecoLogradouro}{proposta.EnderecoNumero}{proposta.EnderecoComplemento}";
                var cobrancaEnderecoEndereco = $"{propostaCobranca.EnderecoCep}{propostaCobranca.EnderecoLogradouro}{propostaCobranca.EnderecoNumero}{propostaCobranca.EnderecoComplemento}";

                if (propostaEndereco.Equals(cobrancaEnderecoEndereco))
                    cobracaEnderecoIsValid = false;
            }

            return AssertionConcern.IsSatisfiedBy
           (
               AssertionConcern.AssertTrue(cobracaEnderecoIsValid, "enderecoCobranca", "Endereço de cobrança é o mesmo do cliente!"),
               AssertionConcern.AssertTrue(cobracaDadosIsValid, "dadosCobranca", "O CPF/CNPJ de cobrança é o mesmo do cliente!")
           );
        }

        public static bool PropostaFaturamentoIsValid(this Proposta proposta, PropostaFaturamento propostaFaturamento, bool utilizaDadosFaturamento, bool utilizaEnderecoFaturamento)
        {
            var faturamentoEnderecoIsValid = true;
            var faturamentoDadosIsValid = true;
            

            if (utilizaDadosFaturamento)
            {
                if (proposta.CpfCnpj.Equals(propostaFaturamento.CpfCnpj))
                    faturamentoDadosIsValid = false;
            }

            if (utilizaEnderecoFaturamento)
            {
                var propostaEndereco = $"{proposta.EnderecoCep}{proposta.EnderecoLogradouro}{proposta.EnderecoNumero}{proposta.EnderecoComplemento}";
                var faturamentoEnderecoEndereco = $"{propostaFaturamento.EnderecoCep}{propostaFaturamento.EnderecoLogradouro}{propostaFaturamento.EnderecoNumero}{propostaFaturamento.EnderecoComplemento}";

                if (propostaEndereco.Equals(faturamentoEnderecoEndereco))
                    faturamentoEnderecoIsValid = false;
            }

            return AssertionConcern.IsSatisfiedBy
           (
               AssertionConcern.AssertTrue(faturamentoEnderecoIsValid, "enderecoFaturamento", "Endereço de faturamento é o mesmo do cliente!"),
               AssertionConcern.AssertTrue(faturamentoDadosIsValid, "dadosCobranca", "O CPF/CNPJ do faturamento é o mesmo do cliente!")
           );
        }

        public static bool PropostaFaturamentoIsValid(this PropostaVersao proposta, PropostaFaturamentoVersao propostaFaturamento, bool utilizaDadosFaturamento, bool utilizaEnderecoFaturamento)
        {
            var faturamentoEnderecoIsValid = true;
            var faturamentoDadosIsValid = true;


            if (utilizaDadosFaturamento)
            {
                if (proposta.CpfCnpj.Equals(propostaFaturamento.CpfCnpj))
                    faturamentoDadosIsValid = false;
            }

            if (utilizaEnderecoFaturamento)
            {
                var propostaEndereco = $"{proposta.EnderecoCep}{proposta.EnderecoLogradouro}{proposta.EnderecoNumero}{proposta.EnderecoComplemento}";
                var faturamentoEnderecoEndereco = $"{propostaFaturamento.EnderecoCep}{propostaFaturamento.EnderecoLogradouro}{propostaFaturamento.EnderecoNumero}{propostaFaturamento.EnderecoComplemento}";

                if (propostaEndereco.Equals(faturamentoEnderecoEndereco))
                    faturamentoEnderecoIsValid = false;
            }

            return AssertionConcern.IsSatisfiedBy
           (
               AssertionConcern.AssertTrue(faturamentoEnderecoIsValid, "enderecoFaturamento", "Endereço de faturamento é o mesmo do cliente!"),
               AssertionConcern.AssertTrue(faturamentoDadosIsValid, "dadosCobranca", "O CPF/CNPJ do faturamento é o mesmo do cliente!")
           );
        }
    }
}
