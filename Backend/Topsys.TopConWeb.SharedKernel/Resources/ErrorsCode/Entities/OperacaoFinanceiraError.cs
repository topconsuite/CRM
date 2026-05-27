namespace Topsys.TopConWeb.SharedKernel.Resources.ErrorsCode.Entities
{
    public class OperacaoFinanceiraError
    {
        //                                              HEXADECIMAL -> STRING
        public static string ERROR_TCON375f385f31 = "TCON375f385f31"; // 7.8.1 The <T> field must be 0 if the update_bank field is not equal to 9
        public static string ERROR_TCON375f385f32 = "TCON375f385f32"; // 7.8.2 The sell_operation field must references a FinanceOperation that has inclusion_discharge = B 
        public static string ERROR_TCON375f385f33 = "TCON375f385f33"; // 7.8.3 The bank_movement_operation field must reference a FinanceOperation that has inclusion_discharge = B and subsystem = <X>
    }
}