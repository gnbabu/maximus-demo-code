using System.Runtime.Serialization;
using System;


namespace PDMSRestServices.Models
    {
        [System.Runtime.Serialization.DataContractAttribute(Name = "MessageHeaderType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class MessageHeaderType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private string BusinessFlowField;

            private string StateCodeField;

            private MessageHeaderType.RequestorSystemType RequestorSystemField;

            private MessageHeaderType.SubscriberSystemType SubscriberSystemField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ModuleTransactionIdField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string AdditionalModuleTransactionIdField;

            private System.DateTime RequestTimestampField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string SITransactionKeyField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string BusinessFlow
            {
                get
                {
                    return this.BusinessFlowField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.BusinessFlowField, value) != true))
                    {
                        this.BusinessFlowField = value;
                        this.RaisePropertyChanged("BusinessFlow");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string StateCode
            {
                get
                {
                    return this.StateCodeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.StateCodeField, value) != true))
                    {
                        this.StateCodeField = value;
                        this.RaisePropertyChanged("StateCode");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
            public MessageHeaderType.RequestorSystemType RequestorSystem
            {
                get
                {
                    return this.RequestorSystemField;
                }
                set
                {
                    if ((this.RequestorSystemField.Equals(value) != true))
                    {
                        this.RequestorSystemField = value;
                        this.RaisePropertyChanged("RequestorSystem");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 3)]
            public MessageHeaderType.SubscriberSystemType SubscriberSystem
            {
                get
                {
                    return this.SubscriberSystemField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.SubscriberSystemField, value) != true))
                    {
                        this.SubscriberSystemField = value;
                        this.RaisePropertyChanged("SubscriberSystem");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
            public string ModuleTransactionId
            {
                get
                {
                    return this.ModuleTransactionIdField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ModuleTransactionIdField, value) != true))
                    {
                        this.ModuleTransactionIdField = value;
                        this.RaisePropertyChanged("ModuleTransactionId");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
            public string AdditionalModuleTransactionId
            {
                get
                {
                    return this.AdditionalModuleTransactionIdField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.AdditionalModuleTransactionIdField, value) != true))
                    {
                        this.AdditionalModuleTransactionIdField = value;
                        this.RaisePropertyChanged("AdditionalModuleTransactionId");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
            public System.DateTime RequestTimestamp
            {
                get
                {
                    return this.RequestTimestampField;
                }
                set
                {
                    if ((this.RequestTimestampField.Equals(value) != true))
                    {
                        this.RequestTimestampField = value;
                        this.RaisePropertyChanged("RequestTimestamp");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
            public string SITransactionKey
            {
                get
                {
                    return this.SITransactionKeyField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.SITransactionKeyField, value) != true))
                    {
                        this.SITransactionKeyField = value;
                        this.RaisePropertyChanged("SITransactionKey");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }


            [System.Runtime.Serialization.DataContractAttribute(Name = "MessageHeaderType.RequestorSystemType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
            public enum RequestorSystemType : int
            {

                [System.Runtime.Serialization.EnumMemberAttribute()]
                PNM = 0,
            }



            [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "MessageHeaderType.SubscriberSystemType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "Subscriber")]
            [System.SerializableAttribute()]
            public class SubscriberSystemType : System.Collections.Generic.List<string>
            {
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "InquireMemberEligibilityRequestPayloadType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class InquireMemberEligibilityRequestPayloadType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string MedicaidIdField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string SSNField;

            private string DateOfBirthField;

            private string FromDateOfServiceField;

            private string ToDateOfServiceField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ProcedureCodeField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string MedicaidId
            {
                get
                {
                    return this.MedicaidIdField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.MedicaidIdField, value) != true))
                    {
                        this.MedicaidIdField = value;
                        this.RaisePropertyChanged("MedicaidId");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string SSN
            {
                get
                {
                    return this.SSNField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.SSNField, value) != true))
                    {
                        this.SSNField = value;
                        this.RaisePropertyChanged("SSN");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 2)]
            public string DateOfBirth
            {
                get
                {
                    return this.DateOfBirthField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.DateOfBirthField, value) != true))
                    {
                        this.DateOfBirthField = value;
                        this.RaisePropertyChanged("DateOfBirth");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 3)]
            public string FromDateOfService
            {
                get
                {
                    return this.FromDateOfServiceField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.FromDateOfServiceField, value) != true))
                    {
                        this.FromDateOfServiceField = value;
                        this.RaisePropertyChanged("FromDateOfService");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 4)]
            public string ToDateOfService
            {
                get
                {
                    return this.ToDateOfServiceField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ToDateOfServiceField, value) != true))
                    {
                        this.ToDateOfServiceField = value;
                        this.RaisePropertyChanged("ToDateOfService");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
            public string ProcedureCode
            {
                get
                {
                    return this.ProcedureCodeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ProcedureCodeField, value) != true))
                    {
                        this.ProcedureCodeField = value;
                        this.RaisePropertyChanged("ProcedureCode");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "ResponseHeaderType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class ResponseHeaderType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string SITransactionKeyField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ModuleTransactionIdField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string AdditionalModuleTransactionIdField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ResponseCodeField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ResponseTypeField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ResponseMessageField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ResponseDetailsField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string SITransactionKey
            {
                get
                {
                    return this.SITransactionKeyField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.SITransactionKeyField, value) != true))
                    {
                        this.SITransactionKeyField = value;
                        this.RaisePropertyChanged("SITransactionKey");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
            public string ModuleTransactionId
            {
                get
                {
                    return this.ModuleTransactionIdField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ModuleTransactionIdField, value) != true))
                    {
                        this.ModuleTransactionIdField = value;
                        this.RaisePropertyChanged("ModuleTransactionId");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
            public string AdditionalModuleTransactionId
            {
                get
                {
                    return this.AdditionalModuleTransactionIdField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.AdditionalModuleTransactionIdField, value) != true))
                    {
                        this.AdditionalModuleTransactionIdField = value;
                        this.RaisePropertyChanged("AdditionalModuleTransactionId");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
            public string ResponseCode
            {
                get
                {
                    return this.ResponseCodeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ResponseCodeField, value) != true))
                    {
                        this.ResponseCodeField = value;
                        this.RaisePropertyChanged("ResponseCode");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
            public string ResponseType
            {
                get
                {
                    return this.ResponseTypeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ResponseTypeField, value) != true))
                    {
                        this.ResponseTypeField = value;
                        this.RaisePropertyChanged("ResponseType");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
            public string ResponseMessage
            {
                get
                {
                    return this.ResponseMessageField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ResponseMessageField, value) != true))
                    {
                        this.ResponseMessageField = value;
                        this.RaisePropertyChanged("ResponseMessage");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
            public string ResponseDetails
            {
                get
                {
                    return this.ResponseDetailsField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ResponseDetailsField, value) != true))
                    {
                        this.ResponseDetailsField = value;
                        this.RaisePropertyChanged("ResponseDetails");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "MemberEligibilityInfoType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class MemberEligibilityInfoType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private RecipientInfoType RecipientInfoField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private ListOfBenefitAssignmentPlansType ListOfBenefitAssignmentPlansField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private ListOfManagedCarePlanType ListOfManagedCarePlanField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private ListOfThirdPartyLiabilityType ListOfThirdPartyLiabilityField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private ListOfPatientLiabilityType ListOfPatientLiabilityField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private ListOfLTCFPlacementType ListOfLTCFPlacementField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private ListOfLockinDetailType ListOfLockinDetailField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private ListOfMedicareCoverageDetailType ListOfMedicareCoverageDetailField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private ListOfSNIFlOCDetailType ListOfSNIFlOCDetailField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private ListOfServiceLimitationsType ListOfServiceLimitationsField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private ListOfRestictedCoverageType ListOfRestictedCoverageField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private ListOfFamilyMemberInfoType ListOfMembersOnCaseUnder19Field;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public RecipientInfoType RecipientInfo
            {
                get
                {
                    return this.RecipientInfoField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.RecipientInfoField, value) != true))
                    {
                        this.RecipientInfoField = value;
                        this.RaisePropertyChanged("RecipientInfo");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
            public ListOfBenefitAssignmentPlansType ListOfBenefitAssignmentPlans
            {
                get
                {
                    return this.ListOfBenefitAssignmentPlansField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ListOfBenefitAssignmentPlansField, value) != true))
                    {
                        this.ListOfBenefitAssignmentPlansField = value;
                        this.RaisePropertyChanged("ListOfBenefitAssignmentPlans");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
            public ListOfManagedCarePlanType ListOfManagedCarePlan
            {
                get
                {
                    return this.ListOfManagedCarePlanField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ListOfManagedCarePlanField, value) != true))
                    {
                        this.ListOfManagedCarePlanField = value;
                        this.RaisePropertyChanged("ListOfManagedCarePlan");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
            public ListOfThirdPartyLiabilityType ListOfThirdPartyLiability
            {
                get
                {
                    return this.ListOfThirdPartyLiabilityField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ListOfThirdPartyLiabilityField, value) != true))
                    {
                        this.ListOfThirdPartyLiabilityField = value;
                        this.RaisePropertyChanged("ListOfThirdPartyLiability");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
            public ListOfPatientLiabilityType ListOfPatientLiability
            {
                get
                {
                    return this.ListOfPatientLiabilityField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ListOfPatientLiabilityField, value) != true))
                    {
                        this.ListOfPatientLiabilityField = value;
                        this.RaisePropertyChanged("ListOfPatientLiability");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
            public ListOfLTCFPlacementType ListOfLTCFPlacement
            {
                get
                {
                    return this.ListOfLTCFPlacementField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ListOfLTCFPlacementField, value) != true))
                    {
                        this.ListOfLTCFPlacementField = value;
                        this.RaisePropertyChanged("ListOfLTCFPlacement");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
            public ListOfLockinDetailType ListOfLockinDetail
            {
                get
                {
                    return this.ListOfLockinDetailField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ListOfLockinDetailField, value) != true))
                    {
                        this.ListOfLockinDetailField = value;
                        this.RaisePropertyChanged("ListOfLockinDetail");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
            public ListOfMedicareCoverageDetailType ListOfMedicareCoverageDetail
            {
                get
                {
                    return this.ListOfMedicareCoverageDetailField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ListOfMedicareCoverageDetailField, value) != true))
                    {
                        this.ListOfMedicareCoverageDetailField = value;
                        this.RaisePropertyChanged("ListOfMedicareCoverageDetail");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
            public ListOfSNIFlOCDetailType ListOfSNIFlOCDetail
            {
                get
                {
                    return this.ListOfSNIFlOCDetailField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ListOfSNIFlOCDetailField, value) != true))
                    {
                        this.ListOfSNIFlOCDetailField = value;
                        this.RaisePropertyChanged("ListOfSNIFlOCDetail");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
            public ListOfServiceLimitationsType ListOfServiceLimitations
            {
                get
                {
                    return this.ListOfServiceLimitationsField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ListOfServiceLimitationsField, value) != true))
                    {
                        this.ListOfServiceLimitationsField = value;
                        this.RaisePropertyChanged("ListOfServiceLimitations");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 10)]
            public ListOfRestictedCoverageType ListOfRestictedCoverage
            {
                get
                {
                    return this.ListOfRestictedCoverageField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ListOfRestictedCoverageField, value) != true))
                    {
                        this.ListOfRestictedCoverageField = value;
                        this.RaisePropertyChanged("ListOfRestictedCoverage");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 11)]
            public ListOfFamilyMemberInfoType ListOfMembersOnCaseUnder19
            {
                get
                {
                    return this.ListOfMembersOnCaseUnder19Field;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ListOfMembersOnCaseUnder19Field, value) != true))
                    {
                        this.ListOfMembersOnCaseUnder19Field = value;
                        this.RaisePropertyChanged("ListOfMembersOnCaseUnder19");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "RecipientInfoType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class RecipientInfoType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private string MedicaidIdField;

            private string DateOfBirthField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string DateOfDeathField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string FirstNameField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string MiddleNameField;

            private string LastNameField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string SSNField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string GenderField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private MemberAddressType MemberAddressField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string MedicaidId
            {
                get
                {
                    return this.MedicaidIdField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.MedicaidIdField, value) != true))
                    {
                        this.MedicaidIdField = value;
                        this.RaisePropertyChanged("MedicaidId");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 1)]
            public string DateOfBirth
            {
                get
                {
                    return this.DateOfBirthField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.DateOfBirthField, value) != true))
                    {
                        this.DateOfBirthField = value;
                        this.RaisePropertyChanged("DateOfBirth");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
            public string DateOfDeath
            {
                get
                {
                    return this.DateOfDeathField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.DateOfDeathField, value) != true))
                    {
                        this.DateOfDeathField = value;
                        this.RaisePropertyChanged("DateOfDeath");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
            public string FirstName
            {
                get
                {
                    return this.FirstNameField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.FirstNameField, value) != true))
                    {
                        this.FirstNameField = value;
                        this.RaisePropertyChanged("FirstName");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
            public string MiddleName
            {
                get
                {
                    return this.MiddleNameField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.MiddleNameField, value) != true))
                    {
                        this.MiddleNameField = value;
                        this.RaisePropertyChanged("MiddleName");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 5)]
            public string LastName
            {
                get
                {
                    return this.LastNameField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.LastNameField, value) != true))
                    {
                        this.LastNameField = value;
                        this.RaisePropertyChanged("LastName");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
            public string SSN
            {
                get
                {
                    return this.SSNField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.SSNField, value) != true))
                    {
                        this.SSNField = value;
                        this.RaisePropertyChanged("SSN");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
            public string Gender
            {
                get
                {
                    return this.GenderField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.GenderField, value) != true))
                    {
                        this.GenderField = value;
                        this.RaisePropertyChanged("Gender");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
            public MemberAddressType MemberAddress
            {
                get
                {
                    return this.MemberAddressField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.MemberAddressField, value) != true))
                    {
                        this.MemberAddressField = value;
                        this.RaisePropertyChanged("MemberAddress");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ListOfBenefitAssignmentPlansType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "BenefitAssignmentPlan")]
        [System.SerializableAttribute()]
        public class ListOfBenefitAssignmentPlansType : System.Collections.Generic.List<BenefitAssignmentPlanType>
        {
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ListOfManagedCarePlanType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "ManagedCarePlan")]
        [System.SerializableAttribute()]
        public class ListOfManagedCarePlanType : System.Collections.Generic.List<ManagedCarePlanType>
        {
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ListOfThirdPartyLiabilityType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "ThirdPartyLiability")]
        [System.SerializableAttribute()]
        public class ListOfThirdPartyLiabilityType : System.Collections.Generic.List<ThirdPartyLiabilityType>
        {
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ListOfPatientLiabilityType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "PatientLiability")]
        [System.SerializableAttribute()]
        public class ListOfPatientLiabilityType : System.Collections.Generic.List<PatientLiabilityType>
        {
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ListOfLTCFPlacementType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "LTCFPlacement")]
        [System.SerializableAttribute()]
        public class ListOfLTCFPlacementType : System.Collections.Generic.List<LTCFPlacementType>
        {
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ListOfLockinDetailType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "LockinDetail")]
        [System.SerializableAttribute()]
        public class ListOfLockinDetailType : System.Collections.Generic.List<LockinDetailType>
        {
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ListOfMedicareCoverageDetailType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "MedicareCoverageDetail")]
        [System.SerializableAttribute()]
        public class ListOfMedicareCoverageDetailType : System.Collections.Generic.List<MedicareCoverageDetailType>
        {
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ListOfSNIFlOCDetailType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "SNIFlOCDetail")]
        [System.SerializableAttribute()]
        public class ListOfSNIFlOCDetailType : System.Collections.Generic.List<SNIFlOCDetailType>
        {
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ListOfServiceLimitationsType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "ServiceLimitations")]
        [System.SerializableAttribute()]
        public class ListOfServiceLimitationsType : System.Collections.Generic.List<ServiceLimitationsType>
        {
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ListOfRestictedCoverageType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "RestictedCoverage")]
        [System.SerializableAttribute()]
        public class ListOfRestictedCoverageType : System.Collections.Generic.List<RestictedCoverageType>
        {
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ListOfFamilyMemberInfoType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "FamilyMemberInfo")]
        [System.SerializableAttribute()]
        public class ListOfFamilyMemberInfoType : System.Collections.Generic.List<FamilyMemberInfoType>
        {
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "MemberAddressType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class MemberAddressType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string AddressLine1Field;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string AddressLine2Field;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string CityField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string StateCodeField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ZipCode5Field;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string AddressLine1
            {
                get
                {
                    return this.AddressLine1Field;
                }
                set
                {
                    if ((object.ReferenceEquals(this.AddressLine1Field, value) != true))
                    {
                        this.AddressLine1Field = value;
                        this.RaisePropertyChanged("AddressLine1");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string AddressLine2
            {
                get
                {
                    return this.AddressLine2Field;
                }
                set
                {
                    if ((object.ReferenceEquals(this.AddressLine2Field, value) != true))
                    {
                        this.AddressLine2Field = value;
                        this.RaisePropertyChanged("AddressLine2");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string City
            {
                get
                {
                    return this.CityField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.CityField, value) != true))
                    {
                        this.CityField = value;
                        this.RaisePropertyChanged("City");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string StateCode
            {
                get
                {
                    return this.StateCodeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.StateCodeField, value) != true))
                    {
                        this.StateCodeField = value;
                        this.RaisePropertyChanged("StateCode");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string ZipCode5
            {
                get
                {
                    return this.ZipCode5Field;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ZipCode5Field, value) != true))
                    {
                        this.ZipCode5Field = value;
                        this.RaisePropertyChanged("ZipCode5");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "BenefitAssignmentPlanType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class BenefitAssignmentPlanType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private string AssignmentPlanField;

            private string EffectiveDateField;

            private string EndDateField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string AssignmentPlan
            {
                get
                {
                    return this.AssignmentPlanField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.AssignmentPlanField, value) != true))
                    {
                        this.AssignmentPlanField = value;
                        this.RaisePropertyChanged("AssignmentPlan");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string EffectiveDate
            {
                get
                {
                    return this.EffectiveDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EffectiveDateField, value) != true))
                    {
                        this.EffectiveDateField = value;
                        this.RaisePropertyChanged("EffectiveDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string EndDate
            {
                get
                {
                    return this.EndDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EndDateField, value) != true))
                    {
                        this.EndDateField = value;
                        this.RaisePropertyChanged("EndDate");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "ManagedCarePlanType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class ManagedCarePlanType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private string ManagedCarePlanIdField;

            private string PlanNameField;

            private string PlanDescriptionField;

            private string EffectiveDateField;

            private string EndDateField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ManagedCareBenefitsField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string ManagedCarePlanId
            {
                get
                {
                    return this.ManagedCarePlanIdField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ManagedCarePlanIdField, value) != true))
                    {
                        this.ManagedCarePlanIdField = value;
                        this.RaisePropertyChanged("ManagedCarePlanId");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string PlanName
            {
                get
                {
                    return this.PlanNameField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.PlanNameField, value) != true))
                    {
                        this.PlanNameField = value;
                        this.RaisePropertyChanged("PlanName");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 2)]
            public string PlanDescription
            {
                get
                {
                    return this.PlanDescriptionField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.PlanDescriptionField, value) != true))
                    {
                        this.PlanDescriptionField = value;
                        this.RaisePropertyChanged("PlanDescription");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 3)]
            public string EffectiveDate
            {
                get
                {
                    return this.EffectiveDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EffectiveDateField, value) != true))
                    {
                        this.EffectiveDateField = value;
                        this.RaisePropertyChanged("EffectiveDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 4)]
            public string EndDate
            {
                get
                {
                    return this.EndDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EndDateField, value) != true))
                    {
                        this.EndDateField = value;
                        this.RaisePropertyChanged("EndDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
            public string ManagedCareBenefits
            {
                get
                {
                    return this.ManagedCareBenefitsField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ManagedCareBenefitsField, value) != true))
                    {
                        this.ManagedCareBenefitsField = value;
                        this.RaisePropertyChanged("ManagedCareBenefits");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "ThirdPartyLiabilityType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class ThirdPartyLiabilityType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string CarrierNameField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string CarrierNumberField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string PolicyNumberField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string PolicyHolderField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string CoverageTypeField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string CoverageField;

            private string EffectiveDateField;

            private string EndDateField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string GroupNumberField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string CarrierName
            {
                get
                {
                    return this.CarrierNameField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.CarrierNameField, value) != true))
                    {
                        this.CarrierNameField = value;
                        this.RaisePropertyChanged("CarrierName");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string CarrierNumber
            {
                get
                {
                    return this.CarrierNumberField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.CarrierNumberField, value) != true))
                    {
                        this.CarrierNumberField = value;
                        this.RaisePropertyChanged("CarrierNumber");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string PolicyNumber
            {
                get
                {
                    return this.PolicyNumberField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.PolicyNumberField, value) != true))
                    {
                        this.PolicyNumberField = value;
                        this.RaisePropertyChanged("PolicyNumber");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
            public string PolicyHolder
            {
                get
                {
                    return this.PolicyHolderField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.PolicyHolderField, value) != true))
                    {
                        this.PolicyHolderField = value;
                        this.RaisePropertyChanged("PolicyHolder");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
            public string CoverageType
            {
                get
                {
                    return this.CoverageTypeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.CoverageTypeField, value) != true))
                    {
                        this.CoverageTypeField = value;
                        this.RaisePropertyChanged("CoverageType");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
            public string Coverage
            {
                get
                {
                    return this.CoverageField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.CoverageField, value) != true))
                    {
                        this.CoverageField = value;
                        this.RaisePropertyChanged("Coverage");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 6)]
            public string EffectiveDate
            {
                get
                {
                    return this.EffectiveDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EffectiveDateField, value) != true))
                    {
                        this.EffectiveDateField = value;
                        this.RaisePropertyChanged("EffectiveDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 7)]
            public string EndDate
            {
                get
                {
                    return this.EndDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EndDateField, value) != true))
                    {
                        this.EndDateField = value;
                        this.RaisePropertyChanged("EndDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
            public string GroupNumber
            {
                get
                {
                    return this.GroupNumberField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.GroupNumberField, value) != true))
                    {
                        this.GroupNumberField = value;
                        this.RaisePropertyChanged("GroupNumber");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "PatientLiabilityType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class PatientLiabilityType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private string PatientLiabilityFinancialPayerField;

            private decimal MonthlyAmountField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string TypeField;

            private string EffectiveDateField;

            private string EndDateField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string PatientLiabilityFinancialPayer
            {
                get
                {
                    return this.PatientLiabilityFinancialPayerField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.PatientLiabilityFinancialPayerField, value) != true))
                    {
                        this.PatientLiabilityFinancialPayerField = value;
                        this.RaisePropertyChanged("PatientLiabilityFinancialPayer");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
            public decimal MonthlyAmount
            {
                get
                {
                    return this.MonthlyAmountField;
                }
                set
                {
                    if ((this.MonthlyAmountField.Equals(value) != true))
                    {
                        this.MonthlyAmountField = value;
                        this.RaisePropertyChanged("MonthlyAmount");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
            public string Type
            {
                get
                {
                    return this.TypeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.TypeField, value) != true))
                    {
                        this.TypeField = value;
                        this.RaisePropertyChanged("Type");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 3)]
            public string EffectiveDate
            {
                get
                {
                    return this.EffectiveDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EffectiveDateField, value) != true))
                    {
                        this.EffectiveDateField = value;
                        this.RaisePropertyChanged("EffectiveDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 4)]
            public string EndDate
            {
                get
                {
                    return this.EndDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EndDateField, value) != true))
                    {
                        this.EndDateField = value;
                        this.RaisePropertyChanged("EndDate");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "LTCFPlacementType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class LTCFPlacementType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private string FacilityTypeField;

            private string EffectiveDateField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string EndDateField;

            private string EffectiveDateMedicaidCoverageField;

            private string EndDateMedicaidCoverageField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string FacilityType
            {
                get
                {
                    return this.FacilityTypeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.FacilityTypeField, value) != true))
                    {
                        this.FacilityTypeField = value;
                        this.RaisePropertyChanged("FacilityType");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 1)]
            public string EffectiveDate
            {
                get
                {
                    return this.EffectiveDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EffectiveDateField, value) != true))
                    {
                        this.EffectiveDateField = value;
                        this.RaisePropertyChanged("EffectiveDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
            public string EndDate
            {
                get
                {
                    return this.EndDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EndDateField, value) != true))
                    {
                        this.EndDateField = value;
                        this.RaisePropertyChanged("EndDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 3)]
            public string EffectiveDateMedicaidCoverage
            {
                get
                {
                    return this.EffectiveDateMedicaidCoverageField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EffectiveDateMedicaidCoverageField, value) != true))
                    {
                        this.EffectiveDateMedicaidCoverageField = value;
                        this.RaisePropertyChanged("EffectiveDateMedicaidCoverage");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 4)]
            public string EndDateMedicaidCoverage
            {
                get
                {
                    return this.EndDateMedicaidCoverageField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EndDateMedicaidCoverageField, value) != true))
                    {
                        this.EndDateMedicaidCoverageField = value;
                        this.RaisePropertyChanged("EndDateMedicaidCoverage");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "LockinDetailType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class LockinDetailType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private string LockinPlanField;

            private string LockinTypeField;

            private string EffectiveDateField;

            private string EndDateField;

            private string ProviderNPIField;

            private string ProviderNameField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ProviderPhoneNumberField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string LockinPlan
            {
                get
                {
                    return this.LockinPlanField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.LockinPlanField, value) != true))
                    {
                        this.LockinPlanField = value;
                        this.RaisePropertyChanged("LockinPlan");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string LockinType
            {
                get
                {
                    return this.LockinTypeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.LockinTypeField, value) != true))
                    {
                        this.LockinTypeField = value;
                        this.RaisePropertyChanged("LockinType");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 2)]
            public string EffectiveDate
            {
                get
                {
                    return this.EffectiveDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EffectiveDateField, value) != true))
                    {
                        this.EffectiveDateField = value;
                        this.RaisePropertyChanged("EffectiveDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 3)]
            public string EndDate
            {
                get
                {
                    return this.EndDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EndDateField, value) != true))
                    {
                        this.EndDateField = value;
                        this.RaisePropertyChanged("EndDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 4)]
            public string ProviderNPI
            {
                get
                {
                    return this.ProviderNPIField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ProviderNPIField, value) != true))
                    {
                        this.ProviderNPIField = value;
                        this.RaisePropertyChanged("ProviderNPI");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 5)]
            public string ProviderName
            {
                get
                {
                    return this.ProviderNameField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ProviderNameField, value) != true))
                    {
                        this.ProviderNameField = value;
                        this.RaisePropertyChanged("ProviderName");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
            public string ProviderPhoneNumber
            {
                get
                {
                    return this.ProviderPhoneNumberField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ProviderPhoneNumberField, value) != true))
                    {
                        this.ProviderPhoneNumberField = value;
                        this.RaisePropertyChanged("ProviderPhoneNumber");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "MedicareCoverageDetailType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class MedicareCoverageDetailType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private string CoverageField;

            private string PlanIdField;

            private string PlanNameField;

            private string MedicareIdField;

            private string EffectiveDateField;

            private string EndDateField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string Coverage
            {
                get
                {
                    return this.CoverageField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.CoverageField, value) != true))
                    {
                        this.CoverageField = value;
                        this.RaisePropertyChanged("Coverage");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string PlanId
            {
                get
                {
                    return this.PlanIdField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.PlanIdField, value) != true))
                    {
                        this.PlanIdField = value;
                        this.RaisePropertyChanged("PlanId");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string PlanName
            {
                get
                {
                    return this.PlanNameField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.PlanNameField, value) != true))
                    {
                        this.PlanNameField = value;
                        this.RaisePropertyChanged("PlanName");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 3)]
            public string MedicareId
            {
                get
                {
                    return this.MedicareIdField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.MedicareIdField, value) != true))
                    {
                        this.MedicareIdField = value;
                        this.RaisePropertyChanged("MedicareId");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 4)]
            public string EffectiveDate
            {
                get
                {
                    return this.EffectiveDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EffectiveDateField, value) != true))
                    {
                        this.EffectiveDateField = value;
                        this.RaisePropertyChanged("EffectiveDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 5)]
            public string EndDate
            {
                get
                {
                    return this.EndDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EndDateField, value) != true))
                    {
                        this.EndDateField = value;
                        this.RaisePropertyChanged("EndDate");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "SNIFlOCDetailType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class SNIFlOCDetailType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private string FacilityTypeField;

            private string StatusField;

            private string DeterminationDateField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string LOCDeterminationField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string DescriptionField;

            private string StartDateField;

            private string EndDateField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string FacilityType
            {
                get
                {
                    return this.FacilityTypeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.FacilityTypeField, value) != true))
                    {
                        this.FacilityTypeField = value;
                        this.RaisePropertyChanged("FacilityType");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string Status
            {
                get
                {
                    return this.StatusField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.StatusField, value) != true))
                    {
                        this.StatusField = value;
                        this.RaisePropertyChanged("Status");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 2)]
            public string DeterminationDate
            {
                get
                {
                    return this.DeterminationDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.DeterminationDateField, value) != true))
                    {
                        this.DeterminationDateField = value;
                        this.RaisePropertyChanged("DeterminationDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
            public string LOCDetermination
            {
                get
                {
                    return this.LOCDeterminationField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.LOCDeterminationField, value) != true))
                    {
                        this.LOCDeterminationField = value;
                        this.RaisePropertyChanged("LOCDetermination");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
            public string Description
            {
                get
                {
                    return this.DescriptionField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.DescriptionField, value) != true))
                    {
                        this.DescriptionField = value;
                        this.RaisePropertyChanged("Description");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 5)]
            public string StartDate
            {
                get
                {
                    return this.StartDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.StartDateField, value) != true))
                    {
                        this.StartDateField = value;
                        this.RaisePropertyChanged("StartDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 6)]
            public string EndDate
            {
                get
                {
                    return this.EndDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EndDateField, value) != true))
                    {
                        this.EndDateField = value;
                        this.RaisePropertyChanged("EndDate");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "ServiceLimitationsType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class ServiceLimitationsType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ProcedureCodeField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string ServiceLimitDescriptionField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string BenefitDescriptionField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private int TotalLimitsField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private int UsedLimitsField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private int RemainingLimitsField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string TimeframeField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string DateOfNextServiceField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string ProcedureCode
            {
                get
                {
                    return this.ProcedureCodeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ProcedureCodeField, value) != true))
                    {
                        this.ProcedureCodeField = value;
                        this.RaisePropertyChanged("ProcedureCode");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string ServiceLimitDescription
            {
                get
                {
                    return this.ServiceLimitDescriptionField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ServiceLimitDescriptionField, value) != true))
                    {
                        this.ServiceLimitDescriptionField = value;
                        this.RaisePropertyChanged("ServiceLimitDescription");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
            public string BenefitDescription
            {
                get
                {
                    return this.BenefitDescriptionField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.BenefitDescriptionField, value) != true))
                    {
                        this.BenefitDescriptionField = value;
                        this.RaisePropertyChanged("BenefitDescription");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(Order = 3)]
            public int TotalLimits
            {
                get
                {
                    return this.TotalLimitsField;
                }
                set
                {
                    if ((this.TotalLimitsField.Equals(value) != true))
                    {
                        this.TotalLimitsField = value;
                        this.RaisePropertyChanged("TotalLimits");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(Order = 4)]
            public int UsedLimits
            {
                get
                {
                    return this.UsedLimitsField;
                }
                set
                {
                    if ((this.UsedLimitsField.Equals(value) != true))
                    {
                        this.UsedLimitsField = value;
                        this.RaisePropertyChanged("UsedLimits");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(Order = 5)]
            public int RemainingLimits
            {
                get
                {
                    return this.RemainingLimitsField;
                }
                set
                {
                    if ((this.RemainingLimitsField.Equals(value) != true))
                    {
                        this.RemainingLimitsField = value;
                        this.RaisePropertyChanged("RemainingLimits");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
            public string Timeframe
            {
                get
                {
                    return this.TimeframeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.TimeframeField, value) != true))
                    {
                        this.TimeframeField = value;
                        this.RaisePropertyChanged("Timeframe");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
            public string DateOfNextService
            {
                get
                {
                    return this.DateOfNextServiceField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.DateOfNextServiceField, value) != true))
                    {
                        this.DateOfNextServiceField = value;
                        this.RaisePropertyChanged("DateOfNextService");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "RestictedCoverageType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class RestictedCoverageType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private string EffectiveDateField;

            private string EndDateField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string EffectiveDate
            {
                get
                {
                    return this.EffectiveDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EffectiveDateField, value) != true))
                    {
                        this.EffectiveDateField = value;
                        this.RaisePropertyChanged("EffectiveDate");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string EndDate
            {
                get
                {
                    return this.EndDateField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.EndDateField, value) != true))
                    {
                        this.EndDateField = value;
                        this.RaisePropertyChanged("EndDate");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "FamilyMemberInfoType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class FamilyMemberInfoType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string MedicaidIdField;

            private string DateOfBirthField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string FirstNameField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string MiddleNameField;

            private string LastNameField;

            [System.Runtime.Serialization.OptionalFieldAttribute()]
            private string GenderField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
            public string MedicaidId
            {
                get
                {
                    return this.MedicaidIdField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.MedicaidIdField, value) != true))
                    {
                        this.MedicaidIdField = value;
                        this.RaisePropertyChanged("MedicaidId");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 1)]
            public string DateOfBirth
            {
                get
                {
                    return this.DateOfBirthField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.DateOfBirthField, value) != true))
                    {
                        this.DateOfBirthField = value;
                        this.RaisePropertyChanged("DateOfBirth");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
            public string FirstName
            {
                get
                {
                    return this.FirstNameField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.FirstNameField, value) != true))
                    {
                        this.FirstNameField = value;
                        this.RaisePropertyChanged("FirstName");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
            public string MiddleName
            {
                get
                {
                    return this.MiddleNameField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.MiddleNameField, value) != true))
                    {
                        this.MiddleNameField = value;
                        this.RaisePropertyChanged("MiddleName");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false, Order = 4)]
            public string LastName
            {
                get
                {
                    return this.LastNameField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.LastNameField, value) != true))
                    {
                        this.LastNameField = value;
                        this.RaisePropertyChanged("LastName");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
            public string Gender
            {
                get
                {
                    return this.GenderField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.GenderField, value) != true))
                    {
                        this.GenderField = value;
                        this.RaisePropertyChanged("Gender");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }



        [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "ErrorsType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ItemName = "ErrorDetails")]
        [System.SerializableAttribute()]
        public class ErrorsType : System.Collections.Generic.List<ErrorDetailsType>
        {
        }



        [System.Runtime.Serialization.DataContractAttribute(Name = "ErrorDetailsType", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        [System.SerializableAttribute()]
        public partial class ErrorDetailsType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        {

            [System.NonSerializedAttribute()]
            private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

            private string ErrorCodeField;

            private string ErrorDescriptionField;

            [global::System.ComponentModel.BrowsableAttribute(false)]
            public System.Runtime.Serialization.ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.extensionDataField;
                }
                set
                {
                    this.extensionDataField = value;
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string ErrorCode
            {
                get
                {
                    return this.ErrorCodeField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ErrorCodeField, value) != true))
                    {
                        this.ErrorCodeField = value;
                        this.RaisePropertyChanged("ErrorCode");
                    }
                }
            }

            [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, EmitDefaultValue = false)]
            public string ErrorDescription
            {
                get
                {
                    return this.ErrorDescriptionField;
                }
                set
                {
                    if ((object.ReferenceEquals(this.ErrorDescriptionField, value) != true))
                    {
                        this.ErrorDescriptionField = value;
                        this.RaisePropertyChanged("ErrorDescription");
                    }
                }
            }

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected void RaisePropertyChanged(string propertyName)
            {
                System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
                if ((propertyChanged != null))
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }


        [System.ServiceModel.ServiceContractAttribute(Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", ConfigurationName = "MemberEligibilityInquiryService")]
        public interface MemberEligibilityInquiryService
        {

            [System.ServiceModel.OperationContractAttribute(Action = "http://service.membermgmt.fi/MemberEligibilityInquiryService/InquireMemberEligibi" +
                "lity", ReplyAction = "*")]
            InquireMemberEligibilityResponse InquireMemberEligibility(InquireMemberEligibilityRequest request);

            [System.ServiceModel.OperationContractAttribute(Action = "http://service.membermgmt.fi/MemberEligibilityInquiryService/InquireMemberEligibi" +
                "lity", ReplyAction = "*")]
            System.Threading.Tasks.Task<InquireMemberEligibilityResponse> InquireMemberEligibilityAsync(InquireMemberEligibilityRequest request);
        }

        public partial class InquireMemberEligibilityRequest
        {

            [System.ServiceModel.MessageBodyMemberAttribute(Name = "InquireMemberEligibilityRequest", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", Order = 0)]
            public InquireMemberEligibilityRequestBody Body;

            public InquireMemberEligibilityRequest()
            {
            }

            public InquireMemberEligibilityRequest(InquireMemberEligibilityRequestBody Body)
            {
                this.Body = Body;
            }
        }


        [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        public partial class InquireMemberEligibilityRequestBody
        {

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
            public MessageHeaderType MessageHeader;

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
            public InquireMemberEligibilityRequestPayloadType RequestPayload;

            public InquireMemberEligibilityRequestBody()
            {
            }

            public InquireMemberEligibilityRequestBody(MessageHeaderType MessageHeader, InquireMemberEligibilityRequestPayloadType RequestPayload)
            {
                this.MessageHeader = MessageHeader;
                this.RequestPayload = RequestPayload;
            }
        }



        [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
        public partial class InquireMemberEligibilityResponse
        {

            [System.ServiceModel.MessageBodyMemberAttribute(Name = "InquireMemberEligibilityResponse", Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService", Order = 0)]
            public InquireMemberEligibilityResponseBody Body;

            public InquireMemberEligibilityResponse()
            {
            }

            public InquireMemberEligibilityResponse(InquireMemberEligibilityResponseBody Body)
            {
                this.Body = Body;
            }
        }



        [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://service.membermgmt.fi/MemberEligibilityInquiryService")]
        public partial class InquireMemberEligibilityResponseBody
        {

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
            public ResponseHeaderType ResponseHeader;

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 1)]
            public MemberEligibilityInfoType MemberEligibilityInfo;

            [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 2)]
            public ErrorsType Errors;

            public InquireMemberEligibilityResponseBody()
            {
            }

            public InquireMemberEligibilityResponseBody(ResponseHeaderType ResponseHeader, MemberEligibilityInfoType MemberEligibilityInfo, ErrorsType Errors)
            {
                this.ResponseHeader = ResponseHeader;
                this.MemberEligibilityInfo = MemberEligibilityInfo;
                this.Errors = Errors;
            }
        }


        public interface MemberEligibilityInquiryServiceChannel : MemberEligibilityInquiryService, System.ServiceModel.IClientChannel
        {
        }



        public partial class MemberEligibilityInquiryServiceClient : System.ServiceModel.ClientBase<MemberEligibilityInquiryService>, MemberEligibilityInquiryService
        {

            public MemberEligibilityInquiryServiceClient()
            {
            }

            public MemberEligibilityInquiryServiceClient(string endpointConfigurationName) :
                    base(endpointConfigurationName)
            {
            }

            public MemberEligibilityInquiryServiceClient(string endpointConfigurationName, string remoteAddress) :
                    base(endpointConfigurationName, remoteAddress)
            {
            }

            public MemberEligibilityInquiryServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                    base(endpointConfigurationName, remoteAddress)
            {
            }

            public MemberEligibilityInquiryServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                    base(binding, remoteAddress)
            {
            }

            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
            InquireMemberEligibilityResponse MemberEligibilityInquiryService.InquireMemberEligibility(InquireMemberEligibilityRequest request)
            {
                return base.Channel.InquireMemberEligibility(request);
            }

            public ResponseHeaderType InquireMemberEligibility(MessageHeaderType MessageHeader, InquireMemberEligibilityRequestPayloadType RequestPayload, out MemberEligibilityInfoType MemberEligibilityInfo, out ErrorsType Errors)
            {
                InquireMemberEligibilityRequest inValue = new InquireMemberEligibilityRequest();
                inValue.Body = new InquireMemberEligibilityRequestBody();
                inValue.Body.MessageHeader = MessageHeader;
                inValue.Body.RequestPayload = RequestPayload;
                InquireMemberEligibilityResponse retVal = ((MemberEligibilityInquiryService)(this)).InquireMemberEligibility(inValue);
                MemberEligibilityInfo = retVal.Body.MemberEligibilityInfo;
                Errors = retVal.Body.Errors;
                return retVal.Body.ResponseHeader;
            }

            [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
            System.Threading.Tasks.Task<InquireMemberEligibilityResponse> MemberEligibilityInquiryService.InquireMemberEligibilityAsync(InquireMemberEligibilityRequest request)
            {
                return base.Channel.InquireMemberEligibilityAsync(request);
            }

            public System.Threading.Tasks.Task<InquireMemberEligibilityResponse> InquireMemberEligibilityAsync(MessageHeaderType MessageHeader, InquireMemberEligibilityRequestPayloadType RequestPayload)
            {
                InquireMemberEligibilityRequest inValue = new InquireMemberEligibilityRequest();
                inValue.Body = new InquireMemberEligibilityRequestBody();
                inValue.Body.MessageHeader = MessageHeader;
                inValue.Body.RequestPayload = RequestPayload;
                return ((MemberEligibilityInquiryService)(this)).InquireMemberEligibilityAsync(inValue);
            }
        }
    }
