﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;

namespace RIAPP.DataService.Utils
{
    public static class DiffGram
    {
        private static Dictionary<string, object> GetValues(Type t, object obj, string[] propNames)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            if (obj == null)
                return res;
            foreach (string name in propNames)
            {
                PropertyInfo pi = t.GetProperty(name);
                res.Add(pi.Name, pi.GetValue(obj, null));
            }
            return res;
        }

        private struct Vals
        {
            public string Name
            {
                get;
                set;
            }

            public string Val1
            {
                get;
                set;
            }

            public string Val2
            {
                get;
                set;
            }
        }

        public static string GetDiffGram(Dictionary<string, object> d1, Dictionary<string, object> d2, Type t)
        {
            LinkedList<Vals> lst = new LinkedList<Vals>();

            foreach (string pnm in d1.Keys.Intersect(d2.Keys))
            {
                object val1 = d1[pnm];
                object val2 = d2[pnm];

                if (val2 != null && val1 != null)
                {
                    if (!val2.ToString().Equals(val1.ToString(), StringComparison.Ordinal))
                    {
                        lst.AddLast(new Vals
                        {
                            Val1 = Convert.ToString(val1, System.Globalization.CultureInfo.InvariantCulture),
                            Val2 = Convert.ToString(val2, System.Globalization.CultureInfo.InvariantCulture),
                            Name = pnm
                        });
                    }
                }
                else if (val1 == null && val2 != null)
                {
                    lst.AddLast(new Vals { Val1 = "NULL", Val2 = Convert.ToString(val2, System.Globalization.CultureInfo.InvariantCulture), Name = pnm });
                }
                else if (val1 != null && val2 == null)
                {
                    lst.AddLast(new Vals { Val1 = Convert.ToString(val1, System.Globalization.CultureInfo.InvariantCulture), Val2 = "NULL", Name = pnm });
                }
            }

            foreach (string pnm in d1.Keys.Except(d2.Keys))
            {
                object val1 = d1[pnm];
                if (val1 != null)
                {
                    lst.AddLast(new Vals { Val1 = Convert.ToString(val1, System.Globalization.CultureInfo.InvariantCulture), Val2 = "", Name = pnm });
                }
                else if (val1 == null)
                {
                    lst.AddLast(new Vals { Val1 = "NULL", Val2 = "", Name = pnm });
                }
            }

            foreach (string pnm in d2.Keys.Except(d1.Keys))
            {
                object val2 = d2[pnm];
                if (val2 != null)
                {
                    lst.AddLast(new Vals { Val1 = "", Val2 = Convert.ToString(val2, System.Globalization.CultureInfo.InvariantCulture), Name = pnm });
                }
                else if (val2 == null)
                {
                    lst.AddLast(new Vals { Val1 = "", Val2 = "NULL", Name = pnm });
                }
            }

            XElement x = new XElement("changes",
                 from v in lst
                 select new XElement(v.Name,
                      new XAttribute("old", v.Val1),
                      new XAttribute("new", v.Val2))
            );
            return x.ToString();
        }

        public static string GetDiffGram(object obj1, object obj2, Type t, string[] propNames)
        {
            Dictionary<string, object> d1 = GetValues(t, obj1, propNames);
            Dictionary<string, object> d2 = GetValues(t, obj2, propNames);
            return GetDiffGram(d1, d2, t);
        }

    }
}
