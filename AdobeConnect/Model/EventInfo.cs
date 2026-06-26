/*
Copyright 2007-2014 Dmitry Stroganov (dmitrystroganov.dk)
Redistributions of any form must retain the above copyright notice.
 
Use of any commands included in this SDK is at your own risk. 
Dmitry Stroganov cannot be held liable for any damage through the use of these commands.
*/

using System;
using System.Xml.Serialization;
using AdobeConnectSDK.Common;

namespace AdobeConnectSDK.Model
{  
  /// <summary>
  /// Event information 
  /// </summary>
  [Serializable]
  public class EventInfo : XmlDateTimeBase
  {
    [XmlAttribute("sco-id")]
    public string ScoId;

    [XmlAttribute("tree-id")]
    public int TreeId;

    [XmlIgnore]
    public SCOtype ItemType;

    [XmlAttribute("type")]
    public string ItemTypeRaw
    {
      get
      {
        return Helpers.EnumToString(this.ItemType);
      }
      set
      {
        this.ItemType = Helpers.ReflectEnum<SCOtype>(value);
      }
    }

    [XmlIgnore]
    public PermissionId PermissionId;

    [XmlAttribute("permission-id")]
    public string PermissionIdRaw
    {
      get { return Helpers.EnumToString(this.PermissionId); }
      set
      {
        this.PermissionId = Helpers.ReflectEnum<PermissionId>(value);
      }
    }

    [XmlElement("name")]
    public string Name;

    [XmlElement("domain-name")]
    public string DomainName;

    [XmlElement("url-path")]
    public string UrlPath;

    [XmlElement("expired")]
    public bool Expired;

    [XmlElement]
    public TimeSpan Duration;
  }
    public class EventInfoViewModel : EventInfo
    {
        public string ScoIdVm=> ScoId;
        public int TreeIdVm=> TreeId;
        public SCOtype ItemTypeVm=> ItemType;
        public string ItemTypeRawVm=> ItemTypeRaw;
        public PermissionId PermissionIdVm=> PermissionId;
        public string PermissionIdRawVm=> PermissionIdRaw;
        public string NameVm=> Name;
        public string DomainNameVm=> DomainName;
        public string UrlPathVm=> UrlPath;
        public bool ExpiredVm=> Expired;
        public TimeSpan DurationVm=> Duration;
    }
}
