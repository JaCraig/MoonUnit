using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using Utilities.IO.ExtensionMethods;
using Utilities.Environment;

namespace UnitTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MoonUnit console test runner");
            Console.WriteLine("Copyright (C) 2011 James Craig");
            if (args.Length == 0)
            {
                PrintInstructions();
                return;
            }
            Utilities.Environment.ArgsParser Parser = new Utilities.Environment.ArgsParser();
            List<Option> Options = Parser.Parse(args);
            string AssemblyFile = "";
            bool HTML = false;
            bool ShowAfter = true;
            foreach (Option Option in Options)
            {
                if (Option.Command.ToLower() == "assembly" && Option.Parameters.Count > 0)
                {
                    AssemblyFile = Option.Parameters[0].Replace("\"", "");
                    Console.WriteLine("Testing: " + AssemblyFile);
                }
                else if (Option.Command.ToLower() == "html")
                {
                    HTML = true;
                }
                else if (Option.Command.ToLower() == "dontshowafter")
                {
                    ShowAfter = false;
                }
            }
            if (string.IsNullOrEmpty(AssemblyFile))
            {
                PrintInstructions();
                return;
            }
            string FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Result." + (HTML ? "html" : "xml");
            foreach (Option Option in Options)
            {
                if (Option.Command.ToLower() == "file")
                {
                    FileName = Option.Parameters[0].Replace("\"", "");
                }
            }
            Utilities.Configuration.ConfigurationManager.RegisterConfigFile(typeof(Config).Assembly);
            List<FileInfo> Files = new DirectoryInfo(Path.GetDirectoryName(AssemblyFile)).GetFiles().ToList();
            foreach (FileInfo File in Files)
            {
                if (File.Extension.ToLower() == ".dll" && File.FullName != AssemblyFile)
                {
                    AssemblyName Name = AssemblyName.GetAssemblyName(File.FullName);
                    AppDomain.CurrentDomain.Load(Name);
                }
            }
            AssemblyName Name2 = AssemblyName.GetAssemblyName(AssemblyFile);
            Assembly TestAssembly = AppDomain.CurrentDomain.Load(Name2);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            string Output = MoonUnitLoader.Manager.Instance.Test(TestAssembly);
            if (HTML)
            {
                string XSLTFileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\HTML.xslt";
                Output = Transform(Output, XSLTFileName);
            }
            new FileInfo(FileName).Save(Output);
            if(ShowAfter)
                System.Diagnostics.Process.Start(FileName);
        }

        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == args.Name);
        }

        private static string Transform(string Output, string XSLT)
        {
            using (StreamReader Reader = new StreamReader(XSLT))
            {
                XPathDocument doc = new XPathDocument(new StringReader(Output.Replace("`", "'")));
                XslCompiledTransform xslTransform = new XslCompiledTransform();
                XmlTextReader transformReader = new XmlTextReader(Reader);
                xslTransform.Load(transformReader);

                using (StringWriter Writer = new StringWriter())
                {
                    xslTransform.Transform(doc, null, Writer);
                    return Writer.ToString();
                }
            }
        }

        private static void PrintInstructions()
        {
            string Name = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine("Usage: {0} [options]", Name);
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("     /Assembly <AssemblyFileLocation>    Specifies the assembly to test");
            Console.WriteLine("     /File <FileLocation>                Outputs the results to the file specified");
            Console.WriteLine("     /HTML                               Outputs the results to HTML");
            Console.WriteLine();
        }
    }

    public class Config : MoonUnitLoader.Configuration.Configuration
    {
        protected override string ConfigFileLocation { get { return "./Loader.config"; } }
        public override string AssemblyLocation { get { return _AssemblyLocation; } set { _AssemblyLocation = value; } }
        private string _AssemblyLocation = "";
    }
}