using System;

namespace Pooling
{
    public class TypeSerializer
    {
        public static string Serialize(Type type)
        {
            return type.AssemblyQualifiedName;
        }

        public static Type Deserialize(string typeName)
        {
            return Type.GetType(typeName);
        }
    }
}