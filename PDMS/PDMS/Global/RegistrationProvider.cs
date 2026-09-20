/// <summary>
/// Summary description for RegistrationProvider
/// </summary>
public class RegistrationProvider : System.Web.UI.Page
{
    public RegistrationProvider()
    {
    }

    public int RegistrationStep { get; set; }
    public int RegistrationId { get; set; }
    public bool IsReadOnly { get; set; }
    public string CommandName { get; set; }
    public string MedicaidID { get; set; }
}