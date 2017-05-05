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
using MoonUnit.Attributes;
using MoonUnit.Exceptions;
using MoonUnit.InternalClasses;
using Utilities.Profiler;
using Utilities.Reflection.ExtensionMethods;
using MoonUnit.BaseClasses;
#endregion

namespace MoonUnit
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
            try
            {
                Configuration = Utilities.Configuration.ConfigurationManager.GetConfigFile<Configuration.Configuration>("MoonUnit");
            }
            catch
            {
                Utilities.Configuration.ConfigurationManager.RegisterConfigFile(new Configuration.Configuration());
                Configuration = Utilities.Configuration.ConfigurationManager.GetConfigFile<Configuration.Configuration>("MoonUnit");
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Tests an assembly
        /// </summary>
        /// <param name="AssemblyToTest">Assembly to test</param>
        /// <returns>The XML string of the results</returns>
        public string Test(Assembly AssemblyToTest)
        {
            Type[] Types = AssemblyToTest.GetTypes();
            return Test(Types);
        }

        /// <summary>
        /// Test a list of types
        /// </summary>
        /// <param name="Types">Types to test</param>
        /// <returns>The XML string of the results</returns>
        public string Test(Type[] Types)
        {
            Output TempOutput = new Output();
            foreach (Type Type in Types)
                TestClass(Type, TempOutput);
            return TempOutput.ToString();
        }

        /// <summary>
        /// Tests a type
        /// </summary>
        /// <param name="Type">Type to test</param>
        /// <returns>The XML string of the results</returns>
        public string Test(Type Type)
        {
            Output TempOutput = new Output();
            TestClass(Type, TempOutput);
            return TempOutput.ToString();
        }

        private static void TestClass(Type Type, Output TempOutput)
        {
            MethodInfo[] Methods = Type.GetMethods();
            foreach (MethodInfo Method in Methods)
            {
                object[] Attributes = Method.GetCustomAttributes(false);
                foreach (Attribute TempAttribute in Attributes)
                {
                    if (TempAttribute is TestAttribute)
                    {
                        TestAttribute Attribute = (TestAttribute)TempAttribute;
                        if (!Attribute.Skip)
                        {
                            try
                            {
                                StopWatch Watch = new StopWatch();
                                if (Type.IsOfType(typeof(IDisposable)))
                                {
                                    using (IDisposable TempClass = (IDisposable)Type.Assembly.CreateInstance(Type.FullName))
                                    {
                                        Watch.Start();
                                        Method.Invoke(TempClass, Type.EmptyTypes);
                                        Watch.Stop();
                                        if (Attribute.TimeOut > 0 && Watch.ElapsedTime > Attribute.TimeOut)
                                            throw new TimeOut("Method took longer than expected");
                                        TempOutput.MethodCalled(Method);
                                    }
                                }
                                else
                                {
                                    object TestClass = Type.Assembly.CreateInstance(Type.FullName);
                                    Watch.Start();
                                    Method.Invoke(TestClass, Type.EmptyTypes);
                                    Watch.Stop();
                                    if (Attribute.TimeOut > 0 && Watch.ElapsedTime > Attribute.TimeOut)
                                        throw new TimeOut("Method took longer than expected");
                                    TempOutput.MethodCalled(Method);
                                }
                            }
                            catch (Exception e)
                            {
                                if (e.InnerException != null && e.InnerException.IsOfType(typeof(BaseException)))
                                    TempOutput.MethodCalled(Method, e.InnerException);
                                else if(e.IsOfType(typeof(BaseException)))
                                    TempOutput.MethodCalled(Method, e);
                                else if (e.InnerException != null)
                                    TempOutput.MethodCalled(Method, new Generic(e.InnerException));
                                else
                                    TempOutput.MethodCalled(Method, new Generic(e));
                            }
                        }
                        else
                        {
                            TempOutput.MethodCalled(Method, new Skipped(Attribute.ReasonForSkipping));
                        }
                    }
                }
            }
        }

        #endregion

        #region Properties

        private Configuration.Configuration Configuration { get; set; }

        #endregion
    }
}