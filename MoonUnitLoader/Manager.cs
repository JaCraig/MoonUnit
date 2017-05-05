/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.DataTypes.Patterns.BaseClasses;
using System.Reflection;
using MoonUnitLoader.Configuration;
#endregion

namespace MoonUnitLoader
{
    /// <summary>
    /// Manager class for unit testing framework
    /// </summary>
    public class Manager : Singleton<Manager>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected Manager()
            : base()
        {
            Configuration = Utilities.Configuration.ConfigurationManager.GetConfigFile<Configuration.Configuration>("MoonUnitLoader");
            if (string.IsNullOrEmpty(Configuration.AssemblyLocation))
            {
                Configuration.AssemblyLocation = System.IO.Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath) + "\\MoonUnit.dll";
                Configuration.Save();
            }
            AssemblyName Name = AssemblyName.GetAssemblyName(Configuration.AssemblyLocation);
            MoonUnitAssembly = AppDomain.CurrentDomain.Load(Name);
            MoonUnitManagerType = MoonUnitAssembly.GetType("MoonUnit.Manager", true, true);
            PropertyInfo InstanceProperty = MoonUnitManagerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            MethodInfo Method = InstanceProperty.GetGetMethod();
            MoonUnitManager = Method.Invoke(null, new object[] { });
            TestAssembly = MoonUnitManagerType.GetMethod("Test", new Type[] { typeof(Assembly) });
            TestArray = MoonUnitManagerType.GetMethod("Test", new Type[] { typeof(Type[]) });
            TestType = MoonUnitManagerType.GetMethod("Test", new Type[] { typeof(Type) });
        }

        #endregion

        #region Function

        /// <summary>
        /// Tests an assembly
        /// </summary>
        /// <param name="AssemblyToTest">Assembly to test</param>
        /// <returns>XML string containing the results</returns>
        public string Test(Assembly AssemblyToTest)
        {
            return (string)TestAssembly.Invoke(MoonUnitManager, new object[] { AssemblyToTest });
        }

        /// <summary>
        /// Tests a list of types
        /// </summary>
        /// <param name="Types">Types to test</param>
        /// <returns>XML string containing the results</returns>
        public string Test(Type[] Types)
        {
            return (string)TestArray.Invoke(MoonUnitManager, new object[] { Types });
        }

        /// <summary>
        /// Tests a type
        /// </summary>
        /// <param name="Type">Type to test</param>
        /// <returns>XML string containing the results</returns>
        public string Test(Type Type)
        {
            return (string)TestType.Invoke(MoonUnitManager, new object[] { Type });
        }

        #endregion

        #region Properties

        private Configuration.Configuration Configuration { get; set; }
        private Assembly MoonUnitAssembly { get; set; }
        private object MoonUnitManager { get; set; }
        private Type MoonUnitManagerType { get; set; }
        private MethodInfo TestAssembly { get; set; }
        private MethodInfo TestArray { get; set; }
        private MethodInfo TestType { get; set; }

        #endregion
    }
}
