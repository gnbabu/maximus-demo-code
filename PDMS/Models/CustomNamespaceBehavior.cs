using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace x12eRATest
{
    public class CustomNamespaceBehaviorExtensionElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new CustomNamespaceBehavior();
        }

        public override Type BehaviorType
        {
            get
            {
                return typeof(CustomNamespaceBehavior);
            }
        }
    }

    public class CustomNamespaceBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            foreach (var operation in endpoint.Contract.Operations)
            {
                operation.Behaviors.Add(new CustomFormatterOperationBehavior());
            }
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            throw new NotImplementedException();
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    public class CustomFormatterOperationBehavior : IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters) { }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            IClientMessageFormatter innerDispatchFormatter = clientOperation.Formatter;

            clientOperation.Formatter = new CustomNamespaceMessageFormatter(innerDispatchFormatter);
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
        }

        public void Validate(OperationDescription operationDescription) { }
    }

    public class CustomNamespaceMessageFormatter : IClientMessageFormatter
    {
        private readonly IClientMessageFormatter formatter;

        public CustomNamespaceMessageFormatter(IClientMessageFormatter formatter)
        {
            this.formatter = formatter;
        }

        public object DeserializeReply(Message message, object[] parameters)
        {
            var customMessage = new CustomNamespaceMessage(message);
            var obj = this.formatter.DeserializeReply(customMessage, parameters);
            return obj;
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            var message = this.formatter.SerializeRequest(messageVersion, parameters);
            return new CustomNamespaceMessage(message);
        }
    }

    public class CustomNamespaceMessage : Message
    {
        private readonly Message message;

        public CustomNamespaceMessage(Message message)
        {
            this.message = message;
        }
        public override MessageHeaders Headers
        {
            get { return this.message.Headers; }
        }
        public override MessageProperties Properties
        {
            get { return this.message.Properties; }
        }
        public override MessageVersion Version
        {
            get { return this.message.Version; }
        }
        protected override void OnWriteStartBody(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("Body", "http://schemas.xmlsoap.org/soap/envelope/");
        }
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            this.message.WriteBodyContents(writer);
        }
        protected override void OnWriteStartEnvelope(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            writer.WriteAttributeString("xmlns", "v11",null, "http://service.tenncare.tn.gov/PDMS/835/v1.0");
            writer.WriteAttributeString("xmlns", "v1", null, "http://schemas.tenncare.tn.gov/header/v1.0");
        }
    }


}
