using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;

/*
 Сериализация представляет процесс преобразования какого-либо объекта в поток байтов. 
 После преобразования мы можем этот поток байтов или записать на диск или сохранить его временно в памяти. 
 А при необходимости можно выполнить обратный процесс - десериализацию, 
 то есть получить из потока байтов ранее сохраненный объект.
 Чтобы объект определенного класса можно было сериализовать, надо этот класс пометить атрибутом Serializable.
 Если мы не хотим, чтобы какой-то член класса сериализовался, то мы его помечаем атрибутом NonSerialized.
    В .NET можно использовать следующие форматы:
    - SOAP
    - xml
    - JSON
 Для каждого формата предусмотрен свой класс: 
 * для формата SOAP - класс SoapFormatter, 
 * для xml - XmlSerializer, 
 * для json - DataContractJsonSerializer
 * 
 Протокол SOAP (Simple Object Access Protocol) представляет простой протокол для обмена данными между различными платформами. 
 При такой сериализации данные упакуются в конверт SOAP, данные в котором имеют вид xml-подобного документа.
 * 
 JSON (JavaScript Object Notation) — текстовый формат обмена данными, основанный на JavaScript
 и обычно используемый именно с этим языком. Чтобы пометить класс как сериализуемый, к нему применяется атрибут DataContract, 
 а ко всем его сериализуемым свойствам - атрибут DataMember.
 */
namespace serialization
{
    // для XML-сериализации необходим открытый доступ к классу
    [Serializable]
    [DataContract]
    public class Person
    {
        [DataMember]
        public string? Name { get; set; }

        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public Company? Company { get; set; }

        public Person(string name, int age, Company comp)
        {
            Name = name;
            Age = age;
            Company = comp;
        }
        // для XML-сериализации необходим конструктор по умолчанию
        public Person()
        {

        }
    }

    [Serializable]
    [DataContract]
    public class Company
    {
        [DataMember]
        public string? Name { get; set; }

        // для XML-сериализации необходим конструктор по умолчанию
        public Company() { }

        public Company(string name)
        {
            Name = name;
        }
    }

    [Serializable]
    [KnownType(typeof(Flash))]
    [KnownType(typeof(DVD))]
    [KnownType(typeof(HDD))]
    [XmlInclude(typeof(Flash))]
    [XmlInclude(typeof(DVD))]
    [XmlInclude(typeof(HDD))]
    [DataContract]
    public abstract class Storage
    {
        [DataMember]
        public string? Type { get; set; }
        [DataMember]
        public string? Name { get; set; }
        [DataMember]
        public string? Model { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int Price { get; set; }

        public Storage()
        {
            Name = "Name";
            Model = "Model";
            Quantity = 0;
            Price = 0;
        }

        public virtual void Print()
        {
            Console.WriteLine("Name: {0}  Model: {1}  Quantity: {2}  Price: {3}", Name, Model, Quantity, Price);
        }
        public virtual void Save() { }
        public virtual void Load() { }
    }

    [Serializable]
    [DataContract]
    public class DVD : Storage
    {
        [DataMember]
        public string WriteSpeed { get; set; }
        [DataMember]
        public string ReadSpeed { get; set; }

        public DVD() : base()
        {
            Type = "DVD";
            WriteSpeed = "0x";
            ReadSpeed = "0x";
        }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Type: {0}  WriteSpeed: {1}  ReadSpeed: {2}", Type, WriteSpeed, ReadSpeed);
        }
    }

    [Serializable]
    [DataContract]
    public class HDD : Storage
    {
        [DataMember]
        public int CapacityHDD { get; set; }
        [DataMember]
        public int SpeedHDD { get; set; }

