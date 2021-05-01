using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PowerAutomate.PackageManager.Models {
    #region SubArea

    [DataContract]
    public class OpenSubArea : PowerAutomateAction {
        [DataMember(Name = "body/area")]
        public string area { get; set; }
        [DataMember(Name="body/subArea")]
        public string subArea { get; set; }
    }
    #endregion

    #region Initialize Browser
    [DataContract]
    public class InitializeBrowser: PowerAutomateAction {
        [DataMember(Name = "body/browser")]
        public string browser { get; set; }
        [DataMember(Name = "body/incognito")]
        public bool incognito { get; set; }
        [DataMember(Name = "body/defaultThinkTime")]
        public string defaultThinkTime { get; set; }
    }
    #endregion

    #region Login
    [DataContract]
    public class Login : PowerAutomateAction {
        [DataMember(Name = "body/uri")]
        public string uri { get; set; }
        [DataMember(Name = "body/username")]
        public string username { get; set; }
        [DataMember(Name = "body/password")]
        public string password { get; set; }
    }
    #endregion

    #region OpenApp
    [DataContract]
    public class OpenApp : PowerAutomateAction {
        [DataMember(Name = "body/name")]
        public string name { get; set; }
    }
    #endregion

    #region End
    [DataContract]
    public class End : PowerAutomateAction {
        [DataMember(Name = "body/area")]
        public string area { get; set; }
        [DataMember(Name = "body/subArea")]
        public string subArea { get; set; }
    }
    #endregion

    #region OpenRecord
    [DataContract]
    public class OpenRecord : PowerAutomateAction {
        [DataMember(Name = "body/ViewKey")]
        public string viewKey { get; set; }
    }
    #endregion

    #region SetValue
    [DataContract]
    public class SetValue : PowerAutomateAction {
        [DataMember(Name = "body/field")]
        public string field { get; set; }
        [DataMember(Name = "body/value")]
        public string value { get; set; }
    }
    #endregion

    #region GetValue
    [DataContract]
    public class GetValue : PowerAutomateAction {
        [DataMember(Name = "body/area")]
        public string area { get; set; }
        [DataMember(Name = "body/subArea")]
        public string subArea { get; set; }
    }
    #endregion

    #region Assert
    [DataContract]
    public class Assert : PowerAutomateAction {
        [DataMember(Name = "body/field")]
        public string field { get; set; }
        [DataMember(Name = "body/comparsion")]
        public string comparsion { get; set; }
        [DataMember(Name = "body/value")]
        public string value { get; set; }
        [DataMember(Name = "body/errorMessage")]
        public string errorMessage { get; set; }
    }
    #endregion

    #region Condition
    [DataContract]
    public class Condition : PowerAutomateAction {
        [DataMember(Name = "body/field")]
        public string field { get; set; }
        [DataMember(Name = "body/comparsion")]
        public string comparsion { get; set; }
        [DataMember(Name = "body/value")]
        public string value { get; set; }
        [DataMember(Name = "body/errorMessage")]
        public string errorMessage { get; set; }
    }
    #endregion

    #region Save
    [DataContract]
    public class Save : PowerAutomateAction {
        [DataMember(Name = "body/area")]
        public string area { get; set; }
        [DataMember(Name = "body/subArea")]
        public string subArea { get; set; }
    }
    #endregion 

    [DataContract]
    public class PowerAutomateFlow {
        [DataMember]
        public Properties properties { get; set; }
    }

    public class Properties {
        [DataMember]
        public Definition definition { get; set; }
    }

    public class Definition {
        [DataMember]
        public PowerAutomateAction actions { get; set; }
    }

    public class LoginDefinition {
        [DataMember]
        public string correlationid { get; set; }
    }

    public class PowerAutomateAction {
        [DataMember]
        public RunAfter runAfter { get; set; }
        //[DataMember]
        //public RunAfter runAfter { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public Inputs inputs { get; set; }
    }

    public class Inputs {
        [DataMember]
        public Parameters parameters { get; set; }
        //[DataMember]
        //public string runAfterStatus { get; set; }

    }
    public class Parameters {
        //[DataMember]
        //public string name { get; set; }
        //[DataMember]
        //public string runAfterStatus { get; set; }

    }

    public class RunAfter {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string runAfterStatus { get; set; }

    }

}
