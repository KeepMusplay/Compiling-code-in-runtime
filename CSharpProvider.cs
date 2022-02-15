using System;
using System.IO;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Example
{
    class CSharpProvider
    {

        public static Object CreateClass(string nameClass, string codeClass)
        {
            if (File.Exists(@"C:\Server\Example\bin\Debug\" + nameClass + ".cs"))
            {
                File.Delete(@"C:\Server\Example\bin\Debug\" + nameClass + ".cs");
            }

            using (FileStream stream = File.Create(@"C:\Server\Example\bin\Debug\" + nameClass + ".cs"))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(codeClass);
                stream.Write(bytes, 0, bytes.Length);
            }


            using (FileStream stream = File.OpenRead($@"C:\Server\Example\bin\Debug\" + nameClass + ".cs"))
            {
                byte[] b = new byte[1024];

                UTF8Encoding temp = new UTF8Encoding(true);

                while (stream.Read(b, 3, b.Length - 3) > 0)
                {
                    Console.WriteLine(temp.GetString(b));
                }
            }

            CompilerResults compilerResults;

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            CompilerParameters cp = new CompilerParameters();

            cp.GenerateExecutable = false;

            cp.ReferencedAssemblies.Add("System.dll");


            compilerResults = provider.CompileAssemblyFromFile(cp, nameClass + ".cs");

            Console.WriteLine("Errors: " + compilerResults.Errors.Count);
            foreach (var something in compilerResults.Errors)
            {
                Console.WriteLine(something.ToString());
            }

            Assembly assem = compilerResults.CompiledAssembly;

            Object t = assem.GetType("Example." + nameClass);

            return t;
        }
    }
}