        public HDD() : base()
        {
            Type = "HDD";
            CapacityHDD = 0;
            SpeedHDD = 0;
        }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Type: {0}  CapacityHDD: {1}  ReadSpeed: {2}", Type, CapacityHDD, SpeedHDD);
        }
    }

    [Serializable]
    [DataContract]
    public class Flash : Storage
    {
        [DataMember]
        public int CapacityUSB { get; set; }
        [DataMember]
        public int SpeedUSB { get; set; }

        public Flash() : base()
        {
            Type = "Flash";
            CapacityUSB = 0;
            SpeedUSB = 0;
        }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Type: {0}  CapacityUSB: {1}  SpeedUSB: {2}", Type, CapacityUSB, SpeedUSB);
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                char answer;
                FileStream? stream = null;
                SoapFormatter? soap = null;
                XmlSerializer? serializer = null;
                DataContractJsonSerializer? jsonFormatter = null;
                Person[]? arr = null;
                Person? h = null;
                List<int>? l = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
                do
                {
                    Console.WriteLine("1.Сериализация массива объектов (SOAP-форматтер)");
                    Console.WriteLine("2.Десериализация массива объектов (SOAP-форматтер)");
                    Console.WriteLine("3.Сериализация коллекции (SOAP-форматтер)");
                    Console.WriteLine("4.Десериализация коллекции (SOAP-форматтер)");
                    Console.WriteLine("5.XML-сериализация объекта");
                    Console.WriteLine("6.XML-десериализация объекта");
                    Console.WriteLine("7.XML-сериализация коллекции");
                    Console.WriteLine("8.XML-десериализация коллекции");
                    Console.WriteLine("9.XML-сериализация массива объектов");
                    Console.WriteLine("10.XML-десериализация массива объектов");
                    Console.WriteLine("11.JSON-сериализация объекта");
                    Console.WriteLine("12.JSON-десериализация объекта");
                    Console.WriteLine("13.JSON-сериализация коллекции");
                    Console.WriteLine("14.JSON-десериализация коллекции");
                    Console.WriteLine("15.JSON-сериализация массива объектов");
                    Console.WriteLine("16.JSON-десериализация массива объектов");
                    Console.WriteLine("17.JSON-сериализация объектов производных классов");
                    Console.WriteLine("18.JSON-десериализация объектов производных классов");
                    Console.WriteLine("19.XML-сериализация объектов производных классов");
                    Console.WriteLine("20.XML-десериализация объектов производных классов");
                    Console.WriteLine("21.Сериализация объектов производных классов (SOAP-форматтер)");
                    Console.WriteLine("22.Десериализация объектов производных классов (SOAP-форматтер)");
                    Console.WriteLine("Ваш выбор: ");
                    int choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            arr = [ new Person("Ларри Пейдж", 42, new Company("Google")),
                                new Person("Сатья Наделла", 48, new Company("Microsoft")),
                                new Person("Тим Кук", 55, new Company("Apple")) ];
                            stream = new FileStream("soap.xml", FileMode.Create);
                            soap = new SoapFormatter();
                            soap.Serialize(stream, arr);
                            stream.Close();
                            Console.WriteLine("Сериализация успешно выполнена!");
                            break;
                        case 2:
                            stream = new FileStream("soap.xml", FileMode.Open);
                            soap = new SoapFormatter();
                            arr = (Person[])soap.Deserialize(stream);
                            foreach (Person j in arr)
                            {
                                Console.WriteLine(j.Name + "	" + j.Age + "	" + j?.Company?.Name);
                            }
                            stream.Close();
                            break;
                        case 3:
                            stream = new FileStream("list.xml", FileMode.Create);
                            soap = new SoapFormatter();
                            soap.Serialize(stream, l);
                            stream.Close();
                            Console.WriteLine("Сериализация успешно выполнена!");
                            break;
                        case 4:
                            stream = new FileStream("list.xml", FileMode.Open);
                            soap = new SoapFormatter();
                            l = (List<int>)soap.Deserialize(stream);
                            string s = string.Empty;
                            foreach (int j in l)
                            {
                                s += j.ToString() + ',';
                            }
                            Console.WriteLine(s);
                            stream.Close();
                            break;
                        case 5:
                            h = new Person("Ларри Пейдж", 42, new Company("Google"));
                            stream = new FileStream("data.xml", FileMode.Create);
                            serializer = new XmlSerializer(typeof(Person));
                            serializer.Serialize(stream, h);
                            stream.Close();
                            Console.WriteLine("Сериализация успешно выполнена!");
                            break;
                        case 6:
                            stream = new FileStream("data.xml", FileMode.Open);
                            serializer = new XmlSerializer(typeof(Person));
                            h = serializer.Deserialize(stream) as Person;
                            Console.WriteLine(h?.Name + "	" + h?.Age + "	" + h?.Company?.Name);
                            stream.Close();
                            break;
                        case 7:
                            stream = new FileStream("list.xml", FileMode.Create);
                            serializer = new XmlSerializer(typeof(List<int>));
                            serializer.Serialize(stream, l);
                            stream.Close();
                            Console.WriteLine("Сериализация успешно выполнена!");
                            break;
                        case 8:
                            stream = new FileStream("list.xml", FileMode.Open);
                            serializer = new XmlSerializer(typeof(List<int>));
                            l = serializer.Deserialize(stream) as List<int>;
                            s = string.Empty;
                            foreach (int j in l)
                            {
                                s += j.ToString() + ',';
                            }
                            Console.WriteLine(s);
                            stream.Close();
                            break;
                        case 9:
                            arr = [ new Person("Ларри Пейдж", 42, new Company("Google")),
                                new Person("Сатья Наделла", 48, new Company("Microsoft")),
                                new Person("Тим Кук", 55, new Company("Apple"))  ];
                            stream = new FileStream("Person.xml", FileMode.Create);
                            serializer = new XmlSerializer(typeof(Person[]));
                            serializer.Serialize(stream, arr);
                            stream.Close();
                            Console.WriteLine("Сериализация успешно выполнена!");
                            break;
                        case 10:
                            stream = new FileStream("Person.xml", FileMode.Open);
                            serializer = new XmlSerializer(typeof(Person[]));
                            arr = serializer.Deserialize(stream) as Person[];
                            foreach (Person j in arr)
                            {
                                Console.WriteLine(j.Name + "	" + j.Age + "	" + j?.Company?.Name);
                            }
                            stream.Close();
                            break;
                        case 11:
                            h = new Person("Ларри Пейдж", 42, new Company("Google"));
                            stream = new FileStream("data.json", FileMode.Create);
                            jsonFormatter = new DataContractJsonSerializer(typeof(Person));
                            jsonFormatter.WriteObject(stream, h);
                            stream.Close();
                            Console.WriteLine("Сериализация успешно выполнена!");
                            break;
                        case 12:
                            stream = new FileStream("data.json", FileMode.Open);
                            jsonFormatter = new DataContractJsonSerializer(typeof(Person));
                            h = jsonFormatter.ReadObject(stream) as Person;
                            Console.WriteLine(h?.Name + "	" + h?.Age + "	" + h?.Company?.Name);
                            stream.Close();
                            break;
                        case 13:
                            stream = new FileStream("list.json", FileMode.Create);
                            jsonFormatter = new DataContractJsonSerializer(typeof(List<int>));
                            jsonFormatter.WriteObject(stream, l);
                            stream.Close();
                            Console.WriteLine("Сериализация успешно выполнена!");
                            break;
                        case 14:
                            stream = new FileStream("list.json", FileMode.Open);
                            jsonFormatter = new DataContractJsonSerializer(typeof(List<int>));
                            l = jsonFormatter.ReadObject(stream) as List<int>;
                            s = string.Empty;
                            foreach (int j in l)
                            {
                                s += j.ToString() + ',';
                            }
                            Console.WriteLine(s);
                            stream.Close();
                            break;
                        case 15:
                            arr = [ new Person("Ларри Пейдж", 42, new Company("Google")),
                                new Person("Сатья Наделла", 48, new Company("Microsoft")),
                                new Person("Тим Кук", 55, new Company("Apple"))  ];
                            stream = new FileStream("Person.json", FileMode.Create);
                            jsonFormatter = new DataContractJsonSerializer(typeof(Person[]));
                            jsonFormatter.WriteObject(stream, arr);
                            stream.Close();
                            Console.WriteLine("Сериализация успешно выполнена!");
                            break;
                        case 16:
                            stream = new FileStream("Person.json", FileMode.Open);
                            jsonFormatter = new DataContractJsonSerializer(typeof(Person[]));
                            arr = jsonFormatter.ReadObject(stream) as Person[];
                            foreach (Person j in arr)
                            {
                                Console.WriteLine(j.Name + "	" + j.Age + "	" + j?.Company?.Name);
                            }
                            stream.Close();
                            break;
                        case 17:
                            var storage = new Storage[3];
                            storage[0] = new DVD();
                            storage[1] = new HDD();
                            storage[2] = new Flash();
                            stream = new FileStream("device.json", FileMode.Create);
                            jsonFormatter = new DataContractJsonSerializer(typeof(Storage[]));
                            jsonFormatter.WriteObject(stream, storage);
                            stream.Close();
                            Console.WriteLine("Сериализация успешно выполнена!");
                            break;
                        case 18:
                            stream = new FileStream("device.json", FileMode.Open);
                            jsonFormatter = new DataContractJsonSerializer(typeof(Storage[]));
                            storage = jsonFormatter.ReadObject(stream) as Storage[];
                            for (int i = 0; i < storage?.Length; i++)
                            {
                                storage[i].Print();
                            }
                            Console.WriteLine();
                            stream.Close();
                            break;
                        case 19:
                            storage = new Storage[3];
                            storage[0] = new DVD();
                            storage[1] = new HDD();
                            storage[2] = new Flash();
                            stream = new FileStream("device.xml", FileMode.Create);
                            serializer = new XmlSerializer(typeof(Storage[]));
                            serializer.Serialize(stream, storage);
                            stream.Close();
                            Console.WriteLine("Сериализация успешно выполнена!");
                            break;
                        case 20:
                            stream = new FileStream("device.xml", FileMode.Open);
                            serializer = new XmlSerializer(typeof(Storage[]));
                            storage = serializer.Deserialize(stream) as Storage[];
                            for (int i = 0; i < storage?.Length; i++)
                            {
                                storage[i].Print();
                            }
                            Console.WriteLine();
                            stream.Close();
                            break;
                        case 21:
                            storage = new Storage[3];
                            storage[0] = new DVD();
                            storage[1] = new HDD();
                            storage[2] = new Flash();
                            stream = new FileStream("device.xml", FileMode.Create);
                            soap = new SoapFormatter();
                            soap.Serialize(stream, storage);
                            stream.Close();
                            Console.WriteLine("Сериализация успешно выполнена!");
                            break;
                        case 22:
                            stream = new FileStream("device.xml", FileMode.Open);
                            soap = new SoapFormatter();
                            storage = soap.Deserialize(stream) as Storage[];
                            for (int i = 0; i < storage?.Length; i++)
                            {
                                storage[i].Print();
                            }
                            Console.WriteLine();
                            stream.Close();
                            break;
                    }
                    Console.WriteLine("Продолжим? (y/n) ");
                    answer = Convert.ToChar(Console.ReadLine()!);
                    Console.Clear();
                }
                while (answer == 'y');
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
