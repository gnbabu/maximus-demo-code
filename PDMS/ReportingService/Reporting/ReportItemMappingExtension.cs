using ReportingService.Services.Data;


namespace ReportingService.Service.Reporting
{
    public static class ReportItemMappingExtension
    {
        public static ItemDefinition ToItemDefinition(this Item item, ItemDefinition itemDef = null)
        {
            return itemDef == null ? DTOMapper.Map<ItemDefinition>(item) : DTOMapper.Map<ItemDefinition>(item, itemDef);
        }
    }
}
