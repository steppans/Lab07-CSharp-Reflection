using Animals;
using System.Reflection;
using System.Xml;

// Загрузим сборку библиотеки Animal
Assembly animalAssembly = Assembly.Load(new AssemblyName("Animals"));

// Объект класса для работы с XML файлами
XmlDocument xmlDocument = new XmlDocument();

// Корень XML файла - это первый class сборки (Animal)
Type abstractType = (from type in animalAssembly.ExportedTypes
                  where type.IsAbstract
                  select type).First();

XmlElement root = xmlDocument.CreateElement(abstractType.Name);

// Создфём атрибут для Animal
XmlAttribute animalComment = xmlDocument.CreateAttribute("comment");
animalComment.Value = abstractType.GetCustomAttributes().OfType<MyAttribute>().First().Comment;

root.Attributes.Append(animalComment);

// Создаём узлы xRoot
//---------------------------------------------------------------------------------------------------------
var inheritors = (from type in animalAssembly.ExportedTypes
                  where !type.IsAbstract && type.IsClass
                  select type).Take(3);

foreach (var inheritor in inheritors)
{
    XmlElement animal = xmlDocument.CreateElement(inheritor.Name);

    XmlAttribute comment = xmlDocument.CreateAttribute("comment");
    comment.Value = inheritor.GetCustomAttributes().OfType<MyAttribute>().First().Comment;
    animal.Attributes.Append(comment);

    foreach(var property in inheritor.GetProperties())
    {
        XmlElement animalProperty = xmlDocument.CreateElement(property.Name);
        if (property.Name == "WhatAnimal")
        {
            animalProperty.AppendChild(xmlDocument.CreateTextNode(animal.Name));
            animal.AppendChild(animalProperty);
            continue;
        }
        animalProperty.AppendChild(xmlDocument.CreateTextNode(property.Name));
        animal.AppendChild(animalProperty);
    }


    root.AppendChild(animal);
}
//---------------------------------------------------------------------------------------------------------

// Узел с методами классов
//---------------------------------------------------------------------------------------------------------
var methods = from method in abstractType.GetMethods()
              where !method.IsSpecialName && method.DeclaringType?.Name != "Object"
              select method;
XmlElement animalMethods = xmlDocument.CreateElement("Methods");
string methodsNote = "";
foreach(var m in methods)
{
    methodsNote += m.Name + " ";
}
animalMethods.AppendChild(xmlDocument.CreateTextNode(methodsNote));
root.AppendChild(animalMethods);
//---------------------------------------------------------------------------------------------------------


// Создаём дополнительный узел с enum типами
//---------------------------------------------------------------------------------------------------------
XmlElement enumElement = xmlDocument.CreateElement("EnumTypes");
XmlAttribute enumComment = xmlDocument.CreateAttribute("comment");
enumComment.Value = "New enum types for data of animals";
enumElement.Attributes.Append(enumComment);

var enums = from type in animalAssembly.ExportedTypes
                  where type.IsEnum
                  select type;

foreach(Type e in enums)
{
    XmlElement enumType = xmlDocument.CreateElement(e.Name);
    XmlAttribute comment = xmlDocument.CreateAttribute("comment");
    comment.Value = e.GetCustomAttribute<MyAttribute>()?.Comment;
    enumType.Attributes.Append(comment);

    string types = "";
    foreach(string t in e.GetEnumNames())
    {
        types += t + " ";
    }
    enumType.AppendChild(xmlDocument.CreateTextNode(types));
    enumElement.AppendChild(enumType);
}
//---------------------------------------------------------------------------------------------------------

root.AppendChild(enumElement);
xmlDocument.AppendChild(root);

xmlDocument.Save(@"C:\Users\stepp\source\repos\Lab07(Reflection)\GenerateXML\AnimalXML.xml");

Console.WriteLine("Data saved");


