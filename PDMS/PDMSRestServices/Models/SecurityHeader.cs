using System.ServiceModel.Channels;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

public class SoapSecurityHeader : MessageHeader
{
    private readonly string _password, _username, _id;

    public SoapSecurityHeader()
    {    }

    public SoapSecurityHeader(string id, string username, string password)
    {
        _id = id;
        _password = password;
        _username = username;
    }
    public override bool MustUnderstand => true;

    public override string Name
    {
        get { return "Security"; }
    }

    public override string Namespace
    {
        get { return "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"; }
    }

    protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
    {
        writer.WriteStartElement("wsse", Name, Namespace);
        writer.WriteXmlnsAttribute("wsse", Namespace);
        writer.WriteXmlnsAttribute("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
    }

    protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
    {
        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss") + "z";
        Random r = new Random();
        DateTime created = DateTime.Now;
        //var nonce = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
        var nonce = Convert.ToBase64String(Encoding.ASCII.GetBytes(created + r.Next().ToString()));

        writer.WriteStartElement("wsse", "UsernameToken", Namespace);

        writer.WriteXmlnsAttribute("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
        writer.WriteAttributeString("wsu", "Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", _id);
        writer.WriteXmlnsAttribute("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
        // Username
        writer.WriteStartElement("wsse", "Username", Namespace);
        writer.WriteValue(_username);
        writer.WriteEndElement();
        // Password
        writer.WriteStartElement("wsse", "Password", Namespace);
        writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
        writer.WriteValue(_password);
        writer.WriteEndElement();
        // Nonce
        writer.WriteStartElement("wsse", "Nonce", Namespace);
        writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
        writer.WriteValue(nonce);
        writer.WriteEndElement();
        // Created
        writer.WriteStartElement("wsu", "Created", Namespace);
        writer.WriteValue(timeStamp);
        writer.WriteEndElement();

        writer.WriteEndElement();
    }
}