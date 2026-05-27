using System.Globalization;
using System.Resources;

namespace Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages
{
    /// <summary>
    /// Enumeração para os recursos de equipamento.
    /// </summary>
    public enum EResourcesEquipamento
    {
        /// <summary>
        /// 9.2.1 Invalid equipment type
        /// </summary>
        EQUIPAMENTO_ERROR_TCON393231,

        /// <summary>
        /// 9.2.2 No records found
        /// </summary>
        EQUIPAMENTO_ERROR_TCON393232,

        /// <summary>
        /// 9.2.3 Code provided is not valid.
        /// </summary>
        EQUIPAMENTO_ERROR_TCON393233,

        /// <summary>
        /// 9.2.4 Description can not be blank
        /// </summary>
        EQUIPAMENTO_ERROR_TCON393234,

        /// <summary>
        /// 9.2.5 State (UF) invalid
        /// </summary>
        EQUIPAMENTO_ERROR_TCON393235,

        /// <summary>
        /// 9.2.6 Code in equipments of type concrete mixer need to start with prefix '{0}'
        /// </summary>
        EQUIPAMENTO_ERROR_TCON393236,

        /// <summary>
        /// 9.2.7 {field} hour meter field accept only ‘S’ (Yes) or ‘N’ (No)
        /// </summary>
        EQUIPAMENTO_ERROR_TCON393237,

        /// <summary>
        /// 9.2.8 License plate invalid
        /// </summary>
        EQUIPAMENTO_ERROR_TCON393238,

        /// <summary>
        /// 9.2.9 Code provided is already registered
        /// </summary>
        EQUIPAMENTO_ERROR_TCON393239,

        /// <summary>
        /// 9.2.10 License plate provided is already registered
        /// </summary>
        EQUIPAMENTO_ERROR_TCON39323130,

        /// <summary>
        /// 9.2.11 Body type invalid.
        /// </summary>
        EQUIPAMENTO_ERROR_TCON39323131,

        /// <summary>
        /// 9.2.12 Wheelset type invalid.
        /// </summary>
        EQUIPAMENTO_ERROR_TCON39323132,

        /// <summary>
        /// 9.2.13 Owner type invalid.
        /// </summary>
        EQUIPAMENTO_ERROR_TCON39323133,

        /// <summary>
        /// 9.2.14 It is not possible to delete equipment that has already been used
        /// </summary>
        EQUIPAMENTO_ERROR_TCON39323134,

        /// <summary>
        /// 9.2.15 Concrete batching plant invalid.
        /// </summary>
        EQUIPAMENTO_ERROR_TCON39323135,

        /// <summary>
        /// 9.2.16 Allocated employee invalid.
        /// </summary>
        EQUIPAMENTO_ERROR_TCON39323136,

        /// <summary>
        /// 9.2.17 External id provided is already registered
        /// </summary>
        EQUIPAMENTO_ERROR_TCON39323137
    }


    public static class EResourcesEquipamentoExtensions
    {
        public static string GetResourceMessage(this EResourcesEquipamento erro, CultureInfo idioma, params object[] parametros)
        {
            var rm = new ResourceManager(typeof(Resources));

            var mensagem = rm.GetString(erro.ToString(), idioma);

            return string.Format(mensagem, parametros);
        }

        public static string GetMessageCode(this EResourcesEquipamento error)
        {
            switch (error)
            {
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393231:
                    return "TCON393231";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393232:
                    return "TCON393232";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393233:
                    return "TCON393233";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393234:
                    return "TCON393234";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393235:
                    return "TCON393235";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393236:
                    return "TCON393236";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393237:
                    return "TCON393237";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393238:
                    return "TCON393238";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON393239:
                    return "TCON393239";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323130:
                    return "TCON39323130";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323131:
                    return "TCON39323131";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323132:
                    return "TCON39323132";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323133:
                    return "TCON39323133";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323134:
                    return "TCON39323134";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323135:
                    return "TCON39323135";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323136:
                    return "TCON39323136";
                case EResourcesEquipamento.EQUIPAMENTO_ERROR_TCON39323137:
                    return "TCON39323137";
                default:
                    return "";
            }
        }
    }
}
