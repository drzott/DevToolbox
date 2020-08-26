using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

namespace ReadADInfos
{
    public class User
    {
        SearchResult _sr = null;

        public List<string> AvailableProperties = new List<string>();

        public User(SearchResult s)
        {
            if (s == null)
                return;

            _sr = s;

            if(s.Properties != null && s.Properties.Count > 0 && s.Properties.PropertyNames != null && s.Properties.PropertyNames.Count > 0)
            {
                foreach(string p in _sr.Properties.PropertyNames)
                {
                    AvailableProperties.Add(p);
                }
            }
        }

        public string Wildcard
        {


            get
            {
                if (AvailableProperties.Contains(ReadADInfos.Properties.Settings.Default.Wildcard))
                {
                    if (_sr.Properties[ReadADInfos.Properties.Settings.Default.Wildcard] != null && _sr.Properties[ReadADInfos.Properties.Settings.Default.Wildcard].Count > 0)
                        return _sr.Properties[ReadADInfos.Properties.Settings.Default.Wildcard][0].ToString();
                }

                return string.Empty;
            }
        }


        public int UAC
        {

            //  512 = normal account
            //  514 = 512 + 2 = normal account, disabled
            //  546 = 512 + 32 + 2 = normal account, disabled, no password required
            // 2080 = 2048 + 32 = Interdomain trust, no password required
            //66048 = 65536 + 512 = normal account. password never expires
            //66050 = 65536 + 512 + 2 = normal account. password never expires, disabled
            //66080 = 65536 + 512 + 32 = normal account. password never expires, no password required

            get
            {
                if (AvailableProperties.Contains("useraccountcontrol"))
                {
                    if (_sr.Properties["useraccountcontrol"] != null && _sr.Properties["useraccountcontrol"].Count > 0)
                        return Convert.ToInt32(_sr.Properties["useraccountcontrol"][0]);
                }

                return 0;
            }
        }

        public string EMail
        {
            get
            {
                if (AvailableProperties.Contains("mail"))
                {
                    if(_sr.Properties["mail"] != null && _sr.Properties["mail"].Count > 0)
                        return _sr.Properties["mail"][0].ToString();
                }

                return string.Empty;
            }
        }

        public string Department
        {
            get
            {
                if (AvailableProperties.Contains("department"))
                {
                    if (_sr.Properties["department"] != null && _sr.Properties["department"].Count > 0)
                        return _sr.Properties["department"][0].ToString();
                }

                return string.Empty;
            }
        }


        public string Firstname
        {
            get
            {
                if (AvailableProperties.Contains("givenname"))
                {
                    if (_sr.Properties["givenname"] != null && _sr.Properties["givenname"].Count > 0)
                        return _sr.Properties["givenname"][0].ToString();
                }

                return string.Empty;
            }
        }

        public string Lastname
        {
            get
            {
                if (AvailableProperties.Contains("name"))
                {
                    if (_sr.Properties["name"] != null && _sr.Properties["name"].Count > 0 && _sr.Properties["givenname"] != null && _sr.Properties["givenname"].Count > 0)
                    {
                        string str = _sr.Properties["givenname"][0].ToString();
                        string str2 = _sr.Properties["name"][0].ToString();

                        return (str2.Replace(str, "")).Trim();
                    }
                }

                return string.Empty;
            }
        }



        public string SamAccountName
        {
            get
            {
                if (AvailableProperties.Contains("samaccountname"))
                {
                    if (_sr.Properties["samaccountname"] != null && _sr.Properties["samaccountname"].Count > 0)
                        return _sr.Properties["samaccountname"][0].ToString();
                }

                return string.Empty;
            }
        }


        public List<string> Groups
        {
            get
            {
                if (AvailableProperties.Contains("memberof"))
                {
                    if (_sr.Properties["memberof"] != null)
                    {
                        List<string> r = new List<string>();
                        foreach(string s in _sr.Properties["memberof"])
                        {
                            r.Add(s.Substring(3, s.Length - 3));
                        }

                        return r;
                    }
                        
                }

                return new List<string>();
            }
        }


    }
}
