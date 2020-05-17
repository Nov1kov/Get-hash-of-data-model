using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using AutoBogus;
using SharpObjectGenerator.AutoBogusExtensions;

namespace SharpObjectGenerator
{
    public class ObjectGenerator
    {
        public object GeneratedObj { get; private set; }
        private TypeImplementationOverride _typeImplementationOverride = new TypeImplementationOverride();

        /// <summary>
        /// Для создания фейкового объекта, ему нужно пометить
        /// Каким классом должен реализовываться абстракция
        /// </summary>
        /// <typeparam name="TImpl">Конкретная реализация</typeparam>
        /// <typeparam name="TBase">Интерфейс или абстрактный класс</typeparam>
        public void AddTypeMap<TBase, TImpl>() where TImpl : TBase
        {
            _typeImplementationOverride.AddTypeMap<TBase, TImpl>();
        }
        
        public T Generate<T>()
        {
            var faker = AutoFaker.Create(builder => 
                builder.WithOverride(new PrimitiveGeneratorOverride()).
                    WithOverride(_typeImplementationOverride).
                    WithBinder(new SkipNoSerializeBinder())
            );
            
            var obj = faker.Generate<T>();
            GeneratedObj = obj;
            return obj;
        }

        public int GetHash()
        {
            if (GeneratedObj == null)
                throw new Exception("Generate object before");

            return GetHash(GeneratedObj);
        }
        
        public static int GetHash(object obj)
        {
            int hashCode = 0;
            using (var memoryStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);
                memoryStream.Position = 0;
                byte[] buffer = new byte[16384];
                int count;
                while ((count = memoryStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (var i = 0; i < count; i++)
                        hashCode =  hashCode * 31 + buffer[i];
                }
            }

            return hashCode;
        }
    }
}