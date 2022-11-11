using System.Reflection;
using System.Xml.Schema;
using Animals;

Assembly animalAssembly = Assembly.Load(new AssemblyName("Animals"));

animalAssembly.GetCustomAttribute(typeof(MyAttribute));

foreach(Type type in animalAssembly.ExportedTypes)
{
    if (type.GetTypeInfo().IsClass || type.GetTypeInfo().IsEnum)
    {
        Console.WriteLine(type.Name);
    }
}

