namespace Topsys.TopConWeb.SharedKernel.Resources.ErrorsCode
{
    public static class ProgramacaoError
    {
        //                                       HEXADECIMAL -> STRING
        public static string ERROR_TCON393631 = "TCON393631"; // 9.6.1 No records found
        public static string ERROR_TCON393632 = "TCON393632"; // 9.6.2 Concrete batching plant not found
        public static string ERROR_TCON393633 = "TCON393633"; // 9.6.3 Delivery concrete batching plant not found
        public static string ERROR_TCON393634 = "TCON393634"; // 9.6.4 Construction site city not found
        public static string ERROR_TCON393635 = "TCON393635"; // 9.6.5 Gravel not found
        public static string ERROR_TCON393636 = "TCON393636"; // 9.6.6 Product group not found
        public static string ERROR_TCON393637 = "TCON393637"; // 9.6.7 Pump code not found
        public static string ERROR_TCON393638 = "TCON393638"; // 9.6.8 Resistance type not found
        public static string ERROR_TCON393639 = "TCON393639"; // 9.6.9 Slump not found
        public static string ERROR_TCON39363130 = "TCON39363130"; // 9.6.10 Not found the contract for the combination of concrete_batching_plant_contract + contract_number + contract_year
        public static string ERROR_TCON39363131 = "TCON39363131"; // 9.6.11 Not found construction site information for the combination of concrete_batching_plant_contract + construction_site_number + contract_item_sequence
        public static string ERROR_TCON39363132 = "TCON39363132"; // 9.6.12 Concrete recipe information does not match what was reported for the construction site
        public static string ERROR_TCON39363133 = "TCON39363133"; // 9.6.13 Scheduled volume greater than contracted volume
        public static string ERROR_TCON39363134 = "TCON39363134"; // 9.6.14 This quantity released field is mandatory when confirm_delivery is equal to 'P'
        public static string ERROR_TCON39363135 = "TCON39363135"; // 9.6.15 Pump time must be less than delivery time
        public static string ERROR_TCON39363136 = "TCON39363136"; // 9.6.16 The pump piping distance field is mandatory if it informs the type of pump
        public static string ERROR_TCON39363137 = "TCON39363137"; // 9.6.17 The pump code field is mandatory if it informs the type of pump
        public static string ERROR_TCON39363138 = "TCON39363138"; // 9.6.18 Registered key (concrete_batching_plant_contract + contract_year + contract_number + concreting_sequence) already exists.
    }
}